using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation.Uml
{
    /// <summary>
    /// This class includes several methods to support the interaction
    /// between the MOF model and the UML rules
    /// </summary>
    public static class MofUmlHelper
    {
        /// <summary>
        /// Checks whether the value at the given property for the element is the default value.
        /// This infers that the value does not have to be set within the underlying database. 
        /// </summary>
        /// <param name="element">Element to be queried</param>
        /// <param name="property">Property to which the element would be set</param>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if the value is a dafault value</returns>
        public static bool IsDefaultValue(IObject element, string property, object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is int intValue && intValue == 0)
            {
                return true;
            }

            if (value is bool boolValue && boolValue == false)
            {
                return true;
            }

            if ( value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                return true;
            }

            return false;
        }
    }
}