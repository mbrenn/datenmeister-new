using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields;

/// <summary>
/// Method being used for a dropdown field
/// </summary>
public class DropdownField : IDetailField
{
    private ComboBox? _comboBox;

    private string _name = string.Empty;

    private object? _propertyValue;

    public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
        FieldParameter fieldFlags)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
        if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

        _name = fieldData.get<string>(_Forms._FieldData.name);
        var isReadOnly = fieldData.getOrDefault<bool>(_Forms._DropDownFieldData.isReadOnly)
                         || fieldFlags.IsReadOnly;

        _propertyValue = null;
        if (value.isSet(_name))
        {
            _propertyValue = value.get(_name);
        }
        else
        {
            if (fieldData.isSet(_Forms._FieldData.defaultValue))
            {
                _propertyValue = fieldData.get(_Forms._FieldData.defaultValue);
            }
        }

        List<KeyValuePair<string, object>>? values = null;

        var enumerationProperty =
            fieldData.getOrDefault<IElement>(_Forms._DropDownFieldData.valuesByEnumeration);
        if (enumerationProperty != null)
        {
            values = new List<KeyValuePair<string, object>>();
                
            var enumValues = EnumerationMethods.GetEnumValues(enumerationProperty);
            foreach (var enumValue in enumValues)
            {
                values.Add(new KeyValuePair<string, object>(enumValue, enumValue));
            }
        }

        if (values == null)
        {
            var dropDownValues =
                fieldData.getOrDefault<IReflectiveCollection>(_Forms._DropDownFieldData.values);
            if (values == null && dropDownValues == null)
            {
                return new TextBlock
                {
                    Text = "No values are given"
                };
            }
                
            values = dropDownValues.Select(x => x as IElement).Select(x =>
                new KeyValuePair<string, object>(
                    x.getOrDefault<string>(_Forms._ValuePair.name),
                    x.getOrDefault<object>(_Forms._ValuePair.value))
            ).ToList();
        }

        _comboBox = new ComboBox();
        var items = new List<ComboBoxItem>();
        ComboBoxItem? selectedBoxItem = null;

        foreach (var itemPair in values)
        {
            var nameOfItem = itemPair.Key;
            var valueOfItem = itemPair.Value;

            if (nameOfItem != null && valueOfItem != null)
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = nameOfItem,
                    Tag = valueOfItem
                };

                items.Add(comboBoxItem);

                var valueAsString = DotNetHelper.AsString(valueOfItem);
                var propertyValueAsString = DotNetHelper.AsString(_propertyValue);
                if (valueAsString != null && propertyValueAsString != null && valueAsString.Equals(propertyValueAsString)) 
                {
                    selectedBoxItem = comboBoxItem;
                }
            }
        }

        _comboBox.ItemsSource = items;
        _comboBox.SelectedValue = selectedBoxItem;
        _comboBox.IsEnabled = !isReadOnly;

        fieldFlags.CanBeFocused = true;
        return _comboBox;
    }

    public void CallSetAction(IObject element)
    {
        if (_comboBox != null && _propertyValue != _comboBox.SelectedValue)
        {
            element.set(_name, (_comboBox.SelectedValue as ComboBoxItem)?.Tag);
        }
    }
}