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
        public UIElement CreateElement(IElement value, IElement fieldData, DetailFormControl detailForm)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            var name = fieldData.get(_FormAndFields._FieldData.name).ToString();
            var isReadOnly = DotNetHelper.IsTrue(fieldData, _FormAndFields._FieldData.isReadOnly);
            bool? propertyValue = null;
            if (value.isSet(name))
            {
                propertyValue = DotNetHelper.AsBoolean(value.get(name));
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    propertyValue = DotNetHelper.AsBoolean(fieldData.get(_FormAndFields._FieldData.defaultValue));
                }
            }

            var box = new CheckBox
            {
                IsChecked = propertyValue,
                IsEnabled = isReadOnly
            };

            detailForm.SetActions.Add(
                () =>
                {
                    if (propertyValue != box.IsChecked)
                    {
                        value.set(name, box.IsChecked);
                    }
                });

            return box;
        }
    }
}