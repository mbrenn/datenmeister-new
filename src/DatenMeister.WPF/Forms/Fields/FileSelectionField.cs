using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    public class FileSelectionField : IDetailField
    {
        private string _name = string.Empty;
        private TextBox? _textField;
        private string _valueText = string.Empty;

        public UIElement CreateElement(
            IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            _name = fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            var isReadOnly = fieldData.getOrDefault<bool>(_FormAndFields._FieldData.isReadOnly)
                || fieldFlags.IsReadOnly;
            
            _valueText = string.Empty;
            if (!string.IsNullOrEmpty(_name) && value.isSet(_name))
            {
                _valueText = value.getOrDefault<string>(_name) ?? string.Empty;
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    _valueText = fieldData.getOrDefault<string>(_FormAndFields._FieldData.defaultValue) ?? string.Empty;
                }
            }

            // Defines the dockpanel
            var dockPanel = new DockPanel();

            // Defines the button allowing the user to select the file
            var button = new Button
            {
                Content = "Select...",
                IsEnabled = !isReadOnly
            };

            DockPanel.SetDock(button, Dock.Right);
            dockPanel.Children.Add(button);

            // Defines the textbox containing the path
            var textField = new TextBox
            {
                Text = _valueText,
                IsReadOnly = isReadOnly,
                MinWidth = 200
            };
            dockPanel.Children.Add(textField);

            _textField = textField;
            dockPanel.Focusable = true;
            dockPanel.GotFocus += (x, y) => { _textField.Focus(); };

            // Sets the event handler for the button
            button.Click += (x, y) =>
            {
                if (isReadOnly)
                {
                    // Field is read-only, we don't use it
                    return;
                }

                var isSaving = fieldData.getOrDefault<bool>(_FormAndFields._FileSelectionFieldData.isSaving);
                var defaultExtension =
                    fieldData.getOrDefault<string>(_FormAndFields._FileSelectionFieldData.defaultExtension);
                var initialDirectory =
                    fieldData.getOrDefault<string>(_FormAndFields._FileSelectionFieldData.initialPathToDirectory);

                if (isSaving)
                {
                    var dlg = new Microsoft.Win32.SaveFileDialog
                    {
                        DefaultExt = defaultExtension,
                        InitialDirectory = _valueText ?? initialDirectory ?? string.Empty,
                        OverwritePrompt = true,
                        RestoreDirectory = true
                    };

                    if (!string.IsNullOrEmpty(defaultExtension))
                    {
                        dlg.Filter = $"({defaultExtension})|{defaultExtension}|All Files (*.*)|*.*";
                    }

                    if (dlg.ShowDialog() == true)
                    {
                        _textField.Text = dlg.FileName;
                    }
                }
                else /* !isSaving */
                {
                    var dlg = new Microsoft.Win32.OpenFileDialog
                    {
                        DefaultExt = defaultExtension,
                        InitialDirectory = _valueText ?? string.Empty,
                        RestoreDirectory = true
                    };

                    if (!string.IsNullOrEmpty(defaultExtension))
                    {
                        dlg.Filter = $"({defaultExtension})|{defaultExtension}|All Files (*.*)|*.*";
                    }

                    if (dlg.ShowDialog() == true)
                    {
                        _textField.Text = dlg.FileName;
                    }
                }
            };
            
            fieldFlags.CanBeFocused = true;

            return dockPanel;
        }

        public void CallSetAction(IObject element)
        {
            if (_textField != null && _valueText != _textField.Text)
            {
                element.set(_name, _textField.Text);
            }
        }
    }
}