﻿using System;
using System.Collections.Generic;
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
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ListViewControl.xaml
    /// </summary>
    public partial class ListViewControl : UserControl
    {
        public ListViewControl()
        {
            InitializeComponent();
        }

        public IEnumerable<IObject> Items { get; set; }

        public IElement FormDefinition { get; set; }

        public IDatenMeisterScope Scope { get; set; }
        
        private readonly IDictionary<ExpandoObject, IObject> _itemMapping = new Dictionary<ExpandoObject, IObject>();

        /// <summary>
        /// Stores the definitions for the row items
        /// </summary>
        private readonly List<RowItemButtonDefinition> _rowItemButtonDefinitions = new List<RowItemButtonDefinition>();

        private bool SupportNewItems
        {
            get => DataGrid.CanUserAddRows;
            set => DataGrid.CanUserAddRows = value;
        }
        /// <summary>
        /// Updates the content by going through the fields and items
        /// </summary>
        public void SetContent(IDatenMeisterScope scope, IEnumerable<IObject> items, IElement formDefinition)
        {
            Items = items;
            FormDefinition = formDefinition;
            Scope = scope;

            UpdateContent();
        }

        /// <summary>
        /// Updates the content
        /// </summary>
        public void UpdateContent()
        {
            SupportNewItems = !DotNetHelper.AsBoolean(FormDefinition.get(_FormAndFields._Form.inhibitNewItems));

            var listItems = new List<ExpandoObject>();
            _itemMapping.Clear();

            if (!(FormDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            DataGrid.Columns.Clear();

            // Creates the column
            foreach (var field in fields.Cast<IElement>())
            {
                var name = field.get(_FormAndFields._FieldData.name).ToString();
                var title = field.get(_FormAndFields._FieldData.title);
                var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));
                var dataColumn = new DataGridTextColumn
                {
                    Header = title,
                    Binding = new Binding(name),
                    IsReadOnly = isEnumeration
                };

                DataGrid.Columns.Add(dataColumn);
            }

            // Creates the rowns
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    var itemObject = new ExpandoObject();
                    var asDictionary = (IDictionary<string, object>) itemObject;

                    foreach (var field in fields.Cast<IElement>())
                    {
                        var name = field.get(_FormAndFields._FieldData.name).ToString();
                        var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));
                        var value = item.isSet(name) ? item.get(name) : null;

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

                            asDictionary.Add(name, result.ToString());
                        }
                        else
                        {
                            asDictionary.Add(name, value);
                        }
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

            // Creates the row button
            foreach (var definition in _rowItemButtonDefinitions)
            {
                AddRowItemButton(definition, false);
            }
        }

        /// <summary>
        /// Adds the default buttons
        /// </summary>
        public void AddDefaultButtons()
        {
            AddGenericButton("View Extent", () =>
            {
                var dlg = new ItemXmlViewWindow();
                dlg.UpdateContent(Items);
                dlg.ShowDialog();
            });

            AddGenericButton("View Config", () =>
            {
                var dlg = new ItemXmlViewWindow
                {
                    SupportWriting = true
                };
                dlg.UpdateContent(FormDefinition);

                dlg.UpdateButtonPressed += (x, y) =>
                {
                    var temporaryExtent = InMemoryProvider.TemporaryExtent;
                    var factory = new MofFactory(temporaryExtent);
                    FormDefinition = dlg.GetCurrentContentAsMof(factory);
                    UpdateContent();
                };

                dlg.ShowDialog();
            });

            AddRowItemButton("Open", selectedElement =>
            {
                if (selectedElement == null)
                {
                    return;
                }

                Navigator.TheNavigator.NavigateToElementDetailView(
                    Window.GetWindow(this),
                    Scope,
                    selectedElement as IElement);
            });
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
        /// Adds a button for a row item
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="pressed">Called, if the is button pressed</param>
        public void AddRowItemButton(string name, Action<IObject> pressed)
        {
            var definition = new RowItemButtonDefinition
            {
                Name = name,
                Pressed = pressed
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
            
            DataGrid.Columns.Add(dataColumn);
        }

        /// <summary>
        /// Defines the definition for the row item button
        /// </summary>
        public class RowItemButtonDefinition
        {
            public string Name { get; set; }
            public Action<IObject> Pressed { get; set; }
        }


        /// <summary>
        /// Gets the currently selected object
        /// </summary>
        private IObject SelectedItem
        {
            get
            {
                var selectedItem = DataGrid.SelectedItem;
                return selectedItem == null ? null : _itemMapping[(ExpandoObject) selectedItem];
            }
        }

        /// <summary>
        /// Called, if the user clicks on the button
        /// </summary>
        /// <param name="sender">Sender being used</param>
        /// <param name="e">Event arguments</param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var result = GetObjectsFromEventRouting(e);
            result.column.OnClick(result.selectedItem);
        }

        /// <summary>
        /// Gets the object and column from the button being clicked by the user
        /// </summary>
        /// <param name="e">Event being called</param>
        /// <returns>The selected item of the button and the column</returns>
        private (IObject selectedItem, ClickedTemplateColumn column) GetObjectsFromEventRouting(RoutedEventArgs e)
        {
            var content = ((ContentPresenter) ((Button) e.Source).TemplatedParent).Content;

            if (!(content is ExpandoObject expandoObject))
            {
                return (null, null);
            }

            var foundItem = _itemMapping[expandoObject];

            var button = (Button) e.Source;
            var contentPresenter = (ContentPresenter) button.TemplatedParent;
            var column = (ClickedTemplateColumn) ((DataGridCell) contentPresenter.Parent).Column;
            return (foundItem, column);
        }


        private void RowButton_OnInitialized(object sender, RoutedEventArgs e)
        {
            var result = GetObjectsFromEventRouting(e);
            var button = (Button)e.Source;

            if (result.selectedItem == null)
            {
                button.Visibility = Visibility.Hidden;
            }
            else
            {
                button.Content = result.column.Header.ToString();
            }
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
