using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Detail.Fields
{
    public class CheckboxField : IDetailField
    {
        private bool? _propertyValue;
        private string _name;
        private CheckBox _checkbox;

        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            _name = fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            var isReadOnly = fieldData.getOrDefault<bool>(_FormAndFields._FieldData.isReadOnly);
            _propertyValue = null;
            if (value.isSet(_name))
            {
                _propertyValue = value.getOrDefault<bool>(_name);
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    _propertyValue = fieldData.getOrDefault<bool>(_FormAndFields._FieldData.defaultValue);
                }
            }

            _checkbox = new CheckBox
            {
                IsChecked = _propertyValue,
                IsEnabled = !isReadOnly 
            };

            fieldFlags.CanBeFocused = true;
            return _checkbox;
        }

        public void CallSetAction(IObject element)
        {
            if (_checkbox != null && _propertyValue != _checkbox.IsChecked)
            {
                element.set(_name, _checkbox.IsChecked);
            }
        }
    }
}