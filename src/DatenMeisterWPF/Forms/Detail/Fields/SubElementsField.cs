using System.Windows;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class SubElementsField : IDetailField
    {
        /// <summary>
        /// Creates the element
        /// </summary>
        /// <param name="value">Value to be shown</param>
        /// <param name="fieldData">Field of type</param>
        /// <param name="detailForm">Detail form control</param>
        /// <returns>The created UI Element</returns>
        public UIElement CreateElement(IElement value, IElement fieldData, DetailFormControl detailForm)
        {
            var name = fieldData.getOrDefault(_FormAndFields._FieldData.name).ToString();
            var valueOfElement = value.getOrDefault(name) as IReflectiveSequence;
            
            var listViewControl = new ListViewControl();
            listViewControl.SetContent(valueOfElement, fieldData);
            return listViewControl;
        }
    }
}