using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
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

        private readonly IDictionary<ExpandoObject, IObject> _itemMapping = new Dictionary<ExpandoObject, IObject>();

        public void UpdateContent()
        {
            var items = new List<ExpandoObject>();
            _itemMapping.Clear();

            if (!(FormDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }
            
            foreach (var field in fields.Cast<IElement>())
            {
                var name = field.get(_FormAndFields._FieldData.name).ToString();
                var title = field.get(_FormAndFields._FieldData.title);
                var dataColumn = new DataGridTextColumn
                {
                    Header = title,
                    Binding = new Binding(name)
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
                        var value = item.isSet(name) ? item.get(name) : null;

                        asDictionary.Add(name, value);
                    }

                    _itemMapping[itemObject] = item;
                    items.Add(itemObject);
                }
            }

            DataGrid.ItemsSource = items;
        }

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
                var dlg = new ItemXmlViewWindow();
                dlg.UpdateContent(FormDefinition);
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

                return _itemMapping[selectedItem as ExpandoObject];

            }
        }
    }
}
