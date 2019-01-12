using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ItemBrowser.xaml
    /// </summary>
    public partial class ItemExplorerControl : UserControl, INavigationGuest, ICanUnregister
    {
        /// <summary>
        /// Stores the information about the active tab controls
        /// </summary>
        protected readonly ObservableCollection<ItemExplorerTab> Tabs = new ObservableCollection<ItemExplorerTab>();

        private EventHandle _eventHandle;

        /// <summary>
        /// Gets or sets the items to be shown. These items are shown also in the navigation view and will
        /// not be modified, even if the user clicks on the navigation tree. 
        /// </summary>
        protected IReflectiveCollection Items { get; set; }

        /// <summary>
        /// Defines the item that the user currently has selected on the object tree
        /// </summary>
        protected IObject SelectedPackage{ get; set; }

        /// <summary>
        /// Gets a value indicating whether the user has selected an extent within the
        /// treeview. 
        /// </summary>
        public bool IsExtentSelectedInTreeview { get; private set; }

        /// <summary>
        /// Gets or sets the items to be shown in the detail view. Usually, they are the same as the items.
        /// If the user clicks on the navigation tree, a subview of the items may be shown
        /// </summary>
        protected IReflectiveCollection SelectedItems { get; set; }

        /// <summary>
        /// Gets or sets the eventhandle for the content of the control
        /// </summary>
        public EventHandle EventHandle
        {
            get => _eventHandle;
            set
            {
                if (_eventHandle != null)
                {
                    App.Scope.Resolve<ChangeEventManager>().Unregister(_eventHandle);
                }

                _eventHandle = value;
            }
        }

        public ItemExplorerControl()
        {
            InitializeComponent();
            ItemTabControl.ItemsSource = Tabs;
        }

        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets the view extensions
        /// </summary>
        /// <returns>Gets the enumeration of the view extensions</returns>
        public virtual IEnumerable<ViewExtension> GetViewExtensions()
        {
            var selectedTab = ItemTabControl.SelectedItem as ItemExplorerTab;
            if (selectedTab?.ViewDefinition?.ViewExtensions == null)
            {
                yield break;
            }
            
            foreach (var extension in selectedTab.ViewDefinition.ViewExtensions)
            {
                yield return extension;
            }
        }

        public void SetItems(IReflectiveCollection items)
        {
            Items = items;
            UpdateTreeContent();
            RecreateViews();
        }

        /// <summary>
        /// Updates all views without recreating the items. 
        /// </summary>
        public virtual void UpdateAllViews()
        {
            UpdateTreeContent();
            foreach (var tab in Tabs)
            {
                tab.Control.UpdateContent();
            }
        }

        /// <summary>
        /// This method shall be called, when the content of the shown information has changed and all views shall be updated
        /// </summary>
        public void RecreateAllViews()
        { 
            UpdateTreeContent();
            RecreateViews();
        }

        /// <summary>
        /// Recreates all views
        /// </summary>
        protected void RecreateViews()
        {
            Tabs.Clear();
            OnRecreateViews();
        }

        /// <summary>
        /// This method will be called when the user has selected an item and the views need to be recreated
        /// </summary>
        protected virtual void OnRecreateViews()
        {
        }
        
        /// <summary>
        /// Updates the tree content of the explorer view
        /// </summary>
        protected void UpdateTreeContent()
        {
            NavigationTreeView.SetDefaultProperties();
            NavigationTreeView.ItemsSource = Items;
            SelectedPackage = null;
            IsExtentSelectedInTreeview = true;
            SelectedItems = Items;
        }

        public virtual void OnMouseDoubleClick(IObject element)
        {
            NavigateToElement(element);
        }

        /// <summary>
        /// Adds a new tab to the form
        /// </summary>
        /// <param name="collection">Collection being used</param>
        /// <param name="viewDefinition">Form to be added</param>
        public ItemExplorerTab AddTab(IReflectiveCollection collection, ViewDefinition viewDefinition)
        {
            // Gets the default view for the given tab
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            IElement result = null;

            switch (viewDefinition.Mode)
            {
                // Used, when an external function requires a specific view mode
                case ViewDefinitionMode.Specific:
                    result = viewDefinition.Element;
                    break;
                // Creates the view by creating the 'all Properties' view by parsing all the items
                case ViewDefinitionMode.AllProperties:
                    result = viewFinder.CreateView(Items);
                    break;
                case ViewDefinitionMode.Default:
                    break;
            }

            if (result == null)
            {
                // Nothing was found... so, create your default list list. 
                result = viewFinder.CreateView(collection);
            }

            // Creates the layoutcontrol for the given view
            var control = new ItemListViewControl
            {
                NavigationHost = NavigationHost
            };

            var tabControl = new ItemExplorerTab(viewDefinition)
            {
                Content = control,
                Header = viewDefinition.Name
            };

            control.SetContent(collection, result, viewDefinition.ViewExtensions);
            Tabs.Add(tabControl);

            // Selects the item, if none of the items are selected
            if (ItemTabControl.SelectedItem == null)
            {
                ItemTabControl.SelectedItem = tabControl;
                NavigationHost.RebuildNavigation();
            }

            return tabControl;
        }

        private void NavigationTreeView_OnItemChosen(object sender, ItemEventArgs e)
        {
            NavigateToElement(e.Item);
        }

        private void NavigationTreeView_OnItemSelected(object sender, ItemEventArgs e)
        {
            SelectedPackage = e.Item;
            if (e.Item != null)
            {
                SelectedItems = new PropertiesAsReflectiveCollection(e.Item);
                IsExtentSelectedInTreeview = false;
                RecreateViews();
            }
            else
            {
                // When user has selected the root element or no other item, all items are shown
                SelectedItems = Items;
                IsExtentSelectedInTreeview = true;
                RecreateViews();
            }
        }

        /// <summary>
        /// Opens the selected element
        /// </summary>
        /// <param name="selectedElement">Selected element</param>
        private void NavigateToElement(IObject selectedElement)
        {
            if (selectedElement == null)
            {
                return;
            }

            NavigatorForItems.NavigateToElementDetailView(NavigationHost, selectedElement as IElement);
        }

        private void ItemTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationTreeView.ViewExtensions.Clear();
            foreach (var extension in GetViewExtensions().OfType<TreeViewItemCommandDefinition>())
            {
                NavigationTreeView.ViewExtensions.Add(extension);
            }
        }

        public void Unregister()
        {
            if (_eventHandle != null)
            {
                App.Scope.Resolve<ChangeEventManager>().Unregister(_eventHandle);
                _eventHandle = null;
            }
        }
    }
}
