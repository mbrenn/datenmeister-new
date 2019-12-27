using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    public class SeparatorLineField : IDetailField
    {
        public UIElement CreateElement(
            IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            var rectangele = new Canvas
            {
                Background = Brushes.Black,
                Margin = new Thickness(0,5,0,5),
                Height = 1
            };

            fieldFlags.IsSpanned = true;

            return rectangele;
        }

        public void CallSetAction(IObject element)
        {
            return;
        }
    }
}