using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class SeparatorLineField : IDetailField
    {
        public UIElement CreateElement(
            IObject value, 
            IElement fieldData, 
            DetailFormControl detailForm, 
            FieldParameter fieldFlags)
        {
            throw new System.NotImplementedException();
        }

        public void CallSetAction(IObject element)
        {
            return;
        }
    }
}