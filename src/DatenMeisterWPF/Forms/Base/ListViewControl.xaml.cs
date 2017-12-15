using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ListViewControl.xaml
    /// </summary>
    public partial class ListViewControl : UserControl, INavigationGuest
    {
        public ListViewControl()
        {
            InitializeComponent();
        }
        
        public INavigationHost NavigationHost;

        public IReflectiveSequence Items { get; set; }

        /// <summary>
        /// Gets or sets the form definition that is actually used for the current view
        /// </summary>
        public IElement ActualFormDefinition { get; set; }

        /// <summary>
        /// Gets or sets the view Definition mode
        /// </summary>
        public ViewDefinition ViewDefinition { get; set; }
        
        private readonly IDictionary<ExpandoObject, IObject> _itemMapping = new Dictionary<ExpandoObject, IObject>();

        /// <summary>
        /// Stores the definitions for the row items
        /// </summary>
        private readonly List<RowItemButtonDefinition> _rowItemButtonDefinitions = new List<RowItemButtonDefinition>();

        /// <summary>
        /// Defines the text being used for search
        /// </summary>
        private string _searchText;

        /// <summary>
        /// Gets the currently selected object
        /// </summary>
        private IObject SelectedItem
        {
            get
            {
                var selectedItem = DataGrid.SelectedItem;
                return selectedItem == null ? null : _itemMapping[(ExpandoObject)selectedItem];
            }
        }

        /// <summary>
        /// Gets or sets the information whether new items can be added via the datagrid
        /// </summary>
        private bool SupportNewItems
        {
            get => DataGrid.CanUserAddRows;
            set => DataGrid.CanUserAddRows = value;
        }

        /// <summary>
        /// Updates the content by going through the fields and items
        /// </summary>
        public void SetContent(IReflectiveSequence items, IElement formDefinition)
        {
            Items = items;
            ViewDefinition = new ViewDefinition(
                null,
                formDefinition,
                formDefinition == null ? ViewDefinitionMode.Default : ViewDefinitionMode.Specific
            );
            
            UpdateViewList();
            UpdateContent();
        }

        /// <summary>
        /// This method gets called, when a new item is added or an existing item was modified. 
        /// Derivation of the class which have automatic creation of columns may include additional columns.
        /// It loads the current view definition
        /// </summary>
        private void RefreshViewDefinition()
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            if (ViewDefinition.Mode == ViewDefinitionMode.Default)
            {
                ActualFormDefinition = viewFinder.FindView((Items as IHasExtent)?.Extent as IUriExtent);
            }

            if (ViewDefinition.Mode == ViewDefinitionMode.AllProperties
                || ViewDefinition.Mode == ViewDefinitionMode.Default && ActualFormDefinition == null)
            {
                ActualFormDefinition = viewFinder.CreateView(Items);
            }

            if (ViewDefinition.Mode == ViewDefinitionMode.Specific)
            {
                ActualFormDefinition = ViewDefinition.Element;
            }
        }

        /// <summary>
        /// Defines the virtual method that is used to collect all possible views which can be selected by the user
        /// </summary>
        /// <returns>Found enumeration of view or null, if the class does not support the collection of views</returns>
        public virtual IEnumerable<IElement> GetFormsForView()
        {
            return null;
        }

        /// <summary>
        /// Gets the value of the element by the field
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <param name="field">Field being used for query</param>
        /// <returns>Returned element for the </returns>
        public object GetValueOfElement(IObject element, IElement field)
        {
            var fieldType = field.get(_FormAndFields._FieldData.fieldType).ToString();
            if (fieldType == MetaClassElementFieldData.FieldType)
            {
                var elementAsElement = element as IElement;
                var metaClass = elementAsElement?.getMetaClass();

                return metaClass == null ? string.Empty : NamedElementMethods.GetFullName(elementAsElement.getMetaClass());
            }

            var name = field.get(_FormAndFields._FieldData.name)?.ToString();
            return element.isSet(name) ? element.get(name) : null;
        }

        /// <summary>
        /// Updates the content of the list by recreating the columns
        /// </summary>
        public void UpdateContent()
        {
            RefreshViewDefinition();

            SupportNewItems = 
                !DotNetHelper.AsBoolean(ActualFormDefinition.getOrDefault(_FormAndFields._Form.inhibitNewItems));
            SupportNewItems = false; // TODO: Make new items working

            var listItems = new ObservableCollection<ExpandoObject>();
            _itemMapping.Clear();
            
            // Updates all columns and returns the fieldnames and fields
            var (fieldDataNames, fields) = UpdateColumnDefinitions();
            if (fieldDataNames == null)
            {
                return;
            }

            // Creates the rowns
            if (Items != null)
            {
                // Get the items and honor searching
                var items = Items.OfType<IObject>();
                if (!string.IsNullOrEmpty(_searchText))
                {
                    var columnNames = fields.OfType<IElement>()
                        .Select(x => x.get("name")?.ToString())
                        .Where(x => x != null);
                    items = Items.WhenOneOfThePropertyContains(columnNames, _searchText).OfType<IObject>();
                }

                // Go through the items and build up the list of elements
                foreach (var item in items)
                {
                    var itemObject = new ExpandoObject();
                    var asDictionary = (IDictionary<string, object>) itemObject;

                    var n = 0;
                    foreach (var field in fields.Cast<IElement>())
                    {
                        var columnName = fieldDataNames[n];
                        var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));
                        var value = GetValueOfElement(item, field);

                        if (isEnumeration)
                        {
                            var result = new StringBuilder();
                            var valueAsList = DotNetHelper.AsEnumeration(value);
                            if (valueAsList != null)
                            {
                                var nr = string.Empty;
                                foreach (var valueElement in valueAsList)
                                {
                                    result.Append(nr + UmlNameResolution.GetName(valueElement));
                                    nr = "\r\n";
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
                            if (!noMessageBox)
                            {
                                MessageBox.Show(exc.Message);
                            }

                            noMessageBox = true;
                            (itemObject as IDictionary<string, object>)[y.PropertyName] = item.get(y.PropertyName);
                            noMessageBox = false;
                        }
                    };
                }
            }

            DataGrid.ItemsSource = listItems;
        }

        /// <summary>
        /// This method gets called to update the views
        /// </summary>
        private void UpdateViewList()
        {
            // Update view
            var views = GetFormsForView()?.ToList();
            if (views != null)
            {
                ViewList.Visibility = Visibility.Visible;
                var list = new List<ViewDefinition>
                {
                    new ViewDefinition("Default", null, ViewDefinitionMode.Default),
                    new ViewDefinition("All Properties", null, ViewDefinitionMode.Default)
                };
                list.AddRange(views.Select(x => new ViewDefinition(NamedElementMethods.GetFullName(x), x)));
                ViewList.ItemsSource = list;

                switch (ViewDefinition.Mode)
                {
                    case ViewDefinitionMode.AllProperties:
                        ViewList.SelectedIndex = 1;
                        break;
                    case ViewDefinitionMode.Default:
                        ViewList.SelectedIndex = 0;
                        break;
                    default:
                        ViewList.SelectedIndex = 2 + views.IndexOf(ActualFormDefinition);
                        break;
                }
            }
            else
            {
                ViewList.Visibility = Visibility.Collapsed;
            }
        }

        private void ViewList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(ViewList.SelectedItem is ViewDefinition newForm))
            {
                return;
            }

            ViewDefinition = newForm;
            UpdateContent();
        }

        /// <summary>
        /// Updates the columns for the fields and returns the names and fields
        /// </summary>
        /// <returns>The tuple containing the names being used for the column and the fields being used.</returns>
        private (List<string> names, IReflectiveCollection fields) UpdateColumnDefinitions()
        {
            if (!(ActualFormDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return (null,null);
            }

            DataGrid.Columns.Clear();
            var fieldNames = new List<string>();

            // Creates the column
            foreach (var field in fields.Cast<IElement>())
            {
                var name = "_" + (field.get(_FormAndFields._FieldData.name)?.ToString() ?? string.Empty);
                var title = field.get(_FormAndFields._FieldData.title)?.ToString() ?? string.Empty;
                var fieldType = field.get(_FormAndFields._FieldData.fieldType).ToString();
                bool isReadOnly;

                if (fieldType == MetaClassElementFieldData.FieldType)
                {
                    title = "Metaclass";
                    name = "Metaclass";
                    isReadOnly = true;
                }
                else
                {
                    var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));
                    isReadOnly = isEnumeration;
                }

                var dataColumn = new DataGridTextColumn
                {
                    Header = title,
                    Binding = new Binding(name),
                    IsReadOnly = isReadOnly
                };

                DataGrid.Columns.Add(dataColumn);
                fieldNames.Add(name);
            }
            
            // Creates the row button
            foreach (var definition in _rowItemButtonDefinitions)
            {
                AddRowItemButton(definition, false);
            }

            return (fieldNames, fields);
        }

        /// <summary>
        /// Adds the default buttons
        /// </summary>
        public void AddDefaultButtons()
        {
            void Open(IObject selectedElement)
            {
                if (selectedElement == null)
                {
                    return;
                }

                var events = Navigator.TheNavigator.NavigateToElementDetailView(
                    NavigationHost, 
                    selectedElement as IElement);
                events.Closed += (sender, args) => UpdateContent();
            }

            AddRowItemButton("Open", Open, ButtonPosition.Before);
        }

        /// <summary>
        /// Adds a button to the view
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
            return button;
        }

        /// <summary>
        /// Adds a button to the view
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
                var selectedItem = SelectedItem;
                onPressed(selectedItem);
            };

            ButtonBar.Children.Add(button);
            return button;
        }

        /// <summary>
        /// Defines the possible position of the row button compared to the data
        /// </summary>
        public enum ButtonPosition
        {
            Before,
            After
        }

        /// <summary>
        /// Adds a button for a row item
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="pressed">Called, if the is button pressed</param>
        /// <param name="position">Position of the button to be used</param>
        public void AddRowItemButton(string name, Action<IObject> pressed, ButtonPosition position = ButtonPosition.After)
        {
            var definition = new RowItemButtonDefinition
            {
                Name = name,
                Pressed = pressed,
                Position = position
            };

            AddRowItemButton(definition);
        }

        /// <summary>
        /// Adds a button for a row item
        /// </summary>
        /// <param name="definition">Definition for the button</param>
        /// <param name="addToList">True, if the buttons shall be added to the list of items. 
        /// False, if the buttons are reiterated</param>
        private void AddRowItemButton(RowItemButtonDefinition definition, bool addToList = true)
        {
            if (addToList)
            {
                _rowItemButtonDefinitions.Add(definition);
            }

            var columnTemplate = FindResource("TemplateColumnButton") as DataTemplate;
            var dataColumn = new ClickedTemplateColumn
            {
                Header = definition.Name,
                CellTemplate = columnTemplate,
                OnClick = definition.Pressed
            };

            if (definition.Position == ButtonPosition.Before)
            {
                DataGrid.Columns.Insert(0, dataColumn);
            }
            else
            {
                DataGrid.Columns.Add(dataColumn);
            }
        }

        /// <summary>
        /// Called, if the user clicks on the button
        /// </summary>
        /// <param name="sender">Sender being used</param>
        /// <param name="e">Event arguments</param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var (selectedItem, column) = GetObjectsFromEventRouting(e);
            column.OnClick(selectedItem);
        }

        /// <summary>
        /// Gets the object and column from the button being clicked by the user
        /// </summary>
        /// <param name="e">Event being called</param>
        /// <returns>The selected item of the button and the column</returns>
        private (IObject selectedItem, ClickedTemplateColumn column) 
            GetObjectsFromEventRouting(RoutedEventArgs e)
        {
            var content = ((ContentPresenter) ((Button) e.Source).TemplatedParent).Content;

            if (!(content is ExpandoObject expandoObject))
            {
                return (null, null);
            }

            if (!_itemMapping.TryGetValue(expandoObject, out var foundItem))
            {
                return (null, null);
            }

            var button = (Button) e.Source;
            var contentPresenter = (ContentPresenter) button.TemplatedParent;
            var column = (ClickedTemplateColumn) ((DataGridCell) contentPresenter.Parent).Column;
            return (foundItem, column);
        }
        
        private void RowButton_OnInitialized(object sender, RoutedEventArgs e)
        {
            var (selectedItem, column) = GetObjectsFromEventRouting(e);
            var button = (Button)e.Source;

            if (selectedItem == null)
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
            UpdateContent();
        }

        /// <summary>
        /// Prepares the navigation and ribbons
        /// </summary>
        public void PrepareNavigation()
        {
            void ViewExtent()
            {
                var dlg = new ItemXmlViewWindow
                {
                    Owner = Window.GetWindow(this)
                };
                dlg.UpdateContent(Items);
                dlg.ShowDialog();
            }

            void ViewConfig()
            {
                var dlg = new ItemXmlViewWindow
                {
                    SupportWriting = true,
                    Owner = Window.GetWindow(this)
                };
                dlg.UpdateContent(ActualFormDefinition);

                dlg.UpdateButtonPressed += (x, y) =>
                {
                    var temporaryExtent = InMemoryProvider.TemporaryExtent;
                    var factory = new MofFactory(temporaryExtent);
                    ActualFormDefinition = dlg.GetCurrentContentAsMof(factory);
                    UpdateContent();
                };

                dlg.ShowDialog();
            }

            NavigationHost.AddNavigationButton(
                "Refresh",
                UpdateContent,
                Icons.Refresh,
                NavigationCategories.File + ".Views");

            NavigationHost.AddNavigationButton(
                "Extent as XMI", 
                ViewExtent, 
                null, 
                NavigationCategories.File + ".Views");

            NavigationHost.AddNavigationButton(
                "View-Configuration", 
                ViewConfig, 
                null, 
                NavigationCategories.File + ".Views");
        }

        /// <summary>
        /// Defines the definition for the row item button
        /// </summary>
        public class RowItemButtonDefinition
        {
            public string Name { get; set; }
            public Action<IObject> Pressed { get; set; }
            public ButtonPosition Position { get; set; }
        }

        /// <inheritdoc />
        /// <summary>
        /// The template being used to click
        /// </summary>
        private class ClickedTemplateColumn : DataGridTemplateColumn
        {
            /// <summary>
            /// Gets or sets the action being called when the user clicks on the button
            /// </summary>
            public Action<IObject> OnClick { get; set; }
        }
    }
}
