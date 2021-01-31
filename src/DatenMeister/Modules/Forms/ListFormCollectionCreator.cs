using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Modules.Forms
{
    /// <summary>
    /// Takes the form information and the selected element and creates a list of elements
    /// for this
    /// </summary>
    public static class ListFormCollectionCreator
    {
        /// <summary>
        /// Gets the reflective collection for the given element, depending on the
        /// form and the selected value
        /// </summary>
        /// <param name="listForm">Form to be evaluated</param>
        /// <param name="value">Value, which is used to derive the collection</param>
        /// <returns>The collection being derived</returns>
        public static IReflectiveCollection GetCollection(IObject? listForm, IObject value)
        {
            if (value is IExtent extent)
            {
                return extent.elements();
            }
            else
            {
                var propertyName = listForm?.getOrDefault<string>(_DatenMeister._Forms._ListForm.property);
                
                return GetPropertiesAsReflection(value, propertyName);
            }
        }

        /// <summary>
        /// Gets the properties of an item as a reflection
        /// </summary>
        /// <param name="value">Object to be evaluated</param>
        /// <param name="propertyName">Name of the property or null, if all properties shall be returned</param>
        /// <returns></returns>
        private static IReflectiveCollection GetPropertiesAsReflection(IObject value, string? propertyName)
        {
            return string.IsNullOrEmpty(propertyName) 
                ? new PropertiesAsReflectiveCollection(value) 
                : value.get<IReflectiveCollection>(propertyName);
        }
    }
}