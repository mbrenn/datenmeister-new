using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.UWP.Forms;
using DatenMeister.Web.Models;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.UWP
{
    /// <summary>
    /// Fills the form of the UWP with the fields being used
    /// </summary>
    public class DetailFormHelper
    {
        public FormViewSettings Settings { get; }

        private readonly Grid _gridButtons;

        public int CurrentRow { get; set; }

        private bool _deleteClickedOnce = false;

        /// <summary>
        /// Stores the element containing data
        /// </summary>
        public IElement DataElement { get; }

        /// <summary>
        /// Stores the list of binding actions being used to close the dialog
        /// </summary>
        public List<Action> BindingActions { get; } = new List<Action>();

        /// <summary>
        /// Stores the element containing all the fields
        /// </summary>
        public Grid GridFields { get; }

        public DetailFormHelper(FormViewSettings settings, IElement element, Grid gridFields, Grid gridButtons)
        {
            Settings = settings;
            DataElement = element;
            GridFields = gridFields;
            _gridButtons = gridButtons;
            GridFields.VerticalAlignment = VerticalAlignment.Top;
        }

        public void AddRows(IEnumerable<FieldData> fields)
        {
            foreach (var fieldData in fields)
            {
                if (fieldData is TextFieldData)
                {
                    AddTextColumn(fieldData as TextFieldData);
                }
                else if (fieldData is DateTimeFieldData)
                {
                    new DateTimeField().CreateField(this, fieldData as DateTimeFieldData);
                }
                else if (fieldData is DropDownFieldData)
                {
                    new DropDownField().CreateField(this, fieldData as DropDownFieldData);
                }
                else
                {
                    AddTextColumn(new TextFieldData(fieldData.title, fieldData.name));
                }
            }
        }

        /// <summary>
        /// Adds a row
        /// </summary>
        /// <param name="fieldData">Field Data to be used to create a new row</param>
        /// <param name="row">Number of row that was created</param>
        /// <returns>Textblock storing the key</returns>
        public TextBlock AddRow(FieldData fieldData, out int row)
        {
            row = CurrentRow;

            CurrentRow++;
            GridFields.RowDefinitions.Insert(row, new RowDefinition()
            {
                Height = GridLength.Auto
            });

            var textBlock = new TextBlock()
            {
                Text = $"{fieldData.title}: ",
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            Grid.SetColumn(textBlock, 0);
            Grid.SetRow(textBlock, row);

            GridFields.Children.Add(textBlock);
            return textBlock;
        }

        private void AddTextColumn(TextFieldData fieldData)
        {
            new TextField().CreateField(this, fieldData);
        }

        public void AddButtons(Action<DialogResult> onClose)
        {
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            var btnOk = new Button()
            {
                Margin = new Thickness(5),
                Content = "OK"
            };

            var btnCancel = new Button()
            {
                Margin = new Thickness(5),
                Content = "Cancel"
            };

            btnOk.Click += (x, y) =>
            {
                foreach (var action in BindingActions)
                {
                    action();
                }

                onClose(DialogResult.Submit);
            };

            btnCancel.Click += (x, y) =>
            {
                onClose(DialogResult.Cancel);
            };

            if (Settings.AllowDelete)
            {
                var btnDelete = new Button()
                {
                    Margin = new Thickness(5),
                    Content = "Delete"
                };

                btnDelete.Click += (x, y) =>
                {
                    if (!_deleteClickedOnce)
                    {
                        _deleteClickedOnce = true;
                        btnDelete.Content = "Confirm";
                    }
                    else
                    {
                        var correspondingExtent = DataElement.GetExtentOf();
                        correspondingExtent.elements().remove(DataElement);
                        onClose(DialogResult.Delete);
                    }
                };

                stackPanel.Children.Add(btnDelete);
            }

            stackPanel.Children.Add(btnCancel);
            stackPanel.Children.Add(btnOk);

            // Adds the stackpanel itself
            _gridButtons.Children.Add(stackPanel);
            CurrentRow++;
        }

        public enum DialogResult
        {
            Submit,
            Cancel,
            Delete
        }

        public static string GetValue(IElement element, string property)
        {
            return element.isSet(property) ? element.get(property).ToString() : string.Empty;
        }
    }
}
