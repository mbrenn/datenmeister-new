using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class CheckboxField : IDetailField
    {
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            ref FieldFlags fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            var name = fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            var isReadOnly = fieldData.getOrDefault<bool>(_FormAndFields._FieldData.isReadOnly);
            bool? propertyValue = null;
            if (value.isSet(name))
            {
                propertyValue = value.getOrDefault<bool>(name);
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    propertyValue = fieldData.getOrDefault<bool>(_FormAndFields._FieldData.defaultValue);
                }
            }

            var box = new CheckBox
            {
                IsChecked = propertyValue,
                IsEnabled = isReadOnly
            };

            detailForm.SetActions.Add(
                element =>
                {
                    if (propertyValue != box.IsChecked)
                    {
                        element.set(name, box.IsChecked);
                    }
                });

            return box;
        }
    }
}