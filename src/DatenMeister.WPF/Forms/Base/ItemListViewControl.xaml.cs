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
using System.Windows.Data;
using System.Windows.Input;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.FastViewFilter;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Commands;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using Microsoft.Win32;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    ///     Interaktionslogik für ItemListViewControl.xaml
    /// </summary>
    public partial class ItemListViewControl : UserControl, IHasSelectedItems, INavigationGuest, ICollectionNavigationGuest
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
        private readonly FormLogic _formLogic;

        private INavigationHost? _navigationHost;
        
        /// <summary>
        /// Stores a cached instance of the change event manager
        /// </summary>
        private ChangeEventManager? _changeEventManager;

        private IObject? _effectiveForm;

        public ItemListViewControl()
        {
            _delayedDispatcher = new DelayedRefreshDispatcher(Dispatcher, UpdateView);
            _fastViewFilter = GiveMe.Scope.Resolve<FastViewFilterLogic>();
            _formLogic = GiveMe.Scope.Resolve<FormLogic>();
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

            foreach (var item in selectedItems)
                if (item is ExpandoObject selectedItem)
                    if (_itemMapping.TryGetValue(selectedItem, out var foundItem))
                        yield return foundItem;
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
        ///     Prepares the navigation of the host. The function is called by the navigation
        ///     host.
        /// </summary>
        public IEnumerable<ViewExtension> GetViewExtensions()
        {
            void ViewCollection(IReflectiveCollection reflectiveCollection)
            {
                var dlg = new ItemXmlViewWindow
                {
                    Owner = Window.GetWindow(this)
                };

                dlg.UpdateContent(reflectiveCollection);
                dlg.ShowDialog();
            }

            void ExportToCSV(IReflectiveCollection items)
            {
                try
                {
                    if (Items == null) throw new InvalidOperationException("Items == null");
                    
                    var dlg = new SaveFileDialog
                    {
                        DefaultExt = "csv",
                        Filter = "CSV-Files|*.csv|All Files|*.*"
                    };

                    if (dlg.ShowDialog(Window.GetWindow(this)) == true)
                    {
                        var loader = new CsvLoader(GiveMe.Scope.Resolve<IWorkspaceLogic>());
                        var memoryProvider = new InMemoryProvider();
                        var temporary = new MofUriExtent(memoryProvider, "datenmeister:///temp");
                        var copier = new ExtentCopier(new MofFactory(temporary));
                        copier.Copy(Items, temporary.elements());

                        loader.Save(
                            memoryProvider,
                            dlg.FileName,
                            new CsvSettings());

                        MessageBox.Show($"CSV Export completed. \r\n{temporary.elements().Count()} Items exported.");
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Export failed\r\n{exc}");
                }
            }

            void CopyContent(IReflectiveCollection items)
            {
                CopyToClipboardCommand.Execute(this, CopyType.Default);
            }

            void CopyContentAsXmi(IReflectiveCollection items)
            {
                CopyToClipboardCommand.Execute(this, CopyType.AsXmi);
            }

            yield return
                new RowItemButtonDefinition(
                    "Edit",
                    NavigateToElement,
                    ButtonPosition.Before);

            yield return
                new CollectionMenuButtonDefinition(
                    "View as Xmi",
                    ViewCollection,
                    null,
                    "Collection");

            yield return
                new CollectionMenuButtonDefinition(
                    "Export CSV",
                    ExportToCSV,
                    Icons.ExportCSV,
                    "Collection");

            yield return
                new CollectionMenuButtonDefinition(
                    "Copy",
                    CopyContent,
                    null,
                    "Selection");

            yield return
                new CollectionMenuButtonDefinition(
                    "Copy as XMI",
                    CopyContentAsXmi,
                    null,
                    "Selection");

            // 2) Get the view extensions by the plugins
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            var extentData = new ViewExtensionInfoCollection(NavigationHost, this)
            {
                Collection = Items
            };

            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(extentData))
                {
                    yield return extension;
                }
            }
        }

        public void EvaluateViewExtensions(ICollection<ViewExtension> viewExtensions)
        {
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

            if (items is IHasExtent asExtent)
            {
                var extent = asExtent.Extent;
                if (extent != null)
                {
                    _changeEventManager ??= GiveMe.Scope.Resolve<ChangeEventManager>();
                    if (_changeEventHandle != null)
                    {
                        _changeEventManager.Unregister(_changeEventHandle);
                    }
                    
                    _changeEventHandle = _changeEventManager.RegisterFor(
                        extent,
                        (innerExtent, element) => _delayedDispatcher.RequestRefresh());
                }
            }

            // If form defines constraints upon metaclass, then the filtering will occur here
            Items = items;

            EffectiveForm = formDefinition;
            ViewExtensions = viewExtensions.ToList(); // ViewExtensions are stored to be used later in UpdateColumnDefinitions
            UpdateView();
        }

        /// <summary>
        ///     Unregisters the change event handle and sets the variable _changeEventHandle to null
        /// </summary>
        protected void UnregisterCurrentChangeEventHandle()
        {
            if (_changeEventHandle != null)
            {
                _changeEventManager ??= GiveMe.Scope.Resolve<ChangeEventManager>();
                GiveMe.Scope.Resolve<ChangeEventManager>().Unregister(_changeEventHandle);
                _changeEventHandle = null;
            }
        }

        /// <summary>
        ///     Gets the value of the element by the field
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <param name="field">Field being used for query</param>
        /// <returns>Returned element for the </returns>
        private object? GetValueOfElement(IObject element, IElement field)
        {
            if (_formLogic == null) throw new InvalidOperationException("_formlogic == null");
            
            var fieldMetaClass = field.getMetaClass();
            if (fieldMetaClass?.equals(_formLogic.GetFormAndFieldInstance().__MetaClassElementFieldData) == true)
            {
                var elementAsElement = element as IElement;
                var metaClass = elementAsElement?.getMetaClass();

                return metaClass == null
                    ? string.Empty
                    : NamedElementMethods.GetFullName(metaClass);
            }

            var name = field.getOrDefault<string>(_FormAndFields._FieldData.name);
            return element.isSet(name) ? element.get(name) : null;
        }

        /// <summary>
        ///     Updates the content of the list by recreating the columns and rows
        ///     of the items.
        ///     This method is called, when the used clicks on the left side or
        ///     an additional item was created/edited or removed.
        /// </summary>
        public void UpdateView()
        {
            if (EffectiveForm == null) throw new InvalidOperationException("EffectiveForm == null");
            
            var watch = new StopWatchLogger(Logger, "UpdateView");
            var listItems = new ObservableCollection<ExpandoObject>();

            var selectedItem = GetSelectedItem();
            var selectedItemPosition = DataGrid.SelectedIndex; // Gets the item position
            ExpandoObject? selectedExpandoObject = null;
            
            SupportNewItems =
                !EffectiveForm.getOrDefault<bool>(_FormAndFields._ListForm.inhibitNewItems);
            SupportNewItems = false; // TODO: Make new items working
            
            _itemMapping.Clear();

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
                if (fieldDataNames == null) return;

                // Creates the rows
                if (Items != null)
                {
                    // Get the items and honor searching
                    var items = Items.OfType<IObject>();
                    if (!string.IsNullOrEmpty(_searchText))
                    {
                        var columnNames = fields.OfType<IElement>()
                            .Select(x => x.get("name")?.ToString())
                            .Where(x => x != null);
                        items = Items.WhenOneOfThePropertyContains(columnNames!, _searchText).OfType<IObject>();
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

                        items = items.Where(x => converter.IsFiltered(x));
                    }

                    // Go through the items and build up the list of elements
                    foreach (var item in items)
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
                            
                            var isEnumeration = field.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration);
                            var value = GetValueOfElement(item, field);

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

                                asDictionary.Add(columnName, result.ToString());
                            }
                            else
                            {
                                asDictionary.Add(columnName, value);
                            }

                            n++;
                        }

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
                                var newPropertyValue = (itemObject as IDictionary<string, object>)[y.PropertyName];
                                item.set(y.PropertyName, newPropertyValue);
                            }
                            catch (Exception exc)
                            {
                                if (!noMessageBox) MessageBox.Show(exc.Message);

                                // Sets flag, so no additional message box will be shown when the itemObject is updated, possibly, leading to a new exception.
                                noMessageBox = true;
                                (itemObject as IDictionary<string, object?>)[y.PropertyName] = item.get(y.PropertyName);
                                noMessageBox = false;
                            }
                        };
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
            var fields = EffectiveForm?.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.field);
            if (fields == null)
                return (new string[] { }, null);

            ClearInfoLines();
            DataGrid.Columns.Clear();
            ButtonBar.Children.Clear();
            ButtonBar.Visibility = Visibility.Collapsed;

            var fieldNames = new List<string>();

            // Creates the column
            foreach (var field in fields.Cast<IElement>())
            {
                var name = "_" + (field.getOrDefault<string>(_FormAndFields._FieldData.name) ?? string.Empty);
                var title = field.getOrDefault<string>(_FormAndFields._FieldData.title) ?? string.Empty;
                var fieldMetaClass = field.getMetaClass();

                bool isReadOnly;

                if (fieldMetaClass?.equals(_formLogic.GetFormAndFieldInstance().__MetaClassElementFieldData) == true)
                {
                    title = "Metaclass";
                    name = "Metaclass";
                    isReadOnly = true;
                }
                else
                {
                    var isEnumeration = field.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration);
                    isReadOnly = isEnumeration;
                }

                var dataColumn = new DataGridTextColumn
                {
                    Header = title,
                    Binding = new Binding(name),
                    IsReadOnly = isReadOnly,
                    ElementStyle = (Style) TryFindResource("DataGridCellCentered")
                };

                DataGrid.Columns.Add(dataColumn);
                fieldNames.Add(name);
            }

            // Creates the row button
            foreach (var definition in ViewExtensions)
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

            if (!_itemMapping.TryGetValue(expandoObject, out var foundItem)) return (null, null);

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
            UpdateView();
        }

        /// <summary>
        ///     Called, if the user performs a double click on the given item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(DataGrid.SelectedItem is ExpandoObject selectedItem)) return;

            if (_itemMapping.TryGetValue(selectedItem, out var foundItem))
            {
                // OnMouseDoubleClick(foundItem);
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
            var translator = new FastViewFilterTranslator(GiveMe.Scope.Resolve<IWorkspaceLogic>());
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
                                var fields = b.View.get<IReflectiveSequence>(_FormAndFields._ListForm.field);
                                var propertyField = QueryHelper.GetChildWithProperty(fields,
                                    _FormAndFields._FieldData.name,
                                    nameof(PropertyComparisonFilter.Property));
                                if (propertyField != null) fields.remove(propertyField);

                                // Now, create the replacement
                                var formAndFields =
                                    GiveMe.Scope.Resolve<IWorkspaceLogic>().GetTypesWorkspace().Require<_FormAndFields>();
                                var factory = new MofFactory(b.View);
                                var element = factory.create(formAndFields.__DropDownFieldData);
                                element.set(_FormAndFields._DropDownFieldData.name,
                                    nameof(PropertyComparisonFilter.Property));
                                element.set(_FormAndFields._DropDownFieldData.title,
                                    nameof(PropertyComparisonFilter.Property));

                                var pairs = new List<IObject>();
                                foreach (var field in
                                    effectiveForm.get<IReflectiveCollection>(_FormAndFields._ListForm.field)
                                        .OfType<IObject>())
                                {
                                    if (!field.isSet(_FormAndFields._FieldData.name)) continue;

                                    var pair = factory.create(formAndFields.__ValuePair);

                                    pair.set(_FormAndFields._ValuePair.name,
                                        field.get<string>(_FormAndFields._FieldData.title));
                                    pair.set(_FormAndFields._ValuePair.value,
                                        field.get<string>(_FormAndFields._FieldData.name));
                                    pairs.Add(pair);
                                }

                                element.set(_FormAndFields._DropDownFieldData.values, pairs);
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
            effectiveForm.AddCollectionItem(_FormAndFields._ListForm.fastViewFilters, fastFilter);
            UpdateFastFilterTexts();
            UpdateView();
        }

        private void UpdateFastFilterTexts()
        {
            var effectiveForm = EffectiveForm 
                                ?? throw new InvalidOperationException("EffectiveForm == null");
            
            FastViewFilterPanel.Children.Clear();
            var fastFilters =
                effectiveForm.get<IReflectiveCollection>(_FormAndFields._ListForm.fastViewFilters);

            foreach (var filter in fastFilters.OfType<IElement>())
            {
                var translator = new FastViewFilterTranslator(
                    GiveMe.Scope.Resolve<IWorkspaceLogic>());

                var text = new TextBlock
                {
                    Text = translator.TranslateFilter(filter)
                };

                text.MouseDown += (x, y) =>
                {
                    fastFilters.remove(filter);
                    UpdateFastFilterTexts();
                    UpdateView();
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
                                
            return effectiveForm.ForceAsEnumerable(_FormAndFields._ListForm.fastViewFilters).OfType<IElement>();
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