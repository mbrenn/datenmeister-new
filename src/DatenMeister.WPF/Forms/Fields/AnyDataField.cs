using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    public class AnyDataField : IDetailField
    {
        public UIElement? CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
        {
            var textBox = new TextBox();
            textBox.Text = "YES";
            return textBox;
        }

        public void CallSetAction(IObject element)
        {
        }
    }
}