using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Models;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.UWP
{
    /// <summary>
    /// Fills the form of the UWP with the fields being used
    /// </summary>
    public class DetailFormHelper
    {
        private readonly FormViewSettings _settings;
        private readonly IElement _element;
        private readonly Grid _fields;
        private readonly Grid _buttons;

        public int CurrentRow { get; set; }

        private bool _deleteClickedOnce = false;

        /// <summary>
        /// Stores the list of binding actions being used to close the dialog
        /// </summary>
        private readonly List<Action> _bindingActions = new List<Action>();

        public DetailFormHelper(FormViewSettings settings, IElement element, Grid fields, Grid buttons)
        {
            _settings = settings;
            _element = element;
            _fields = fields;
            _buttons = buttons;
            _fields.VerticalAlignment = VerticalAlignment.Top;
        }

        public void AddRows(IEnumerable<DataField> columns)
        {
            foreach (var column in columns)
            {
                if (column is TextDataField)
                {
                    AddTextColumn(column as TextDataField);
                }
                else if (column is DateTimeDataField)
                {
                    AddDateTimeColumn(column as DateTimeDataField);
                }
                else
                {
                    AddTextColumn(new TextDataField(column.title, column.name));
                }
            }
        }

        private TextBlock AddRow(DataField textField, out int row)
        {
            row = CurrentRow;

            CurrentRow++;
            _fields.RowDefinitions.Insert(row, new RowDefinition()
            {
                Height = GridLength.Auto
            });

            var textBlock = new TextBlock()
            {
                Text = $"{textField.title}: ",
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            Grid.SetColumn(textBlock, 0);
            Grid.SetRow(textBlock, row);

            _fields.Children.Add(textBlock);
            return textBlock;
        }

        private void AddTextColumn(TextDataField textField)
        {
            int row;
            var textBlock = AddRow(textField, out row);

            var textBox = new TextBox
            {
                Text = GetValue(_element, textField.name),
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap
            };

            var lineHeight = Math.Max(1, textField.lineHeight);
            textBox.Height = 20 * lineHeight;
            if (lineHeight > 1)
            {
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBox.AcceptsReturn = true;
            }

            Grid.SetColumn(textBox, 1);
            Grid.SetRow(textBox, row);

            _fields.Children.Add(textBox);

            _bindingActions.Add(
                () => _element.set(textField.name, textBox.Text));
        }

        private void AddDateTimeColumn(DateTimeDataField dateTimeDataField)
        {
            int row;
            AddRow(dateTimeDataField, out row);
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            var dateTime = new DatePicker
            {
                Margin = new Thickness(5)
            };

            var time = new TimePicker
            {
                Margin = new Thickness(5)
            };

            Grid.SetColumn(stackPanel, 1);
            Grid.SetRow(stackPanel, row);

            if (dateTimeDataField.showDate)
            {
                stackPanel.Children.Add(dateTime);
            }

            if (dateTimeDataField.showTime)
            {
                stackPanel.Children.Add(time);
            }

            _fields.Children.Add(stackPanel);

            _bindingActions.Add(
                () => _element.set(dateTimeDataField.name, dateTime.Date.Date));
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
                foreach (var action in _bindingActions)
                {
                    action();
                }

                onClose(DialogResult.Submit);
            };

            btnCancel.Click += (x, y) =>
            {
                onClose(DialogResult.Cancel);
            };

            if (_settings.AllowDelete)
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
                        var correspondingExtent = _element.GetExtentOf();
                        correspondingExtent.elements().remove(_element);
                        onClose(DialogResult.Delete);
                    }
                };


                stackPanel.Children.Add(btnDelete);
            }

            stackPanel.Children.Add(btnCancel);
            stackPanel.Children.Add(btnOk);

            // Adds the stackpanel itself
            _buttons.Children.Add(stackPanel);
            CurrentRow++;
        }

        public enum DialogResult
        {
            Submit,
            Cancel,
            Delete
        }

        public static string GetValue(IElement element, object property)
        {
            return element.isSet(property) ? element.get(property).ToString() : string.Empty;
        }
    }
}
