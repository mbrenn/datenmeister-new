using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Detail.Fields
{
    public interface IDetailField
    {
        /// <summary>
        /// Creates the element by giving the value and the field data
        /// </summary>
        /// <param name="value">Value to be used</param>
        /// <param name="fieldData">Field Data to be used</param>
        /// <param name="detailForm">Defines the navigation host being used to navigate through the elemnt</param>
        /// <param name="fieldFlags"></param>
        /// <returns>Created UI Element</returns>
        UIElement CreateElement(
            IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags);

        /// <summary>
        /// This instance will be called, when the setting shall be performed upon the given element.
        /// This may be different as the one as specified in CreateElement
        /// </summary>
        /// <param name="element"> Element to be set</param>
        void CallSetAction(IObject element);

    }
}