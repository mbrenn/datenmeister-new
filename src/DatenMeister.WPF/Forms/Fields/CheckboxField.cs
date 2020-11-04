using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    public class CheckboxField : IDetailField, IPropertyValueChangeable
    {
        private bool? _propertyValue;
        
        private string _name = string.Empty;
        
        private CheckBox? _checkbox;

        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            _name = fieldData.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            var isReadOnly = fieldData.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isReadOnly)
                || fieldFlags.IsReadOnly;
            _propertyValue = null;
            if (value.isSet(_name))
            {
                _propertyValue = value.getOrDefault<bool>(_name);
            }
            else
            {
                if (fieldData.isSet(_DatenMeister._Forms._FieldData.defaultValue))
                {
                    _propertyValue = fieldData.getOrDefault<bool>(_DatenMeister._Forms._FieldData.defaultValue);
                }
            }
            
            _propertyValue ??= false;

            _checkbox = new CheckBox
            {
                IsChecked = _propertyValue,
                IsEnabled = !isReadOnly
            };

            _checkbox.Click += (x, y) =>
            {
                var ev = PropertyValueChanged;
                if (ev != null)
                {
                    ev(this,
                        new PropertyValueChangedEventArgs(detailForm, _name)
                        {
                            NewValue = _checkbox.IsChecked
                        });
                }
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
        
        /// <summary>
        /// Defines the event that will be called when the property value is changed
        /// </summary>
        public event EventHandler<PropertyValueChangedEventArgs>? PropertyValueChanged;
    }
}