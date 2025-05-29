using System.Windows;
using System.Windows.Controls;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields;

public class TextboxField : IDetailField, IPropertyValueChangeable, IInjectPropertyValue
{
    private string _name = string.Empty;
    private TextBox? _contentBlock;
    private string _valueText = string.Empty;

    public UIElement CreateElement(IObject value,
        IElement fieldData,
        DetailFormControl detailForm,
        FieldParameter fieldFlags)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
        if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));
            
        _name = fieldData.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
        var isReadOnly = fieldData.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isReadOnly)
                         || fieldFlags.IsReadOnly;
        var width = fieldData.getOrDefault<int>(_DatenMeister._Forms._TextFieldData.width);
        var height = fieldData.getOrDefault<int>(_DatenMeister._Forms._TextFieldData.lineHeight);
        var isEnumeration = fieldData.getOrDefault<bool>(_DatenMeister._Forms._TextFieldData.isEnumeration);

        _valueText = string.Empty;
        if (!string.IsNullOrEmpty(_name) && value.isSet(_name))
        {
            if (isEnumeration && isReadOnly)
            {
                _valueText = value.getOrDefault<IReflectiveSequence>(_name)
                    .ToList()
                    .Select(x=> x?.ToString() ?? string.Empty)
                    .Where (x=> !string.IsNullOrEmpty(x))
                    .Join("\r\n");
            }
            else
            {
                _valueText = value.getOrDefault<string>(_name) ?? string.Empty;
            }
        }
        else
        {
            if (fieldData.isSet(_DatenMeister._Forms._FieldData.defaultValue))
            {
                _valueText = fieldData.get(_DatenMeister._Forms._FieldData.defaultValue)?.ToString() ?? string.Empty;
            }
        }

        if (isReadOnly)
        {
            var contentBlock = new TextBlock
            {
                Text = _valueText,
                TextWrapping = TextWrapping.Wrap
            };

            var copyToClipboardAdd = new MenuItem {Header = "Copy to Clipboard"};
            contentBlock.ContextMenu = new ContextMenu();
            contentBlock.ContextMenu.Items.Add(copyToClipboardAdd);
            copyToClipboardAdd.Click += (x, y) => Clipboard.SetText(_valueText);

            return contentBlock;
        }

        _contentBlock = new TextBox
        {
            Text = _valueText,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        if (width > 0)
        {
            _contentBlock.Width = 10 * width;
            _contentBlock.HorizontalAlignment = HorizontalAlignment.Left;
        }
        else
        {
            _contentBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
            _contentBlock.MinWidth = 200;
        }

        if (height > 0)
        {
            _contentBlock.Height = 10 * height;
            _contentBlock.VerticalAlignment = VerticalAlignment.Top;
            _contentBlock.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            _contentBlock.TextWrapping = TextWrapping.Wrap;
            _contentBlock.AcceptsReturn = true;
        }
                
        _contentBlock.TextChanged += (x, y) =>
        {
            var ev = PropertyValueChanged;
            ev?.Invoke(this,
                new PropertyValueChangedEventArgs(detailForm, _name)
                {
                    NewValue = _contentBlock.Text
                });
        };

        fieldFlags.CanBeFocused = true;

        return _contentBlock;
    }

    public void CallSetAction(IObject element)
    {
        if (_contentBlock != null && _valueText != _contentBlock.Text)
        {
            element.set(_name, _contentBlock.Text);
        }
    }

    /// <summary>
    /// Defines the event that will be called when the property value is changed
    /// </summary>
    public event EventHandler<PropertyValueChangedEventArgs>? PropertyValueChanged;

    public void InjectValue(string propertyName, object value)
    {
        if (_name == propertyName && _contentBlock != null)
        {
            _contentBlock.Text = value.ToString();
        }
    }
}