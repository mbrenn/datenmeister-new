using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using BurnSystems;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.FastViewFilter;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Commands;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements;
using DatenMeister.WPF.Navigation;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;
using Clipboard = System.Windows.Clipboard;
using ContextMenu = System.Windows.Controls.ContextMenu;
using DataGridCell = System.Windows.Controls.DataGridCell;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    ///     Interaktionslogik für ItemListViewControl.xaml
    /// </summary>
    public partial class ItemListViewControl : UserControl, IHasSelectedItems, INavigationGuest,
        ICollectionNavigationGuest
    {
        /// <summary>
        ///     Defines the possible position of the row button compared to the data
        /// </summary>
        public enum ButtonPosition
        {
            Before,
            After
        }

        private static readonly ILogger Logger = new ClassLogger(typeof(ItemListViewControl));

        /// <summary>
        ///     Stores the delaying dispatcher
        /// </summary>
        private readonly DelayedRefreshDispatcher _delayedDispatcher;

        /// <summary>
        ///     Defines the logic for the fastview filters
        /// </summary>
        private readonly FastViewFilterLogic _fastViewFilter;

        private readonly IDictionary<ExpandoObject, IObject> _itemMapping = new Dictionary<ExpandoObject, IObject>();

        /// <summary>
        ///     Defines the change event handle being used for current view
        /// </summary>
        private EventHandle? _changeEventHandle;

        /// <summary>
        ///     Defines the text being used for search
        /// </summary>
        private string _searchText = string.Empty;

        /// <summary>
        ///     Stores the view logic
        /// </summary>
        private readonly FormsPlugin _formsPlugin;

        private INavigationHost? _navigationHost;

        /// <summary>
        /// Stores a cached instance of the change event manager
        /// </summary>
        private ChangeEventManager? _changeEventManager;

        private IObject? _effectiveForm;

        public ItemListViewControl()
        {
            _delayedDispatcher = new DelayedRefreshDispatcher(Dispatcher, UpdateForm);
            _fastViewFilter = GiveMe.Scope.Resolve<FastViewFilterLogic>();
            _formsPlugin = GiveMe.Scope.Resolve<FormsPlugin>();
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the items to be shown. These items are shown also in the navigation view and will
        ///     not be modified, even if the user clicks on the navigation tree.
        /// </summary>
        public IReflectiveCollection? Items { get; set; }

        /// <summary>
        ///     Defines the current form definition of the window as provided by the
        ///     derived method 'RequestForm'.
        /// </summary>
        public IObject? EffectiveForm
        {
            get => _effectiveForm;
            set
            {
                _effectiveForm = value;
#if DEBUG
                if (value != null && !FormMethods.ValidateForm(value))
                    throw new InvalidOperationException("The form did not pass validation");
#endif
            }
        }

        /// <summary>
        ///     Gets or sets the view extensions
        /// </summary>
        public List<ViewExtension> ViewExtensions { get; private set; } = new List<ViewExtension>();

        /// <summary>
        ///     Gets or sets the information whether new items can be added via the datagrid
        /// </summary>
        private bool SupportNewItems
        {
            get => DataGrid.CanUserAddRows;
            set => DataGrid.CanUserAddRows = value;
        }

        /// <summary>
        ///     Gets an enumeration of all selected items
        /// </summary>
        /// <returns>Enumeration of selected item</returns>
        public IEnumerable<IObject> GetSelectedItems()
        {
            var selectedItems = DataGrid.SelectedItems;
            if (selectedItems.Count == 0)
                // If no item is selected, get no item
                yield break;

            lock (_itemMapping)
            {
                foreach (var item in selectedItems)
                    if (item is ExpandoObject selectedItem)
                        if (_itemMapping.TryGetValue(selectedItem, out var foundItem))
                            yield return foundItem;
            }
        }

        /// <summary>
        ///     Gets the currently selected item
        /// </summary>
        /// <returns>Item being selected</returns>
        public IObject? GetSelectedItem()
            => GetSelectedItems().FirstOrDefault();

        /// <summary>
        ///     Gets or sets the host for the list view item. The navigation
        ///     host is used to set the ribbons and other navigational things
        /// </summary>
        public INavigationHost NavigationHost
        {
            get => _navigationHost ?? throw new InvalidOperationException("NavigationHost == null");
            set => _navigationHost = value;
        }

        /// <summary>
        ///     Updates the content by going through the fields and items
        /// </summary>
        public void SetContent(
            IReflectiveCollection items,
            IObject formDefinition,
            IEnumerable<ViewExtension> viewExtensions)
        {
            UnregisterCurrentChangeEventHandle();

            RegisterChangeEventHandle(items);

            // If form defines constraints upon metaclass, then the filtering will occur here
            Items = items;
            EffectiveForm = formDefinition;
            ViewExtensions =
                viewExtensions.ToList(); // ViewExtensions are stored to be used later in UpdateColumnDefinitions
            UpdateForm();
        }

        /// <summary>
        /// Registers the event handler, so the ItemListViewControl gets informed when an update of the data occurred.
        /// </summary>
        /// <param name="items"></param>
        private void RegisterChangeEventHandle(IReflectiveCollection items)
        {
            if (items is IHasExtent asExtent)
            {
                var extent = asExtent.Extent;
                if (extent != null)
                {
                    _changeEventManager ??= GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>();
                    if (_changeEventHandle != null)
                    {
                        _changeEventManager.Unregister(_changeEventHandle);
                    }

                    _changeEventHandle = _changeEventManager.RegisterFor(
                        extent,
                        (innerExtent, element) => _delayedDispatcher.RequestRefresh());
                }
            }
        }

        /// <summary>
        ///     Unregisters the change event handle and sets the variable _changeEventHandle to null
        /// </summary>
        protected void UnregisterCurrentChangeEventHandle()
        {
            if (_changeEventHandle != null)
            {
                _changeEventManager ??= GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>();
                var tryScope = GiveMe.TryGetScope();

                if (tryScope != null)
                {
                    tryScope.ScopeStorage.Get<ChangeEventManager>().Unregister(_changeEventHandle);
                }

                _changeEventHandle = null;
            }
        }

        /// <summary>
        /// Gets the value of the element by the field
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <param name="field">Field being used for query</param>
        /// <returns>Returned element for the </returns>
        private object? GetValueOfElement(IObject element, IElement field)
        {
            if (_formsPlugin == null) throw new InvalidOperationException("_formlogic == null");

            var fieldMetaClass = field.getMetaClass();
            if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
            {
                var elementAsElement = element as IElement;
                var metaClass = elementAsElement?.getMetaClass();

                return metaClass == null
                    ? string.Empty
                    : NamedElementMethods.GetFullName(metaClass);
            }

            var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            if (fieldMetaClass?.@equals(_DatenMeister.TheOne.Forms.__EvalTextFieldData) == true)
            {
                var cellInformation = InMemoryObject.CreateEmpty();
                var defaultText = name != null ? element.getOrDefault<string>(name) : string.Empty;
                cellInformation.set("text", defaultText);

                var evalProperties = field.getOrDefault<string>(_DatenMeister._Forms._EvalTextFieldData.evalCellProperties);
                if (evalProperties != null)
                {
                    defaultText = TextTemplateEngine.Parse(
                        evalProperties,
                        new Dictionary<string, object>
                        {
                            ["i"] = element,
                            ["c"] = cellInformation
                        });
                }

                return cellInformation.isSet("text")
                    ? cellInformation.getOrDefault<string>("text")
                    : defaultText;
            }

            return element.isSet(name) ? element.get(name) : null;
        }

        /// <summary>
        ///     Updates the content of the list by recreating the columns and rows
        ///     of the items.
        ///     This method is called, when the used clicks on the left side or
        ///     an additional item was created/edited or removed.
        /// </summary>
        public void UpdateForm()
        {
            if (EffectiveForm == null) throw new InvalidOperationException("EffectiveForm == null");

            var watch = new StopWatchLogger(Logger, "UpdateView", LogLevel.Trace);

            var listItems = new ObservableCollection<ExpandoObject>();

            var selectedItem = GetSelectedItem();
            var selectedItemPosition = DataGrid.SelectedIndex; // Gets the item position
            ExpandoObject? selectedExpandoObject = null;

            SupportNewItems =
                !EffectiveForm.getOrDefault<bool>(_DatenMeister._Forms._ListForm.inhibitNewItems);

            lock (_itemMapping)
            {
                _itemMapping.Clear();
            }

            // Updates all columns and returns the fieldnames and fields
            var (fieldDataNames, fields) = UpdateColumnDefinitions();
            if (fields == null)
            {
                Logger.Warn("UpdateColumnDefinitions did not return any fields.");
                return;
            }

            watch.IntermediateLog("UpdateColumnDefinitions done");

            Task.Run(() =>
            {
                // Creates the rows
                if (Items != null)
                {
                    var items = GetFilteredAndSortedItems(Items, fields);

                    lock (_itemMapping)
                    {
                        // Go through the items and build up the list of elements
                        foreach (var item in items.OfType<IObject>())
                        {
                            var itemObject = new ExpandoObject();
                            var asDictionary = (IDictionary<string, object?>) itemObject;

                            var n = 0;
                            foreach (var field in fields.Cast<IElement>())
                            {
                                var columnName = fieldDataNames[n];
                                if (asDictionary.ContainsKey(columnName))
                                {
                                    Logger.Warn($"Column {columnName} skipped because it is given multiple times");
                                    continue;
                                }

                                var value = GetValueOfElement(item, field);
                                var isEnumeration =
                                    field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration);

                                if (isEnumeration || DotNetHelper.IsEnumeration(value?.GetType()))
                                {
                                    var result = new StringBuilder();
                                    var valueAsList = DotNetHelper.AsEnumeration(value);
                                    if (valueAsList != null)
                                    {
                                        var elementCount = 0;
                                        var nr = string.Empty;
                                        foreach (var valueElement in valueAsList)
                                        {
                                            result.Append(nr + NamedElementMethods.GetName(valueElement));
                                            nr = "\r\n";

                                            elementCount++;
                                            if (elementCount > 10)
                                            {
                                                result.Append("\r\n... (more)");
                                                break;
                                            }
                                        }
                                    }

                                    value = result.ToString();
                                }

                                asDictionary.Add(columnName, value);

                                n++;
                            }

                            // Ok, we got the item. Now we need to have a mapping from ExpandoObject to the real item
                            _itemMapping[itemObject] = item;
                            listItems.Add(itemObject);

                            if (item.Equals(selectedItem))
                            {
                                selectedExpandoObject = itemObject;
                            }

                            // Adds the notification for the property
                            var noMessageBox = false;
                            (itemObject as INotifyPropertyChanged).PropertyChanged += (x, y) =>
                            {
                                try
                                {
                                    var newPropertyValue =
                                        (itemObject as IDictionary<string, object>)[y.PropertyName];
                                    item.set(y.PropertyName, newPropertyValue);
                                }
                                catch (Exception exc)
                                {
                                    if (!noMessageBox) MessageBox.Show(exc.Message);

                                    // Sets flag, so no additional message box will be shown when the itemObject is updated, possibly, leading to a new exception.
                                    noMessageBox = true;
                                    (itemObject as IDictionary<string, object?>)[y.PropertyName] =
                                        item.get(y.PropertyName);
                                    noMessageBox = false;
                                }
                            };
                        }
                    }
                }
            }).ContinueWith((x) =>
            {
                watch.IntermediateLog("Before setting");

                Dispatcher?.Invoke(() =>
                {
                    DataGrid.ItemsSource = listItems;
                    if (selectedExpandoObject != null)
                    {
                        DataGrid.SelectedItem = selectedExpandoObject;
                    }
                    else if (selectedItemPosition != -1 && listItems.Count > 0)
                    {
                        DataGrid.SelectedIndex = selectedItemPosition < listItems.Count
                            ? selectedItemPosition
                            : selectedItemPosition - 1;
                    }

                });

                watch.Stop();
            });
        }

        private IReflectiveCollection GetFilteredAndSortedItems(IReflectiveCollection items, IReflectiveCollection fields)
        {
            if (_effectiveForm.getOrDefault<bool>(_DatenMeister._Forms._ListForm.includeDescendents))
            {
                items = items.GetAllDescendantsIncludingThemselves();
            }

            // Extent shall be shown
            items = FilterByMetaClass(items).WhenElementIsObject();

            // Get the items and honor searching
            if (!string.IsNullOrEmpty(_searchText))
            {
                var columnNames = fields.OfType<IElement>()
                    .Select(x => x.get("name")?.ToString())
                    .Where(x => x != null);
                items = items.WhenOneOfThePropertyContains(
                    columnNames!, 
                    _searchText,
                    StringComparison.CurrentCultureIgnoreCase);
            }

            // Goes through the fast filters and filters the items
            foreach (var fastFilter in GetFastFilters())
            {
                var converter = FastViewFilterConverter.Convert(fastFilter);
                if (converter == null)
                {
                    Logger.Warn("FastViewFilter is not known: " + fastFilter);
                    continue;
                }

                items = items.WhenFiltered(x => converter.IsFiltered(x));
            }

            // Checks, if we have a view node
            var viewNode = _effectiveForm.getOrDefault<IElement>(_DatenMeister._Forms._ListForm.viewNode);
            if (viewNode != null)
            {
                var dataviewHandler =
                    new DataViewEvaluation(GiveMe.Scope.WorkspaceLogic, GiveMe.Scope.ScopeStorage);
                dataviewHandler.AddDynamicSource("input", items);

                items = dataviewHandler.GetElementsForViewNode(viewNode);
            }

            // Now performs the sorting
            var sortingOrder =
                _effectiveForm.getOrDefault<IReflectiveCollection>(
                    _DatenMeister._Forms._ListForm.sortingOrder);
            if (sortingOrder != null)
            {
                var sortingColumnNames =
                    sortingOrder
                        .OfType<IElement>()
                        .Select(x =>
                            (x.getOrDefault<bool>(_DatenMeister._Forms._SortingOrder.isDescending)
                                ? "!"
                                : "") +
                            x.getOrDefault<string>(_DatenMeister._Forms._SortingOrder.name))
                        .Where(x => !string.IsNullOrEmpty(x) && x != "!");
                items = items.OrderElementsBy(sortingColumnNames);
            }

            return items;
        }

        /// <summary>
        /// Gets the collection and return the collection by the filtered metaclasses. If the metaclass
        /// is not defined, then null is returned
        /// </summary>
        /// <param name="collection">Collection to be filtered</param>
        /// <returns>The filtered metaclasses</returns>
        private IReflectiveCollection FilterByMetaClass(IReflectiveCollection collection)
        {
            if (EffectiveForm == null) throw new InvalidOperationException("EffectiveForm == null");
            
            var noItemsWithMetaClass =
                EffectiveForm.getOrDefault<bool>(_DatenMeister._Forms._ListForm.noItemsWithMetaClass);

            // If form  defines constraints upon metaclass, then the filtering will occur here
            var metaClass = EffectiveForm.getOrDefault<IElement?>(_DatenMeister._Forms._ListForm.metaClass);

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

        /// <summary>
        /// Forces a refresh of the view
        /// </summary>
        public void ForceRefresh()
        {
            _delayedDispatcher.ForceRefresh();
        }

        /// <summary>
        ///     Updates the columns for the fields and returns the names and fields
        /// </summary>
        /// <returns>
        ///     The tuple containing the names being used for the column
        ///     and the fields being used.
        /// </returns>
        private (IList<string> names, IReflectiveCollection? fields) UpdateColumnDefinitions()
        {
            var fields = EffectiveForm?.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field);
            if (fields == null)
                return (new string[] { }, null);

            ClearInfoLines();
            DataGrid.Columns.Clear();
            ButtonBar.Children.Clear();
            ButtonBar.Visibility = Visibility.Collapsed;

            var fieldNames = new List<string>();

            var isFormReadOnly = EffectiveForm?.getOrDefault<bool>(_DatenMeister._Forms._Form.isReadOnly) == true;

            // Creates the column
            foreach (var field in fields.Cast<IElement>())
            {
                var internalName = StringManipulation.RandomString(20, true);
                var name = "_" + (field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) ?? string.Empty);
                var title = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title) ?? string.Empty;
                var fieldMetaClass = field.getMetaClass();

                bool isReadOnly;

                if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
                {
                    title = "Metaclass";
                    name = "Metaclass";
                    isReadOnly = true;
                }
                else
                {
                    var isEnumeration = field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration);
                    isReadOnly = isEnumeration;
                }

                var dataColumn = new DataGridTextColumn
                {
                    Header = title,
                    Binding = new Binding(internalName),
                    IsReadOnly = isReadOnly || isFormReadOnly,
                    ElementStyle = (Style) TryFindResource("DataGridCellCentered")
                };

                DataGrid.Columns.Add(dataColumn);
                fieldNames.Add(internalName);
            }

            // Creates the row button
            var effectiveViewExtensions = new List<ViewExtension>(ViewExtensions);
            
            // Go through the form and create the creation button
            var defaultTypes =
                EffectiveForm.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);
            if (Items != null && EffectiveForm.getOrDefault<bool>(_DatenMeister._Forms._ListForm.inhibitNewItems) == false)
            {
                foreach (var defaultType in defaultTypes.OfType<IElement>().Distinct())
                {
                    var defaultTypeMetaClass =
                        defaultType.getOrDefault<IElement>(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass);
                    if (defaultTypeMetaClass != null)
                    {
                        effectiveViewExtensions.Add(
                            ViewExtensionHelper.GetCreateButtonForMetaClass(
                                NavigationHost,
                                defaultTypeMetaClass,
                                Items));
                    }
                }
            }
            
            var hashSet = new HashSet<object>();

            foreach (var definition in effectiveViewExtensions)
                switch (definition)
                {
                    case RowItemButtonDefinition rowButtonDefinition:
                        var columnTemplate = FindResource("TemplateColumnButton") as DataTemplate;
                        var dataColumn = new ClickedTemplateColumn
                        {
                            Header = rowButtonDefinition.Name,
                            CellTemplate = columnTemplate,
                            OnClick = rowButtonDefinition.OnPressed
                        };

                        if (rowButtonDefinition.Position == ButtonPosition.Before)
                            DataGrid.Columns.Insert(0, dataColumn);
                        else
                            DataGrid.Columns.Add(dataColumn);

                        break;
                    case GenericButtonDefinition genericButtonDefinition:
                        if (genericButtonDefinition.Tag != null)
                        {
                            // Skip the duplicates
                            if (hashSet.Contains(genericButtonDefinition.Tag))
                                continue;

                            hashSet.Add(genericButtonDefinition.Tag);
                        }
                        
                        AddGenericButton(genericButtonDefinition.Name, genericButtonDefinition.OnPressed);
                        break;
                    case ItemButtonDefinition itemButtonDefinition:
                        AddItemButton(itemButtonDefinition.Name, itemButtonDefinition.OnPressed);
                        break;
                    case InfoLineDefinition lineDefinition:
                        AddInfoLine(lineDefinition.InfolineFactory());
                        break;
                }

            return (fieldNames, fields);
        }

        /// <summary>
        ///     Adds a button to the view
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="onPressed">Called if the user clicks on the button</param>
        /// <returns>The created button</returns>
        public ViewButton AddGenericButton(string name, Action onPressed)
        {
            var button = new ViewButton
            {
                Content = name
            };

            button.Pressed += (x, y) => { onPressed(); };
            ButtonBar.Children.Add(button);
            ButtonBar.Visibility = Visibility.Visible;
            return button;
        }

        /// <summary>
        ///     Adds a button to the view
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onPressed"></param>
        /// <returns>The created button</returns>
        public ViewButton AddItemButton(string name, Action<IObject> onPressed)
        {
            var button = new ViewButton
            {
                Content = name
            };

            button.Pressed += (x, y) =>
            {
                var selectedItem = GetSelectedItem();
                if (selectedItem != null)
                {
                    onPressed(selectedItem);
                }
            };

            ButtonBar.Children.Add(button);
            ButtonBar.Visibility = Visibility.Visible;
            return button;
        }

        /// <summary>
        ///     Opens the selected element
        /// </summary>
        /// <param name="guest">The Guest element to be queried</param>
        /// <param name="selectedElement">Selected element</param>
        private void NavigateToElement(INavigationGuest guest, IObject selectedElement)
        {
            if (selectedElement == null) return;

            _ = NavigatorForItems.NavigateToElementDetailView(
                NavigationHost,
                new NavigateToItemConfig(selectedElement)
                {
                    ContainerCollection = Items
                });
        }

        /// <summary>
        ///     Called, if the user clicks on the button
        /// </summary>
        /// <param name="sender">Sender being used</param>
        /// <param name="e">Event arguments</param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var (selectedItem, column) = GetObjectsFromEventRouting(e);
            if (selectedItem != null)
                column?.OnClick?.Invoke(this, selectedItem);
        }

        /// <summary>
        ///     Gets the object and column from the button being clicked by the user
        /// </summary>
        /// <param name="e">Event being called</param>
        /// <returns>The selected item of the button and the column</returns>
        private (IObject? selectedItem, ClickedTemplateColumn? column)
            GetObjectsFromEventRouting(RoutedEventArgs e)
        {
            var content = ((ContentPresenter) ((Button) e.Source).TemplatedParent).Content;

            if (!(content is ExpandoObject expandoObject)) return (null, null);

            IObject? foundItem;
            lock (_itemMapping)
            {
                if (!_itemMapping.TryGetValue(expandoObject, out foundItem)) return (null, null);
            }

            var button = (Button) e.Source;
            var contentPresenter = (ContentPresenter) button.TemplatedParent;
            var column = (ClickedTemplateColumn) ((DataGridCell) contentPresenter.Parent).Column;
            return (foundItem, column);
        }

        /// <summary>
        ///     Called, when a row button is initialized.
        ///     The method is called, when a row is created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowButton_OnInitialized(object sender, RoutedEventArgs e)
        {
            var (selectedItem, column) = GetObjectsFromEventRouting(e);
            if (column == null) return;
            var button = (Button) e.Source;

            if (selectedItem == null)
                // It is a 'new item'-row which does not need a button
            {
                button.Visibility = Visibility.Hidden;
            }
            else
            {
                button.Content = column.Header.ToString();
            }
        }

        private void SearchField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _searchText = SearchField.Text;
            UpdateForm();
        }

        /// <summary>
        ///     Called, if the user performs a double click on the given item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(DataGrid.SelectedItem is ExpandoObject selectedItem)) return;

            lock (_itemMapping)
            {
                if (_itemMapping.TryGetValue(selectedItem, out _))
                {
                    // OnMouseDoubleClick(foundItem);
                }
            }
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyToClipboardCommand.Execute(this, CopyType.Default);
        }

        /// <summary>
        ///     Clears the infolines
        /// </summary>
        public void ClearInfoLines()
        {
            InfoLines.Children.Clear();
        }

        /// <summary>
        ///     Adds an infoline to the window
        /// </summary>
        /// <param name="element">Element to be added</param>
        public void AddInfoLine(UIElement element)
        {
            InfoLines.Children.Add(element);
        }

        /// <summary>
        /// Called, when the user has clicked on the button to add a new fast view filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FastViewFilter_OnClick(object sender, RoutedEventArgs e)
        {
            var translator = new FastViewFilterTranslator();
            var menu = new ContextMenu();
            var list = new List<object>();

            foreach (var filter in _fastViewFilter.FastViewFilters.OfType<IElement>())
            {
                var filterName = translator.TranslateType(filter);

                var item = new MenuItem
                {
                    Header = filterName
                };

                item.Click += async (x, y) =>
                {
                    var subItem = InMemoryObject.CreateEmpty(filter);

                    var events = await NavigatorForItems.NavigateToElementDetailView(
                        NavigationHost,
                        subItem,
                        d =>
                        {
                            d.ViewDefined += (a, b) =>
                            {
                                var effectiveForm = EffectiveForm ??
                                                    throw new InvalidOperationException("EffectiveForm == null");

                                // Remove the field with property
                                var fields = b.View.get<IReflectiveSequence>(_DatenMeister._Forms._ListForm.field);
                                var propertyField = QueryHelper.GetChildWithProperty(fields,
                                    _DatenMeister._Forms._FieldData.name,
                                    _DatenMeister._FastViewFilters._PropertyComparisonFilter.Property);
                                if (propertyField != null) fields.remove(propertyField);

                                // Now, create the replacement
                                var factory = new MofFactory(b.View);
                                var element = factory.create(_DatenMeister.TheOne.Forms.__DropDownFieldData);
                                element.set(_DatenMeister._Forms._DropDownFieldData.name,
                                    _DatenMeister._FastViewFilters._PropertyComparisonFilter.Property);
                                element.set(_DatenMeister._Forms._DropDownFieldData.title,
                                    _DatenMeister._FastViewFilters._PropertyComparisonFilter.Property);

                                var pairs = new List<IObject>();
                                foreach (var field in
                                    effectiveForm.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field)
                                        .OfType<IObject>())
                                {
                                    if (!field.isSet(_DatenMeister._Forms._FieldData.name)) continue;

                                    var pair = factory.create(_DatenMeister.TheOne.Forms.__ValuePair);

                                    pair.set(_DatenMeister._Forms._ValuePair.name,
                                        field.get<string>(_DatenMeister._Forms._FieldData.title));
                                    pair.set(_DatenMeister._Forms._ValuePair.value,
                                        field.get<string>(_DatenMeister._Forms._FieldData.name));
                                    pairs.Add(pair);
                                }

                                element.set(_DatenMeister._Forms._DropDownFieldData.values, pairs);
                                fields.add(0, element);
                            };
                        });

                    if (events != null &&
                        events.Result == NavigationResult.Saved) AddFastFilter(subItem);
                };

                list.Add(item);
            }

            menu.ItemsSource = list;

            FastViewFilter.ContextMenu = menu;
            FastViewFilter.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Adds a fast filter to the current view
        /// </summary>
        /// <param name="fastFilter">Fast filter to be stored</param>
        private void AddFastFilter(IObject fastFilter)
        {
            var effectiveForm = EffectiveForm 
                                ?? throw new InvalidOperationException("EffectiveForm == null");
            effectiveForm.AddCollectionItem(_DatenMeister._Forms._ListForm.fastViewFilters, fastFilter);
            UpdateFastFilterTexts();
            UpdateForm();
        }

        private void UpdateFastFilterTexts()
        {
            var effectiveForm = EffectiveForm 
                                ?? throw new InvalidOperationException("EffectiveForm == null");
            
            FastViewFilterPanel.Children.Clear();
            var fastFilters =
                effectiveForm.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.fastViewFilters);

            foreach (var filter in fastFilters.OfType<IElement>())
            {
                var translator = new FastViewFilterTranslator();

                var text = new TextBlock
                {
                    Text = translator.TranslateFilter(filter)
                };

                text.MouseDown += (x, y) =>
                {
                    fastFilters.remove(filter);
                    UpdateFastFilterTexts();
                    UpdateForm();
                };

                FastViewFilterPanel.Children.Add(text);
            }
        }

        /// <summary>
        /// Gets a the applied fast filters as an enumeration
        /// </summary>
        /// <returns>Enumeration of the fast filters</returns>
        private IEnumerable<IElement> GetFastFilters()
        {
            var effectiveForm = EffectiveForm
                                ?? throw new InvalidOperationException("EffectiveForm == null"); 
                                
            return effectiveForm.ForceAsEnumerable(_DatenMeister._Forms._ListForm.fastViewFilters).OfType<IElement>();
        }

        private void ItemListViewControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnregisterCurrentChangeEventHandle();
        }

        /// <summary>
        ///     Gets the context menu allowing the copying of the given text to the Clipboard
        /// </summary>
        /// <param name="text">Text to be stored</param>
        /// <returns>Resulting context menu</returns>
        public static ContextMenu GetCopyToClipboardContextMenu(string text)
        {
            var contextMenu = new ContextMenu();
            var item = new MenuItem
            {
                Header = "Copy to Clipboard"
            };
            item.Click += (x, y) => { Clipboard.SetText(text); };
            contextMenu.Items.Add(item);
            return contextMenu;
        }

        /// <inheritdoc />
        /// <summary>
        ///     The template being used to click
        /// </summary>
        private class ClickedTemplateColumn : DataGridTemplateColumn
        {
            /// <summary>
            ///     Gets or sets the action being called when the user clicks on the button
            /// </summary>
            public Action<INavigationGuest, IObject>? OnClick { get; set; }
        }

        IReflectiveCollection ICollectionNavigationGuest.Collection =>
            Items ?? throw new InvalidOperationException("Items == null");
    }
}