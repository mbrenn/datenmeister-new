using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Uml.Helper
{
    /// <summary>
    /// Defines some helper classes for property objects
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Gets the type of the property
        /// </summary>
        /// <param name="value">Property being queried</param>
        /// <returns>Type of the property</returns>
        public static IElement GetPropertyType(IObject value)
        {
            return value.getOrDefault<IElement>(_UML._CommonStructure._TypedElement.type);
        }
    }
}