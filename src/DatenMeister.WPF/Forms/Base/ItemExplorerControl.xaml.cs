using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    ///     Interaktionslogik für ItemBrowser.xaml
    /// </summary>
    public partial class ItemExplorerControl : UserControl, 
        INavigationGuest, ICanUnregister, IExtentNavigationGuest, ICollectionNavigationGuest
    {
        /// <summary>
        ///     Stores the information about the active tab controls
        /// </summary>
        protected readonly ObservableCollection<ItemExplorerTab> Tabs = new ObservableCollection<ItemExplorerTab>();

        private EventHandle _eventHandle;

        protected IExtent _extent;

        public ItemExplorerControl()
        {
            InitializeComponent();
            ItemTabControl.ItemsSource = Tabs;
        }

        /// <summary>
        ///     Gets the definition of the current form
        /// </summary>
        public IElement EffectiveForm { get; private set; }

        /// <summary>
        ///     Gets or sets the items to be shown. These items are shown also in the navigation view and will
        ///     not be modified, even if the user clicks on the navigation tree.
        /// </summary>
        public IReflectiveCollection Items { get; protected set; }

        /// <summary>
        ///     Defines the item that the user currently has selected on the object tree
        /// </summary>
        public IObject SelectedPackage { get; protected set; }

        /// <summary>
        ///     Gets a value indicating whether the user has selected an extent within the
        ///     treeview.
        /// </summary>
        public bool IsExtentSelectedInTreeview { get; private set; }

        /// <summary>
        ///     Gets or sets the items to be shown in the detail view. Usually, they are the same as the items.
        ///     If the user clicks on the navigation tree, a subview of the items may be shown
        /// </summary>
        public IReflectiveCollection SelectedItems { get; set; }

        /// <summary>
        /// Gets or sets the view extensions of the form currently being set
        /// </summary>
        protected ICollection<ViewExtension> ViewExtensions { get; set; }

        /// <summary>
        ///     Gets or sets the eventhandle for the content of the control
        /// </summary>
        public EventHandle EventHandle
        {
            get => _eventHandle;
            set
            {
                if (_eventHandle != null) GiveMe.Scope.Resolve<ChangeEventManager>().Unregister(_eventHandle);

                _eventHandle = value;
            }
        }

        public void Unregister()
        {
            if (_eventHandle != null)
            {
                GiveMe.Scope.Resolve<ChangeEventManager>().Unregister(_eventHandle);
                _eventHandle = null;
            }
        }

        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        ///     Gets the view extensions
        /// </summary>
        /// <returns>Gets the enumeration of the view extensions</returns>
        public virtual IEnumerable<ViewExtension> GetViewExtensions()
        {
            // 1) Gets the view extensions of the ItemExplorerControl
            if (ViewExtensions != null)
                foreach (var viewExtension in ViewExtensions)
                    yield return viewExtension;

            // 2) Gets the view extensions of the selected tab of the detail form
            var selectedTab = ItemTabControl.SelectedItem as ItemExplorerTab;
            var selectedTabViewExtensions = selectedTab?.Control?.GetViewExtensions();

            if (selectedTabViewExtensions != null)
            {
                foreach (var extension in selectedTabViewExtensions)
                {
                    yield return extension;
                }
            }

            // 3) Get the view extensions by the plugins
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            var extentData = new ViewExtensionTargetInformation(ViewExtensionContext.Extent)
            {
                NavigationGuest = this,
                NavigationHost = NavigationHost
            };

            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(extentData))
                {
                    yield return extension;
                }
            }

            // 4) Gets a refresh of window
            yield return
                new ApplicationMenuButtonDefinition(
                    "Refresh",
                    UpdateAllViews,
                    Icons.Refresh,
                    NavigationCategories.File + ".Views");


            // 5) Gets the context menu for the treeview
            if (NavigationTreeView.ShowAllChildren)
            {
                yield return new
                    TreeViewItemCommandDefinition(
                        "Show only packages",
                        _ =>
                        {
                            NavigationTreeView.ShowAllChildren = false;
                            NavigationHost.RebuildNavigation();
                        });
            }
            else
            {
                yield return new
                    TreeViewItemCommandDefinition(
                        "Show all children",
                        _ =>
                        {
                            NavigationTreeView.ShowAllChildren = true;
                            NavigationHost.RebuildNavigation();
                        });

            }
        }

        /// <summary>
        /// Sets the items to be shown in the reflective collection
        /// </summary>
        /// <param name="items"></param>
        public void SetItems(IReflectiveCollection items)
        {
            Items = items;
            UpdateTreeContent();
            RecreateViews();
        }

        /// <summary>
        ///     Updates all views without recreating the items.
        /// </summary>
        public virtual void UpdateAllViews()
        {
            UpdateTreeContent();
            foreach (var tab in Tabs) tab.Control.UpdateContent();
        }

        /// <summary>
        ///     Recreates all views
        /// </summary>
        protected void RecreateViews()
        {
            Tabs.Clear();
            OnRecreateViews();
            NavigationHost.RebuildNavigation();
        }

        /// <summary>
        ///     This method will be called when the user has selected an item and the views need to be recreated.
        ///     This method must be overridden by the subclasses
        /// </summary>
        protected virtual void OnRecreateViews()
        {
        }

        /// <summary>
        ///     Updates the tree content of the explorer view
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
        ///     Evaluates the extent form by passing through the tabs and creating the necessary views of each tab
        ///     If the subform is constrained by a property or metaclass, the collection itself is filtered within the
        ///     this call
        /// </summary>
        /// <param name="collection">Collection of the item which shall be created</param>
        /// <param name="viewDefinition">The extent form to be shown. The tabs of the extern form are passed</param>
        public void EvaluateForm(
            IReflectiveCollection collection,
            ViewDefinition viewDefinition)
        {
            EffectiveForm = viewDefinition.Element;
            var tabs = viewDefinition.Element.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            if (tabs == null)
            {
                // No tabs, nothing to do
                return;
            }

            foreach (var tab in tabs.OfType<IElement>())
            {
                var tabViewExtensions = new List<ViewExtension>();
                if (viewDefinition.TabViewExtensionsFunction != null)
                    tabViewExtensions = viewDefinition.TabViewExtensionsFunction(tab).ToList();

                if (viewDefinition.ViewExtensions != null)
                    foreach (var viewExtension in viewDefinition.ViewExtensions)
                        tabViewExtensions.Add(viewExtension);

                AddTab(collection, tab, tabViewExtensions);
            }

            ViewExtensions = viewDefinition.ViewExtensions;
            NavigationHost.RebuildNavigation();
        }

        /// <summary>
        ///     Adds a new tab to the form
        /// </summary>
        /// <param name="collection">Collection being used</param>
        /// <param name="form">Form to be used for the tabulator</param>
        /// <param name="viewExtensions">Stores the view extensions</param>
        public ItemExplorerTab AddTab(
            IReflectiveCollection collection, 
            IElement form,
            ICollection<ViewExtension> viewExtensions)
        {
            // Gets the default view for the given tab
            var name = form.getOrDefault<string>(_FormAndFields._Form.title) ??
                       form.getOrDefault<string>(_FormAndFields._Form.name);

            // Creates the layoutcontrol for the given view
            var control = new ItemListViewControl
            {
                NavigationHost = NavigationHost
            };

            var tabControl = new ItemExplorerTab(form)
            {
                Control = control,
                Header = name
            };
            
            tabControl.EvaluateViewExtensions(viewExtensions.Union(control.GetViewExtensions()));
            control.SetContent(collection, form, viewExtensions);
            
            Tabs.Add(tabControl);

            // Selects the item, if none of the items are selected
            if (ItemTabControl.SelectedItem == null)
            {
                ItemTabControl.SelectedItem = tabControl;
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
        ///     Opens the selected element
        /// </summary>
        /// <param name="selectedElement">Selected element</param>
        private void NavigateToElement(IObject selectedElement)
        {
            if (selectedElement == null) return;

            NavigatorForItems.NavigateToElementDetailView(NavigationHost, selectedElement as IElement);
        }

        private void ItemTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationHost.RebuildNavigation();
        }

        /// <summary>
        ///     Resets the view extensions for the attached navigation view
        /// </summary>
        public void EvaluateViewExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            var newViewExtensions = new List<ViewExtension>();
            foreach (var extension in GetViewExtensions().OfType<TreeViewItemCommandDefinition>())
                newViewExtensions.Add(extension);
            
            NavigationTreeView.EvaluateViewExtensions(newViewExtensions);
        }

        private void ItemExplorerControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unregister();
        }

        public IExtent Extent => _extent;
        
        public IReflectiveCollection Collection => Items;
    }
}