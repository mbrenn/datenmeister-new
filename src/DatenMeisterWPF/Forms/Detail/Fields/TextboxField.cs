using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class TextboxField : IDetailField
    {
        public UIElement CreateElement(IElement value, IElement fieldData, DetailFormControl detailForm)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            var name = fieldData.get(_FormAndFields._FieldData.name).ToString();
            var contentBlock = new TextBox();

            var valueText = string.Empty;
            if (value?.isSet(name) == true)
            {
                valueText = value.get(name)?.ToString() ?? string.Empty;
                contentBlock.Text = valueText;
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    contentBlock.Text = fieldData.get(_FormAndFields._FieldData.defaultValue)?.ToString() ?? string.Empty;
                }
            }

            detailForm.SetActions.Add(
                () =>
                {
                    if (valueText != contentBlock.Text)
                    {
                        value.set(name, contentBlock.Text);
                    }
                });

            return contentBlock;
        }
    }
}