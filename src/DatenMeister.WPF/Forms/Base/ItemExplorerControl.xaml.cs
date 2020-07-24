#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Fields;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.ListViews;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Tags;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
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

        private EventHandle? _eventHandle;
        private IExtent? _extent;
        private INavigationHost? _navigationHost;
        private IObject? _rootItem;

        /// <summary>
        /// Stores the current view mode as being selected by the user
        /// </summary>
        public IElement? CurrentViewMode { get; private set; }

        /// <summary>
        /// Gets the id of the current view mode. 
        /// </summary>
        public string CurrentViewModeId => CurrentViewMode?.getOrDefault<string>(_FormAndFields._ViewMode.id) ?? "";

        public ItemExplorerControl()
        {
            InitializeComponent();
            ItemTabControl.ItemsSource = Tabs;
        }

        public INavigationHost NavigationHost
        {
            get => _navigationHost ?? throw new InvalidOperationException("NavigationHost == null");
            set => _navigationHost = value;
        }

        /// <summary>
        /// Gets the extent being connected
        /// </summary>
        public IExtent Extent
        {
            get => _extent ?? throw new InvalidOperationException("_extent == null");
            protected set => _extent = value;
        }

        public IReflectiveCollection Collection
        {
            get
            {
                if (RootItem is IExtent extent)
                {
                    return extent.elements();
                }

                return GetPropertiesAsReflection(RootItem, null);
            }
        }

        public IObject Item => SelectedItem ?? Extent;

        /// <summary>
        ///     Gets the definition of the current form
        /// </summary>
        public IObject? EffectiveForm { get; private set; }

        /// <summary>
        /// Gets or sets the root item for the explorer view. The properties of the root item are
        /// walked through to get the child items
        /// </summary>
        public IObject RootItem
        {
            get => _rootItem ?? throw new InvalidOperationException("RootItem == null");
            protected set => _rootItem = value;
        }

        /// <summary>
        /// Gets the currently selected item in the tree view. The RootItem will be selected in case no item is
        /// selected
        /// </summary>
        public IObject? SelectedItem { get; protected set; }

        /// <summary>
        ///     Gets a value indicating whether the user has selected an extent within the
        ///     treeview.
        /// </summary>
        public bool IsExtentSelectedInTreeview { get; private set; }

        /// <summary>
        /// Gets or sets the view extensions of the form currently being set
        /// </summary>
        protected ICollection<ViewExtension>? ViewExtensions { get; set; }


        /// <summary>
        /// Gets or sets the form that is overriding the default form
        /// </summary>
        public FormDefinition? OverridingViewDefinition { get; private set; }

        /// <summary>
        /// Sets the form that shall be shown instead of the default form as created by the inheriting items
        /// </summary>
        /// <param name="form"></param>
        public void SetOverridingForm(IElement form)
        {
            OverridingViewDefinition = new FormDefinition(form);
            RecreateForms();
        }

        /// <summary>
        /// Clears the overriding form, so the default views are used 
        /// </summary>
        public void ClearOverridingForm()
        {
            OverridingViewDefinition = null;
            RecreateForms();
        }

        /// <summary>
        /// Forces the generation of the form via the form creator
        /// </summary>
        public void ForceAutoGenerationOfForm()
        {
            OverridingViewDefinition = new FormDefinition(FormDefinitionMode.ViaFormCreator);
            RecreateForms();
        }
        
        /// <summary>
        ///     Gets or sets the eventhandle for the content of the control
        /// </summary>
        public EventHandle? EventHandle
        {
            get => _eventHandle;
            set
            {
                if (_eventHandle != null) GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>().Unregister(_eventHandle);

                _eventHandle = value;
            }
        }

        public void Unregister()
        {
            if (_eventHandle != null)
            {
                GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>().Unregister(_eventHandle);
                _eventHandle = null;
            }
        }

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
            var extentData = GetViewExtensionInfo();
                
            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(extentData))
                {
                    yield return extension;
                }
            }

            // Gets the default view extensions
            foreach (var defaultViewExtension in GetDefaultViewExtensions()) 
                yield return defaultViewExtension;
        }

        /// <summary>
        /// Gets the view extension information 
        /// </summary>
        /// <returns>The view extension fitting to item explorer</returns>
        public virtual ViewExtensionInfo GetViewExtensionInfo() =>
            new ViewExtensionInfoExplore(NavigationHost, this)
            {
                RootElement = _rootItem,
                SelectedElement = SelectedItem
            };

        /// <summary>
        /// Gets the default view extensions for any item explorer control
        /// </summary>
        /// <returns>enumeration of view extensions</returns>
        private IEnumerable<ViewExtension> GetDefaultViewExtensions()
        {
            // 4) Gets a refresh of window
            yield return
                new ExtentMenuButtonDefinition(
                    "Refresh",
                    x => RecreateForms(),
                    Icons.Refresh,
                    NavigationCategories.Form + ".View");
            
            // 5) Gets the context menu for the treeview
            if (NavigationTreeView?.ShowAllChildren == true)
            {
                yield return new
                    TreeViewItemCommandDefinition(
                        "Show only packages",
                        _ =>
                        {
                            NavigationTreeView.ShowAllChildren = false;
                            NavigationHost?.RebuildNavigation();
                        });
            }
            else
            {
                yield return new
                    TreeViewItemCommandDefinition(
                        "Show all children",
                        _ =>
                        {
                            NavigationTreeView!.ShowAllChildren = true;
                            NavigationHost?.RebuildNavigation();
                        });
            }
        }

        /// <summary>
        /// Sets the items to be shown in the reflective collection
        /// </summary>
        /// <param name="value"></param>
        public void SetRootItem(IObject value)
        {
            using var watch = new StopWatchLogger(_logger, "SetRootItem", LogLevel.Trace);

            RootItem = value;
            SetDefaultViewMode();
            
            UpdateTreeContent();
            RecreateForms();

            watch.Stop();
        }

        /// <summary>
        /// Gets the default view mode.
        /// The default view mode is automatically set by the extent type or by the last usage of the end user.
        /// The result is stored in the local variable 'CurrentViewMode'
        /// </summary>
        private void SetDefaultViewMode()
        {
            var extent = (RootItem as IHasExtent)?.Extent;
            
            // Checks, if the user has already selected something
            var uri = (extent as IUriExtent)?.contextURI();
            if (uri != null)
            {
                var found = GuiObjectCollection.TheOne.UserProperties.GetViewModeSelection(uri);
                if (found != null)
                {
                    CurrentViewMode = found;
                    return;
                }
            }

            // Checks the default by extent type
            var formMethods = GiveMe.Scope.Resolve<FormMethods>();
            var viewMode = formMethods.GetDefaultViewMode(extent);
            CurrentViewMode = viewMode;
        }

        /// <summary>
        ///     Updates all views without recreating the items.
        /// </summary>
        public virtual void UpdateForm()
        {
            UpdateTreeContent();
            foreach (var tab in Tabs) tab.ControlAsNavigationGuest.UpdateForm();
        }

        /// <summary>
        ///     Recreates all views
        /// </summary>
        protected void RecreateForms()
        {
            using var watch = new StopWatchLogger(_logger, "RecreateViews", LogLevel.Trace);
            
            Tabs.Clear();
            OnRecreateForms();
            NavigationHost?.RebuildNavigation();
        }

        /// <summary>
        ///     This method will be called when the user has selected an item and the forms need to be recreated.
        ///     This method must be overridden by the subclasses
        /// </summary>
        protected virtual void OnRecreateForms()
        {
        }

        /// <summary>
        ///     Updates the tree content of the explorer view
        /// </summary>
        protected void UpdateTreeContent()
        {
            NavigationTreeView.SetDefaultProperties();
            NavigationTreeView.ItemsSource = RootItem;
            IsExtentSelectedInTreeview = true;
            SelectedItem = RootItem;
        }
        
        /// <summary>
        ///     Evaluates the extent form by passing through the tabs and creating the necessary views of each tab
        ///     If the subform is constrained by a property or metaclass, the collection itself is filtered within the
        ///     this call
        /// </summary>
        /// <param name="value">Value which shall be shown. This method is </param>
        /// <param name="formDefinition">The extent form to be shown. The tabs of the extern form are passed</param>
        /// <param name="container">Container to which the element is contained by.
        /// This information is used to remove the item</param>
        protected void EvaluateForm(
            IObject value,
            FormDefinition formDefinition,
            IReflectiveCollection? container = null)
        {
            EffectiveForm = formDefinition.Element;

            var tabs = EffectiveForm?.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            if (tabs == null)
            {
                // No tabs, nothing to do
                return;
            }

            foreach (var tab in tabs.OfType<IElement>())
            {
                var tabViewExtensions = new List<ViewExtension>();
                
                // 1) Gets the form extensions as defined by the creator of the form definition
                if (formDefinition.TabViewExtensionsFunction != null)
                    tabViewExtensions = formDefinition.TabViewExtensionsFunction(tab).ToList();

                // 2) Forwards the form definitions
                if (formDefinition.ViewExtensions != null) 
                    tabViewExtensions.AddRange(formDefinition.ViewExtensions);
                
                // 3) Queries the plugins
                var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
                var extentData = new ViewExtensionInfoTab(NavigationHost, this)
                {
                    TabFormDefinition = tab
                };

                foreach (var plugin in viewExtensionPlugins)
                {
                    tabViewExtensions.AddRange(plugin.GetViewExtensions(extentData));
                }

                AddTab(value, tab, tabViewExtensions, container);
            }

            ViewExtensions = formDefinition.ViewExtensions;
            
            NavigationHost?.RebuildNavigation();
        }

        /// <summary>
        ///     Adds a new tab to the form
        /// </summary>
        /// <param name="value">Value to be shown in the explorer control view</param>
        /// <param name="tabForm">Form to be used for the tabulator</param>
        /// <param name="viewExtensions">Stores the view extensions</param>
        /// <param name="container">Container to which the element is contained by.
        /// This information is used to remove the item</param>
        private ItemExplorerTab? AddTab( 
            IObject value,
            IElement tabForm,
            IEnumerable<ViewExtension> viewExtensions,
            IReflectiveCollection? container = null)
        {
            // Gets the default view for the given tab
            var name = tabForm.getOrDefault<string>(_FormAndFields._Form.title) ??
                       tabForm.getOrDefault<string>(_FormAndFields._Form.name);
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Require<_FormAndFields>();
            var usedViewExtensions = viewExtensions.ToList();

            UserControl? createdUserControl = null;
            if (tabForm.getMetaClass()?.equals(formAndFields.__DetailForm) == true)
            {
                createdUserControl = CreateDetailForm(value, tabForm, container);
            }
            else if (tabForm.getMetaClass()?.equals(formAndFields.__ListForm) == true)
            {
                createdUserControl = CreateListControl(value, tabForm, usedViewExtensions);
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
            
            tabControl.EvaluateViewExtensions(usedViewExtensions);

            Tabs.Add(tabControl);

            // Selects the item, if none of the items are selected
            if (ItemTabControl.SelectedItem == null)
            {
                ItemTabControl.SelectedItem = tabControl;
            }

            return tabControl;
        }

        /// <summary>
        /// Creates the list control for one tab
        /// </summary>
        /// <param name="value">Element to be shown</param>
        /// <param name="tabForm">Form definition of the tab</param>
        /// <param name="usedViewExtensions">The view extensions for the tab</param>
        /// <returns></returns>
        private UserControl CreateListControl(IObject value, IObject tabForm, List<ViewExtension> usedViewExtensions)
        {
            // Creates the layoutcontrol for the given view
            var control = new ItemListViewControl
            {
                NavigationHost = NavigationHost,
                EffectiveForm = tabForm
            };

            usedViewExtensions.AddRange(control.GetViewExtensions());

            // Gets the default types by the form definition
            var defaultTypesForNewItems =
                tabForm.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.defaultTypesForNewElements)
                    ?.ToList()
                ?? new List<object?>();

            // Allows the deletion of an item
            if (tabForm.getOrDefault<bool>(_FormAndFields._ListForm.inhibitDeleteItems) != true)
            {
                usedViewExtensions.Add(
                    new RowItemButtonDefinition(
                        "Delete",
                        (guest, item) => { DeleteItem(item); }));
            }

            var createdUserControl = control;
            
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            
            // Sets the content for the tabs
            if (value is IExtent extent)
            {
                // Gets the default types by the View Extensions
                foreach (var extension in 
                    usedViewExtensions.OfType<NewInstanceViewExtension>())
                {
                    defaultTypesForNewItems.Add(extension.MetaClass);
                }

                // Filter MofObject Shadows out, they are not useable anyway.
                defaultTypesForNewItems = defaultTypesForNewItems.Where(x => !(x is MofObjectShadow)).ToList();
            
                // Goes through the view extensions indicating that an extent is shown
                var extentData = new ViewExtensionExtentInformation(NavigationHost, control, extent);

                foreach (var plugin in viewExtensionPlugins)
                {
                    usedViewExtensions.AddRange(plugin.GetViewExtensions(extentData));
                }

                // Creates the buttons for the new items
                var inhibitNewButtons = tabForm.getOrDefault<bool>(_FormAndFields._ListForm.inhibitNewItems);
                if (!inhibitNewButtons)
                {
                    // Creates the menu and buttons for the default types. 
                    CreateMenuAndButtonsForDefaultTypes(defaultTypesForNewItems, usedViewExtensions, null);
                }

                // Extent shall be shown
                IReflectiveCollection elements = extent.elements();
                elements = FilterByMetaClass(elements, tabForm);
                control.SetContent(elements, tabForm, usedViewExtensions);
            }
            else
            {
                // Query all the plugins whether a filter is available
                var propertyName = tabForm.getOrDefault<string>(nameof(ListForm.property));

                // Goes through the properties
                if (!string.IsNullOrEmpty(propertyName))
                {
                    var extentData =
                        new ViewExtensionItemPropertiesInformation(NavigationHost, control, value, propertyName);

                    foreach (var plugin in viewExtensionPlugins)
                    {
                        usedViewExtensions.AddRange(plugin.GetViewExtensions(extentData));
                    }
                }

                // Gets the default types by the View Extensions
                foreach (var extension in usedViewExtensions.OfType<NewInstanceViewExtension>())
                {
                    defaultTypesForNewItems.Add(extension.MetaClass);
                }

                // Adds the default type for the extension, if appropriate
                var metaClass = (value as IElement)?.getMetaClass();
                if (metaClass != null)
                {
                    var property = ClassifierMethods.GetPropertyOfClassifier(metaClass, propertyName);
                    if (property != null)
                    {
                        var elementProperty = PropertyMethods.GetPropertyType(property);
                        if (elementProperty != null)
                        {
                            defaultTypesForNewItems.Add(elementProperty);
                        }
                    }
                }
                
                var inhibitNewButtons = tabForm.getOrDefault<bool>(_FormAndFields._ListForm.inhibitNewItems);

                if (!inhibitNewButtons)
                {
                    //
                    // Creates the menu and buttons for the default types. 
                    CreateMenuAndButtonsForDefaultTypes(defaultTypesForNewItems, usedViewExtensions, propertyName);
                }

                // The properties of a specific item shall be shown
                var elements = GetPropertiesAsReflection(value, propertyName);
                elements = FilterByMetaClass(elements, tabForm);
                control.SetContent(elements, tabForm, usedViewExtensions);
            }

            return createdUserControl;
        }

        /// <summary>
        /// Deletes the item from the current extent
        /// </summary>
        /// <param name="item">Item to be deleted</param>
        private void DeleteItem(IObject item)
        {
            if (Extent != null)
            {
                var name = NamedElementMethods.GetName(item);
                if (MessageBox.Show(
                        $"Are you sure to delete the item '{name}'?",
                        "Confirmation",
                        MessageBoxButton.YesNo) ==
                    MessageBoxResult.Yes)
                {
                    // TODO: Will not work with selected item and its properties
                    // Only with the the items of the extent
                    Extent.elements().remove(item);
                }
            }
            else
            {
                MessageBox.Show("For whatever reason, deletion is not possible" +
                                "because the Extent is not given.");
            }
        }

        /// <summary>
        /// Creates the menu and the buttons for the default types
        /// </summary>
        /// <param name="defaultTypesForNewItems">List of types which shall be directly createable for
        ///     the user</param>
        /// <param name="usedViewExtensions">List of view extension which are applicable for
        ///     the regarded item</param>
        /// <param name="parentProperty">Defines the name of the parent property to which
        /// the item will be added. Null if item will be added to the given extent</param>
        /// <returns>List of menu items being used as context menu</returns>
        private void CreateMenuAndButtonsForDefaultTypes(
            IEnumerable<object?> defaultTypesForNewItems,
            List<ViewExtension> usedViewExtensions,
            string? parentProperty)
        {
            if (usedViewExtensions == null) throw new ArgumentNullException(nameof(usedViewExtensions));
            
            // Stores the menu items for the context menu
            var menuItems = new List<MenuItem>();
            var menuItem = new MenuItem
            {
                Header = "Select Type..."
            };

            menuItem.Click += async (x, y) => await CreateNewElementByUser(null, parentProperty);
            menuItems.Add(menuItem);

            // Sets the generic buttons to create the new types
            foreach (var type in defaultTypesForNewItems.OfType<IElement>())
            {
                // Check if type is a directly type or the DefaultTypeForNewElement
                if (type.metaclass?.equals(
                    GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Create<_FormAndFields>(
                        x => x.__DefaultTypeForNewElement)) == true)
                {
                    var newType =
                        type.getOrDefault<IElement>(_FormAndFields._DefaultTypeForNewElement.metaClass);
                    var tempParentProperty =
                        type.getOrDefault<string>(_FormAndFields._DefaultTypeForNewElement.parentProperty)
                        ?? parentProperty;

                    if (newType != null)
                    {
                        Create(newType, tempParentProperty);
                    }
                }
                else
                {
                    Create(type, parentProperty);
                }

                void Create(IElement newType, string? innerParentProperty)
                {
                    var typeName = newType.get(_UML._CommonStructure._NamedElement.name);

                    usedViewExtensions.Add(new GenericButtonDefinition(
                        $"New {typeName}", 
                        async () => await CreateNewElementByUser(newType, innerParentProperty))
                    {
                        Tag = new TagCreateMetaClass(newType)
                    });

                    foreach (var newSpecializationType in ClassifierMethods.GetSpecializations(newType))
                    {
                        // Stores the menu items for the context menu
                        menuItem = new MenuItem
                        {
                            Header = $"New {newSpecializationType}"
                        };

                        menuItem.Click += async (x, y) => await CreateNewElementByUser(newSpecializationType, null);
                        menuItems.Add(menuItem);
                    }
                }
            }

            async Task CreateNewElementByUser(IElement? type, string? innerParentProperty)
            {
                if (IsExtentSelectedInTreeview)
                    await NavigatorForItems.NavigateToNewItemForExtent(
                        NavigationHost,
                        Extent,
                        type);
                else
                {
                    if (innerParentProperty == null)
                        throw new InvalidOperationException("parentProperty == null");
                    if (SelectedItem == null)
                        throw new InvalidOperationException("SelectedPackage == null");

                    await NavigatorForItems.NavigateToNewItemForItem(
                        NavigationHost,
                        SelectedItem,
                        innerParentProperty, type);
                }
            }

            // Sets the button for the new item
            usedViewExtensions.Add(
                new GenericButtonDefinition(
                    "New Item...",
                    () => _ = new ContextMenu {ItemsSource = menuItems, IsOpen = true}));
        }

        /// <summary>
        /// Creates the detail form for the given value.
        /// </summary>
        /// <param name="value">Value which is shown in the detail form</param>
        /// <param name="tabForm">The form definition for </param>
        /// <param name="container"></param>
        /// <returns></returns>
        private UserControl CreateDetailForm(IObject value, IElement tabForm, IReflectiveCollection? container)
        {
            var control = new DetailFormControl
            {
                NavigationHost = NavigationHost
            };

            var formParameter = new FormParameter {IsReadOnly = true};

            control.SetContent(value, tabForm, container, formParameter);
            control.ElementSaved += (x, y) => MessageBox.Show("Element saved.");
            return control;
        }

        private IReflectiveCollection GetPropertiesAsReflection(IObject value, string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
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
            var metaClass = listFormDefinition.getOrDefault<IElement?>(_FormAndFields._ListForm.metaClass);

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
            var item = e.Item;
            if (item != null)
            {
                NavigateToElement(item);
            }
        }

        private void NavigationTreeView_OnItemSelected(object sender, ItemEventArgs e)
        {
            // Only, if the selected package is null (indicating the root) and
            // if the selected package is not the root package, then assume that a child is selected
            if (e.Item != null && e.Item?.equals(RootItem) != true)
            {
                SelectedItem = e.Item;
                IsExtentSelectedInTreeview = false;
                RecreateForms();
            }
            else
            {
                // When user has selected the root element or no other item, all items are shown
                SelectedItem = RootItem;
                IsExtentSelectedInTreeview = true;
                RecreateForms();
            }
        }

        /// <summary>
        ///     Opens the selected element
        /// </summary>
        /// <param name="selectedObject">Selected element</param>
        private void NavigateToElement(IObject selectedObject)
        {
            _ = NavigatorForItems.NavigateToElementDetailView(NavigationHost, selectedObject);
        }


        private void ItemTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationHost?.RebuildNavigation();
        }

        /// <summary>
        ///     Resets the view extensions for the attached navigation view
        /// </summary>
        public void EvaluateViewExtensions(ICollection<ViewExtension> viewExtensions)
        {
            NavigationTreeView.EvaluateViewExtensions(viewExtensions);
            
            // Creates the buttons for the treeview
            ClearTreeViewUiElement();
            AddTreeViewUiElement(viewExtensions);
        }

        private void ItemExplorerControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unregister();
        }

        private void AddTreeViewUiElement(IEnumerable<ViewExtension> viewExtensionInfo)
        {
            // Gets the elements of the plugin
            var data = new ViewExtensionInfo(NavigationHost, NavigationTreeView);
            
            foreach (var buttonView in viewExtensionInfo.OfType<ItemButtonDefinition>())
            {
                var button = new Button
                {
                    Content = buttonView.Name,
                    Margin = new Thickness(0,10,10,10)
                };

                button.Click += (x, y) =>
                {
                    var selectedElement = NavigationTreeView.GetSelectedItem()
                                          ?? RootItem;

                    buttonView.OnPressed(selectedElement);
                };

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
        
        public virtual void OnMouseDoubleClick(IObject element)
        {
            NavigateToElement(element);
        }

        private void btnViewMode_OnClick(object sender, RoutedEventArgs e)
        {
            var managementWorkspace = GiveMe.Scope.WorkspaceLogic.GetManagementWorkspace();
            var form = managementWorkspace.GetFromMetaWorkspace<_FormAndFields>() ??
                       throw new InvalidOperationException("FormAndFields not found");
            var viewModes = managementWorkspace.GetAllDescendentsOfType(form.__ViewMode, true);
            var contextMenu = new ContextMenu();

            var list = new List<MenuItem>();
            var selectedViewModeId = CurrentViewMode?.getOrDefault<string>(_FormAndFields._ViewMode.id);
            
            foreach (var mode in viewModes.OfType<IElement>())
            {
                var viewMode = mode;
                var item = new MenuItem
                {
                    Header = viewMode.getOrDefault<string>(_FormAndFields._ViewMode.id), 
                    Tag = viewMode
                };

                item.IsChecked = item.Header?.ToString() == selectedViewModeId;
                item.Click += (x, y) =>
                {
                    CurrentViewMode = viewMode;
                    var uri = ((RootItem as IHasExtent)?.Extent as IUriExtent)?.contextURI();

                    if (uri != null)
                    {
                        // Adds the chosen selection to the user properties
                        GuiObjectCollection.TheOne.UserProperties.AddViewModeSelection(uri, viewMode);
                    }

                    RecreateForms();
                };
                
                list.Add(item);
            }

            contextMenu.ItemsSource = list;
            contextMenu.IsOpen = true;
        }
    }
}