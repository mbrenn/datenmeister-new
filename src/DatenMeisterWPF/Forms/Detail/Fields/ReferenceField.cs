using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    /// <summary>
    /// Implements a reference field which is shown the currently selected instance and allows the user to select 
    /// another instance to set the appropriate property
    /// </summary>
    public class ReferenceField : IDetailField
    {
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, ref FieldFlags fieldFlags)
        {
            return new TextBlock
            {
                Text = "REFERNECE PARTY"
            };
        }
    }
}