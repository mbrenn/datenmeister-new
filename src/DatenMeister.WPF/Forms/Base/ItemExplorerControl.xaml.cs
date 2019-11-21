using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Forms.Base.ViewExtensions.ListViews;
using DatenMeister.WPF.Forms.Base.ViewExtensions.TreeView;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    ///     Interaktionslogik für ItemBrowser.xaml
    /// </summary>
    public partial class ItemExplorerControl : UserControl,
        INavigationGuest, ICanUnregister, IExtentNavigationGuest, ICollectionNavigationGuest, IItemNavigationGuest
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private readonly ClassLogger _logger = new ClassLogger(typeof(ItemExplorerControl));

        /// <summary>
        ///     Stores the information about the active tab controls
        /// </summary>
        protected readonly ObservableCollection<ItemExplorerTab> Tabs = new ObservableCollection<ItemExplorerTab>();

        private EventHandle _eventHandle;

        public ItemExplorerControl()
        {
            InitializeComponent();
            ItemTabControl.ItemsSource = Tabs;
        }

        /// <summary>
        ///     Gets the definition of the current form
        /// </summary>
        public IObject EffectiveForm { get; private set; }

        /// <summary>
        /// Gets or sets the root item for the explorer view. The properties of the root item are
        /// walked through to get the child items
        /// </summary>
        public IObject RootItem { get; protected set; }

        public IObject SelectedItem { get; protected set; }

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
        /// Gets or sets the view extensions of the form currently being set
        /// </summary>
        protected ICollection<ViewExtension> ViewExtensions { get; set; }


        /// <summary>
        /// Gets or sets the form that is overriding the default form
        /// </summary>
        public ViewDefinition OverridingViewDefinition { get; private set; }

        /// <summary>
        /// Sets the form that shall be shown instead of the default form as created by the inheriting items
        /// </summary>
        /// <param name="form"></param>
        public void SetOverridingForm(IElement form)
        {
            OverridingViewDefinition = new ViewDefinition(form);
            RecreateViews();
        }

        /// <summary>
        /// Clears the overriding form, so the default views are used 
        /// </summary>
        public void ClearOverridingForm()
        {
            OverridingViewDefinition = null;
            RecreateViews();
        }

        /// <summary>
        /// Forces the generation of the form via the form creator
        /// </summary>
        public void ForceAutoGenerationOfForm()
        {
            OverridingViewDefinition = new ViewDefinition(ViewDefinitionMode.AllProperties);
            RecreateViews();
        }
        
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
            var selectedTabViewExtensions = selectedTab?.ControlAsNavigationGuest?.GetViewExtensions();

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
                new ExtentMenuButtonDefinition(
                    "Refresh",
                    x => RecreateViews(),
                    Icons.Refresh,
                    NavigationCategories.Form + ".View");


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
        /// <param name="value"></param>
        public void SetRootItem(IObject value)
        {
            var watch = new Stopwatch();
            watch.Start();

            RootItem = value;
            UpdateTreeContent();
            RecreateViews();

            watch.Stop();
            _logger.Info(watch.ElapsedMilliseconds.ToString("n0") + "ms required for form creation");
        }

        /// <summary>
        ///     Updates all views without recreating the items.
        /// </summary>
        public virtual void UpdateView()
        {
            UpdateTreeContent();
            foreach (var tab in Tabs) tab.ControlAsNavigationGuest.UpdateView();
        }

        /// <summary>
        ///     Recreates all views
        /// </summary>
        protected void RecreateViews()
        {
            Tabs.Clear();
            OnRecreateViews();
            NavigationHost.RebuildNavigation();

            RebuildNavigationForTreeView();
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
            NavigationTreeView.ItemsSource = RootItem;
            SelectedPackage = null;
            IsExtentSelectedInTreeview = true;
            SelectedItem = RootItem;
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
        /// <param name="value">Value which shall be shown</param>
        /// <param name="viewDefinition">The extent form to be shown. The tabs of the extern form are passed</param>
        /// <param name="container">Container to which the element is contained by.
        /// This information is used to remove the item</param>
        protected void EvaluateForm(
            IObject value,
            ViewDefinition viewDefinition,
            IReflectiveCollection container = null)
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
                
                tabViewExtensions.AddRange(GetViewExtensions());
                            

                AddTab(value, tab, tabViewExtensions, container);
            }

            ViewExtensions = viewDefinition.ViewExtensions;
            NavigationHost.RebuildNavigation();
        }

        /// <summary>
        ///     Adds a new tab to the form
        /// </summary>
        /// <param name="value">Value to be shown in the explorer control view</param>
        /// <param name="tabForm">Form to be used for the tabulator</param>
        /// <param name="viewExtensions">Stores the view extensions</param>
        /// <param name="container">Container to which the element is contained by.
        /// This information is used to remove the item</param>
        public ItemExplorerTab AddTab(
            IObject value,
            IElement tabForm,
            IEnumerable<ViewExtension> viewExtensions,
            IReflectiveCollection container = null)
        {
            // Gets the default view for the given tab
            var name = tabForm.getOrDefault<string>(_FormAndFields._Form.title) ??
                       tabForm.getOrDefault<string>(_FormAndFields._Form.name);
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();
            var usedViewExtensions = viewExtensions.ToList();

            UserControl createdUserControl = null;
            if (tabForm.getMetaClass().@equals(formAndFields.__DetailForm))
            {
                var control = new DetailFormControl
                {
                    NavigationHost = NavigationHost
                };

                control.SetContent(value, tabForm, container);
                control.ElementSaved += (x, y) => MessageBox.Show("Element saved.");
                createdUserControl = control;
            }
            else if (tabForm.getMetaClass().@equals(formAndFields.__ListForm))
            {
                // Creates the layoutcontrol for the given view
                var control = new ItemListViewControl
                {
                    NavigationHost = NavigationHost
                };

                usedViewExtensions.AddRange(control.GetViewExtensions());

                // Gets the default types by the form definition
                var defaultTypesForNewItems =
                    tabForm.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.defaultTypesForNewElements)
                        ?.ToList()
                    ?? new List<object>();

                // Gets the default types by the View Extensions
                foreach (var extension in usedViewExtensions.OfType<NewInstanceViewDefinition>())
                {
                    defaultTypesForNewItems.Add(extension.MetaClass);
                }
                
                // Stores the menu items for the context menu
                var menuItems = new List<MenuItem>();
                var menuItem = new MenuItem
                {
                    Header = "New Item"
                };
                
                menuItem.Click += (x, y) => CreateNewElementByUser(null, null);
                menuItems.Add(menuItem);

                // Sets the generic buttons to create the new types
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    // Check if type is a directly type or the DefaultTypeForNewElement
                    if (type.metaclass.@equals(
                        GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Create<_FormAndFields>(
                            x => x.__DefaultTypeForNewElement)))
                    {
                        var newType =
                            type.getOrDefault<IElement>(_FormAndFields._DefaultTypeForNewElement.metaClass);
                        var parentProperty =
                            type.getOrDefault<string>(_FormAndFields._DefaultTypeForNewElement.parentProperty);

                        Create(newType, parentProperty);
                    }
                    else
                    {
                        Create(type, null);
                    }

                    void Create(IElement newType, string parentProperty)
                    {
                        var typeName = newType.get(_UML._CommonStructure._NamedElement.name);

                        usedViewExtensions.Add(new GenericButtonDefinition(
                            $"New {typeName}", () => CreateNewElementByUser(newType, parentProperty)));

                        foreach (var newSpecializationType in ClassifierMethods.GetSpecializations(newType))
                        {
                            // Stores the menu items for the context menu
                            menuItem = new MenuItem
                            {
                                Header = $"New {newSpecializationType}"
                            };

                            menuItem.Click += (x, y) => CreateNewElementByUser(newSpecializationType, null);
                            menuItems.Add(menuItem);
                        }
                    }
                }

                // Sets the button for the new item
                usedViewExtensions.Add(
                    new GenericButtonDefinition(
                        "New Item",
                        () => _ = new ContextMenu {ItemsSource = menuItems, IsOpen = true}));

                // Allows the deletion of an item
                if (tabForm.getOrDefault<bool>(_FormAndFields._ListForm.inhibitDeleteItems) != true)
                {
                    usedViewExtensions.Add(
                        new RowItemButtonDefinition(
                            "Delete",
                            (guest, item) =>
                            {
                                if (MessageBox.Show(
                                        "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                                    MessageBoxResult.Yes)
                                {
                                    Extent.elements().remove(item);
                                }
                            }));
                }

                void CreateNewElementByUser(IElement type, string parentProperty)
                {
                    if (IsExtentSelectedInTreeview)
                        NavigatorForItems.NavigateToNewItemForExtent(
                            NavigationHost,
                            Extent,
                            type);
                    else
                        NavigatorForItems.NavigateToNewItemForItem(
                            NavigationHost,
                            SelectedPackage,
                            type,
                            parentProperty);
                }

                // Sets the content for the tabs
                if (value is IExtent extent)
                {
                    IReflectiveCollection elements = extent.elements();
                    elements = FilterByMetaClass(elements, tabForm);
                    control.SetContent(elements, tabForm, usedViewExtensions);
                }
                else
                {
                    IReflectiveCollection elements = GetPropertiesAsReflection(value, tabForm);
                    elements = FilterByMetaClass(elements, tabForm);
                    control.SetContent(elements, tabForm, usedViewExtensions);
                }

                createdUserControl = control;
            }

            if (createdUserControl == null)
            {
                _logger.Warn($"No user control was created: {value} {tabForm}");
                return null;
            }

            var tabControl = new ItemExplorerTab(tabForm)
            {
                Control = createdUserControl,
                Header = name
            };

            if (createdUserControl is INavigationGuest navigationGuest)
            {
                usedViewExtensions.AddRange(navigationGuest.GetViewExtensions());
                tabControl.EvaluateViewExtensions(usedViewExtensions);
            }

            Tabs.Add(tabControl);

            // Selects the item, if none of the items are selected
            if (ItemTabControl.SelectedItem == null)
            {
                ItemTabControl.SelectedItem = tabControl;
            }

            return tabControl;
        }

        private IReflectiveCollection GetPropertiesAsReflection(IObject value, IObject listFormDefinition)
        {
            var propertyName = listFormDefinition.getOrDefault<string>(nameof(ListForm.property));
            if (propertyName == null)
            {
                return new PropertiesAsReflectiveCollection(value);
            }

            return new PropertiesAsReflectiveCollection(value, propertyName);
        }

        /// <summary>
        /// Gets the collection and return the collection by the filtered metaclasses. If the metaclass
        /// is not defined, then null is returned
        /// </summary>
        /// <param name="collection">Collection to be filtered</param>
        /// <param name="listFormDefinition">The list form definition defining the meta class</param>
        /// <returns>The filtered metaclasses</returns>
        private IReflectiveCollection FilterByMetaClass(IReflectiveCollection collection, IObject listFormDefinition)
        {
            var noItemsWithMetaClass =
                listFormDefinition.getOrDefault<bool>(_FormAndFields._ListForm.noItemsWithMetaClass);

            // If form  defines constraints upon metaclass, then the filtering will occur here
            var metaClass = listFormDefinition.getOrDefault<IElement>(_FormAndFields._ListForm.metaClass);

            if (metaClass != null)
            {
                return collection.WhenMetaClassIs(metaClass);
            }

            if (noItemsWithMetaClass)
            {
                return collection.WhenMetaClassIs(null);
            }

            return collection;
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
                SelectedItem = e.Item;
                IsExtentSelectedInTreeview = false;
                RecreateViews();
            }
            else
            {
                // When user has selected the root element or no other item, all items are shown
                SelectedItem = RootItem;
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

        public IExtent Extent { get; protected set; }

        public IReflectiveCollection Collection => (RootItem as IExtent)?.elements();

        public IObject Item => SelectedPackage ?? Extent;
        
        
        /// <summary>
        /// Queries the plugins and asks for the navigation buttons for the treeview area 
        /// </summary>
        private void RebuildNavigationForTreeView()
        {
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            
            // Gets the elements of the plugin
            var data = new ViewExtensionTargetInformation(ViewExtensionContext.Application)
            {
                NavigationHost = NavigationHost,
                NavigationGuest = NavigationTreeView
            };
            
            // Creates the buttons for the treeview
            ClearTreeViewUiElement();
            
            foreach (var buttonView in viewExtensionPlugins.SelectMany(x => x.GetViewExtensions(data))
                .OfType<ItemButtonDefinition>())
            {
                var button = new Button
                {
                    Content = buttonView.Name
                };
                
                button.Click += (x, y) => buttonView.OnPressed(NavigationTreeView.SelectedElement);
                AddTreeViewUiElement(button);
            }
        }

        /// <summary>
        /// Adds a new element to the tree view element area
        /// </summary>
        /// <param name="element">Element to be added</param>
        public void AddTreeViewUiElement(UIElement element)
        {
            TreeViewButtonArea.Children.Add(element);
        }

        /// <summary>
        /// Clears the area
        /// </summary>
        public void ClearTreeViewUiElement()
        {
            TreeViewButtonArea.Children.Clear();
        }
    }
}