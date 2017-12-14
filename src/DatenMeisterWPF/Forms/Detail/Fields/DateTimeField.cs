using System;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class DateTimeField : IDetailField
    {
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));

            throw new System.NotImplementedException();
        }
    }
}