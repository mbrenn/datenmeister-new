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

        /// <summary>
        /// Gets the value whether the property is a collection by
        /// </summary>
        /// <param name="value">Value to ne evaluated</param>
        /// <returns>true, if the given property is a collection</returns>
        public static bool IsCollection(IObject value)
        {
            return value.getOrDefault<int>(_UML._CommonStructure._MultiplicityElement.upper) > 1;
        }
    }
}