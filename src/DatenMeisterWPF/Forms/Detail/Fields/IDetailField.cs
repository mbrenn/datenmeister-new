using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public interface IDetailField
    {
        /// <summary>
        /// Creates the element by giving the value and the field data
        /// </summary>
        /// <param name="value">Value to be used</param>
        /// <param name="fieldData">Field Data to be used</param>
        /// <param name="detailForm">Defines the navigation host being used to navigate through the elemnt</param>
        /// <returns>Created UI Element</returns>
        UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm);

    }
}