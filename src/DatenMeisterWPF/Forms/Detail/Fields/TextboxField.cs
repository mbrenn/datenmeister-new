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
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            ref FieldFlags fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            var name = fieldData.get(_FormAndFields._FieldData.name).ToString();
            var isReadOnly = DotNetHelper.IsTrue(fieldData,_FormAndFields._FieldData.isReadOnly);
            var valueText = string.Empty;
            if (value.isSet(name))
            {
                valueText = value.get(name)?.ToString() ?? string.Empty;
            }
            else
            {
                if (fieldData.isSet(_FormAndFields._FieldData.defaultValue))
                {
                    valueText = fieldData.get(_FormAndFields._FieldData.defaultValue)?.ToString() ?? string.Empty;
                }
            }

            if (isReadOnly)
            {
                var contentBlock = new TextBlock
                {
                    Text = valueText
                };

                var copyToClipboardAdd = new MenuItem {Header = "Copy to Clipboard"};
                contentBlock.ContextMenu = new ContextMenu();
                contentBlock.ContextMenu.Items.Add(copyToClipboardAdd);
                copyToClipboardAdd.Click += (x, y) => Clipboard.SetText(valueText);

                return contentBlock;
            }
            else
            {
                var contentBlock = new TextBox
                {
                    Text = valueText
                };

                detailForm.SetActions.Add(
                    detailElement =>
                    {
                        if (valueText != contentBlock.Text)
                        {
                            detailElement.set(name, contentBlock.Text);
                        }
                    });

                fieldFlags = fieldFlags & ~FieldFlags.Focussed;

                return contentBlock;
            }
        }
    }
}