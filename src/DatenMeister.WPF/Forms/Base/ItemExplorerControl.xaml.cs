using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base
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
        public IReflectiveCollection Items { get; protected set; }

        /// <summary>
        /// Defines the item that the user currently has selected on the object tree
        /// </summary>
        public IObject SelectedPackage{ get; protected set; }

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
        /// Gets or sets the view extensions
        /// </summary>
        protected ICollection<ViewExtension> ViewExtensions { get; set; }

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
                    GiveMe.Scope.Resolve<ChangeEventManager>().Unregister(_eventHandle);
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
            if (ViewExtensions != null)
            {
                foreach (var viewExtension in ViewExtensions)
                {
                    yield return viewExtension;
                }
            }

            var selectedTab = ItemTabControl.SelectedItem as ItemExplorerTab;
            if (selectedTab?.ViewExtensions == null)
            {
                yield break;
            }

            foreach (var extension in selectedTab.ViewExtensions)
            {
                if (extension is RibbonButtonDefinition ribbonButtonDefinition)
                {
                    ribbonButtonDefinition.FixTopCategoryIfNotFixed("View");
                }

                yield return extension;
            }

            // Get the view extensions by the plugins
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            var data = new ViewExtensionTargetInformation
            {
                NavigationGuest = this
            };

            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(data))
                {
                    if (extension is RibbonButtonDefinition ribbonButtonDefinition)
                    {
                        ribbonButtonDefinition.FixTopCategoryIfNotFixed("Extent");
                    }

                    yield return extension;
                }
            }


            yield return
                new RibbonButtonDefinition(
                    "Refresh",
                    UpdateAllViews,
                    Icons.Refresh,
                    NavigationCategories.File + ".Views");
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
        /// Recreates all views
        /// </summary>
        protected void RecreateViews()
        {
            Tabs.Clear();
            OnRecreateViews();
        }

        /// <summary>
        /// This method will be called when the user has selected an item and the views need to be recreated.
        /// This method must be overridden by the subclasses
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
        /// Evaluates the extent form by passing through the tabs and creating the necessary views of each tab
        /// If the subform is constrained by a property or metaclass, the collection itself is filtered within the
        /// this call
        /// </summary>
        /// <param name="collection">Collection of the item which shall be created</param>
        /// <param name="extentForm">The extent form to be shown. The tabs of the extern form are passed</param>
        /// <param name="viewExtensions">The view extensions which are applied</param>
        public void EvaluateForm(
            IReflectiveCollection collection, 
            IElement extentForm,
            ICollection<ViewExtension> viewExtensions)
        {
            var tabs = extentForm.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            if (tabs == null)
            {
                // No tabs, nothing to do
                return;
            }

            foreach (var tab in tabs.OfType<IElement>())
            {
                AddTab(collection, tab, Array.Empty<ViewExtension>());
            }

            ViewExtensions = viewExtensions;

        }

        /// <summary>
        /// Adds a new tab to the form
        /// </summary>
        /// <param name="collection">Collection being used</param>
        /// <param name="form">Form to be used for the tabulator</param>
        /// <param name="viewExtensions">Stores the view extensions</param>
        public ItemExplorerTab AddTab(IReflectiveCollection collection, IElement form, ICollection<ViewExtension> viewExtensions)
        {
            // Gets the default view for the given tab
            var viewFinder = GiveMe.Scope.Resolve<ViewFinder>();
            IObject result = form;
            var name = form.getOrDefault<string>(_FormAndFields._Form.title) ??
                       form.getOrDefault<string>(_FormAndFields._Form.name);

            // Creates the layoutcontrol for the given view
            var control = new ItemListViewControl
            {
                NavigationHost = NavigationHost
            };


            var tabControl = new ItemExplorerTab(form)
            {
                Content = control,
                Header = name,
                ViewExtensions = viewExtensions.Union(control.GetViewExtensions())
            };

            control.SetContent(collection, result, viewExtensions);
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
            ResetNavigationTreeViewExtensions();
        }

        /// <summary>
        /// Resets the view extensions for the attached navigation view
        /// </summary>
        private void ResetNavigationTreeViewExtensions()
        {
            NavigationTreeView.ViewExtensions.Clear();
            foreach (var extension in GetViewExtensions().OfType<TreeViewItemCommandDefinition>())
            {
                NavigationTreeView.ViewExtensions.Add(extension);
            }

            if (NavigationTreeView.ShowAllChildren)
            {
                NavigationTreeView.ViewExtensions.Add(new
                    TreeViewItemCommandDefinition(
                        "Show only packages",
                        _ =>
                        {
                            NavigationTreeView.ShowAllChildren = false;
                            ResetNavigationTreeViewExtensions();
                        }));
                
            }
            else
            {
                NavigationTreeView.ViewExtensions.Add(new
                    TreeViewItemCommandDefinition(
                        "Show all children",
                        _ =>
                        {
                            NavigationTreeView.ShowAllChildren = true;
                            ResetNavigationTreeViewExtensions();
                        }));
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

        private void ItemExplorerControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unregister();
        }
    }
}
