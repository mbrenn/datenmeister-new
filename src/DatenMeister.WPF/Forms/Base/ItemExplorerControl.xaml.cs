﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.WPF.Forms.Fields;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.ListViews;
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
        public string CurrentViewModeId => CurrentViewMode?.getOrDefault<string>(_DatenMeister._Forms._ViewMode.id) ?? "";

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

        public IReflectiveCollection Collection => 
            ListFormCollectionCreator.GetCollection(null, RootItem);

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
        /// Forces the generation of the form via the form reportCreator
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
            
            // Creates the tabs to be used for showing  
            CreateTabs(value, formDefinition, container);

            ViewExtensions = formDefinition.ViewExtensions;
            
            NavigationHost.RebuildNavigation();
        }

        /// <summary>
        /// Creates the tabs according to the rules of the tab creation
        /// </summary>
        /// <param name="value">Value to be currently shown (may also be an extent9</param>
        /// <param name="formDefinition">The form definition</param>
        /// <param name="container">The container element being used for deletion</param>
        /// <param name="tabs">Tabs to be shown</param>
        private void CreateTabs(
            IObject value, 
            FormDefinition formDefinition, 
            IReflectiveCollection? container)
        {
            // Creates the dynamic tabs depending on the content
            FormDynamicModifier.ModifyFormDependingOnObject(EffectiveForm, value);
            
            // Now gets the tabs
            var tabs = EffectiveForm?.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab);
            if (tabs == null)
            {
                // No tabs, nothing to do
                return;
            }
            
            // Creates the tabs themselves
            foreach (var tab in tabs.OfType<IElement>())
            {
                // Goes through the tabs

                // Creates the tab extensions
                var tabViewExtensions = new List<ViewExtension>();

                // 1) Gets the form extensions as defined by the reportCreator of the form definition
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
            var name = tabForm.getOrDefault<string>(_DatenMeister._Forms._Form.title) ??
                       tabForm.getOrDefault<string>(_DatenMeister._Forms._Form.name);
            var usedViewExtensions = viewExtensions.ToList();

            UserControl? createdUserControl = null;
            if (tabForm.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__RowForm) == true)
            {
                createdUserControl = CreateDetailForm(value, tabForm, container);
            }
            else if (tabForm.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__TableForm) == true)
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
        private UserControl CreateListControl(
            IObject value,
            IObject tabForm,
            List<ViewExtension> usedViewExtensions)
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
                tabForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.defaultTypesForNewElements)
                    ?.ToList()
                ?? new List<object?>();

            var createdUserControl = control;
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;

            var listCollection = ListFormCollectionCreator.GetCollection(tabForm, value);
            
            // Sets the content for the tabs
            if (value is IExtent extent)
            {
                // Gets the default types by the View Extensions
                foreach (var extension in 
                    usedViewExtensions.OfType<NewInstanceViewExtension>())
                {
                    var factory = new MofFactory(tabForm);
                    var newElement = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                    newElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, extension.MetaClass);
                    defaultTypesForNewItems.Add(newElement);
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
                var inhibitNewButtons = tabForm.getOrDefault<bool>(_DatenMeister._Forms._TableForm.inhibitNewItems);
                if (!inhibitNewButtons)
                {
                    // Creates the menu and buttons for the default types. 
                    CreateMenuAndButtonsForDefaultTypes(defaultTypesForNewItems, usedViewExtensions, null);
                }
                
                control.SetContent(listCollection, tabForm, usedViewExtensions);
            }
            else
            {
                // Query all the plugins whether a filter is available
                var propertyName = tabForm.getOrDefault<string>(_DatenMeister._Forms._TableForm.property);

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
                    var factory = new MofFactory(tabForm);
                    var newElement = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                    newElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, extension.MetaClass);
                    newElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.parentProperty, propertyName);
                    defaultTypesForNewItems.Add(newElement);
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
                
                var inhibitNewButtons = tabForm.getOrDefault<bool>(_DatenMeister._Forms._TableForm.inhibitNewItems);

                if (!inhibitNewButtons)
                {
                    //
                    // Creates the menu and buttons for the default types. 
                    CreateMenuAndButtonsForDefaultTypes(defaultTypesForNewItems, usedViewExtensions, propertyName);
                }
                
                control.SetContent(listCollection, tabForm, usedViewExtensions);
            }

            return createdUserControl;
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
                new FreeWpfElementDefinition
                {
                    ElementFactory = () =>
                    {
                        var result = new CreateNewInstanceButton();
                        result.SetDefaultTypesForCreation(defaultTypesForNewItems.OfType<IElement>());
                        result.TypeSelected += async (x, y) =>
                        {
                            await CreateNewElementByUser(y.SelectedType, parentProperty);
                        };
                        return result;
                    }
                });
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
            if (selectedObject is IExtent asExtent)
            {
                _ = NavigatorForExtents.OpenPropertiesOfExtent(NavigationHost, asExtent);
            }
            else
            {
                _ = NavigatorForItems.NavigateToElementDetailView(NavigationHost, selectedObject);
            }
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

        private void btnViewMode_OnClick(object sender, RoutedEventArgs e)
        {
            var managementWorkspace = GiveMe.Scope.WorkspaceLogic.GetManagementWorkspace();
            var viewModes = managementWorkspace.GetAllDescendentsOfType(_DatenMeister.TheOne.Forms.__ViewMode);
            var contextMenu = new ContextMenu();

            var list = new List<MenuItem>();
            var selectedViewModeId = CurrentViewMode?.getOrDefault<string>(_DatenMeister._Forms._ViewMode.id);
            
            foreach (var mode in viewModes.OfType<IElement>())
            {
                var viewMode = mode;
                var item = new MenuItem
                {
                    Header = viewMode.getOrDefault<string>(_DatenMeister._Forms._ViewMode.id), 
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