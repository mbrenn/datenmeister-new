using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    /// <summary>
    /// Method being used for a dropdown field
    /// </summary>
    public class DropdownField : IDetailField
    {
        private ComboBox _comboBox;

        private string _name;

        private object _propertyValue;

        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            _name = fieldData.get<string>(_FormAndFields._FieldData.name);
            _propertyValue = null;
            if (value.isSet(_name))
            {
                _propertyValue = value.get(_name);
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    _propertyValue = fieldData.get(_FormAndFields._FieldData.defaultValue);
                }
            }

            var dropDownValues = fieldData.getOrDefault<IReflectiveCollection>(_FormAndFields._DropDownFieldData.values);
            if (dropDownValues == null)
            {
                return new TextBlock
                {
                    Text = "No values are given"
                };
            }

            _comboBox = new ComboBox();
            var items = new List<ComboBoxItem>();
            ComboBoxItem selectedBoxItem = null;

            foreach (var itemPair in dropDownValues.Select(x=> x as IElement))
            {
                var nameOfItem = itemPair.getOrDefault<string>(_FormAndFields._ValuePair.name);
                var valueOfItem = itemPair.GetOrDefault(_FormAndFields._ValuePair.value);

                if (nameOfItem != null && valueOfItem != null)
                {
                    var comboBoxItem = new ComboBoxItem
                    {
                        Content = nameOfItem,
                        Tag = valueOfItem
                    };

                    items.Add(comboBoxItem);

                    if (valueOfItem.Equals(_propertyValue))
                    {
                        selectedBoxItem = comboBoxItem;
                    }
                }
            }

            _comboBox.ItemsSource = items;
            _comboBox.SelectedValue = selectedBoxItem;

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
}