using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class TextboxField : IDetailField
    {
        private string _name;
        private TextBox _contentBlock;
        private string _valueText;

        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            _name = fieldData.get(_FormAndFields._FieldData.name).ToString();
            var isReadOnly = fieldData.get<bool>(_FormAndFields._FieldData.isReadOnly);
            var width = fieldData.getOrDefault<int>(_FormAndFields._TextFieldData.width);

            _valueText = string.Empty;
            if (value.isSet(_name))
            {
                _valueText = value.getOrDefault<string>(_name) ?? string.Empty;
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    _valueText = fieldData.get(_FormAndFields._FieldData.defaultValue)?.ToString() ?? string.Empty;
                }
            }

            if (isReadOnly)
            {
                var contentBlock = new TextBlock
                {
                    Text = _valueText
                };

                var copyToClipboardAdd = new MenuItem {Header = "Copy to Clipboard"};
                contentBlock.ContextMenu = new ContextMenu();
                contentBlock.ContextMenu.Items.Add(copyToClipboardAdd);
                copyToClipboardAdd.Click += (x, y) => Clipboard.SetText(_valueText);

                return contentBlock;
            }
            else
            {
                _contentBlock = new TextBox
                {
                    Text = _valueText
                };

                if (width > 0)
                {
                    _contentBlock.Width = 10 * width;
                    _contentBlock.HorizontalAlignment = HorizontalAlignment.Left;
                }
                else
                {
                    _contentBlock.Width = 300;
                    _contentBlock.HorizontalAlignment = HorizontalAlignment.Left;
                }

                fieldFlags.CanBeFocussed = true;

                return _contentBlock;
            }
        }

        public void CallSetAction(IObject element)
        {
            if (_contentBlock != null && _valueText != _contentBlock.Text)
            {
                element.set(_name, _contentBlock.Text);
            }
        }
    }
}