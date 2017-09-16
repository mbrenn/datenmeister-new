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

        public bool SupportNewItems
        {
            get => DataGrid.CanUserAddRows;
            set => DataGrid.CanUserAddRows = value;
        }
        /// <summary>
        /// Updates the content by going through the fields and items
        /// </summary>
        public void UpdateContent(IDatenMeisterScope scope, IEnumerable<IObject> items, IElement formDefinition)
        {
            Items = items;
            FormDefinition = formDefinition;
            Scope = scope;


            UpdateContent();
        }

        private void UpdateContent()
        {
            var umlNameResolve = Scope.Resolve<IUmlNameResolution>();

            var listItems = new List<ExpandoObject>();
            _itemMapping.Clear();

            if (!(FormDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            DataGrid.Columns.Clear();
            foreach (var field in fields.Cast<IElement>())
            {
                var name = field.get(_FormAndFields._FieldData.name).ToString();
                var title = field.get(_FormAndFields._FieldData.title);
                var isEnumeration = ObjectHelper.IsTrue(field.get(_FormAndFields._FieldData.isEnumeration));
                var dataColumn = new DataGridTextColumn
                {
                    Header = title,
                    Binding = new Binding(name),
                    IsReadOnly = isEnumeration
                };

                DataGrid.Columns.Add(dataColumn);
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    var itemObject = new ExpandoObject();
                    var asDictionary = (IDictionary<string, object>) itemObject;

                    foreach (var field in fields.Cast<IElement>())
                    {
                        var name = field.get(_FormAndFields._FieldData.name).ToString();
                        var isEnumeration = ObjectHelper.IsTrue(field.get(_FormAndFields._FieldData.isEnumeration));
                        var value = item.isSet(name) ? item.get(name) : null;

                        if (isEnumeration)
                        {
                            var result = new StringBuilder();
                            var valueAsList = DotNetHelper.AsEnumeration(value);
                            var nr = string.Empty;
                            foreach (var valueElement in valueAsList)
                            {
                                result.Append(nr + umlNameResolve.GetName(valueElement));
                                nr = "\r\n";
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

        private IObject SelectedItem
        {
            get
            {
                var selectedItem = DataGrid.SelectedItem;
                if (selectedItem == null)
                {
                    return null;
                }

                return _itemMapping[(ExpandoObject)selectedItem];

            }
        }
    }
}
