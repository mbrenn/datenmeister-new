using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation.Uml
{
    /// <summary>
    /// This class includes several methods to support the interaction
    /// between the MOF model and the UML rules
    /// </summary>
    public static class MofHelper
    {
        /// <summary>
        /// Checks whether the value at the given property for the element is the default value.
        /// This infers that the value does not have to be set within the underlying database.
        /// </summary>
        /// <param name="element">Element to be queried</param>
        /// <param name="property">Property to which the element would be set</param>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if the value is a dafault value</returns>
        public static bool IsDefaultValue(IObject element, string property, object? value)
        {
            switch (value)
            {
                case null:
                case int intValue when intValue == 0:
                case bool boolValue when boolValue == false:
                case string stringValue when string.IsNullOrEmpty(stringValue):
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the default value for the given type.
        /// </summary>
        /// <param name="type">Type, whose default value is queried</param>
        /// <returns>The default value</returns>
        public static object? GetDefaultValue(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type == typeof(string))
            {
                return null;
            }

            if (type == typeof(double))
            {
                return 0.0;
            }

            if (type == typeof(int))
            {
                return 0;
            }

            if (type == typeof(bool))
            {
                return false;
            }

            if (type == typeof(DateTime))
            {
                return DateTime.MinValue;
            }

            throw new ArgumentException(
                $@"Type of element is not supported {type.FullName}",
                nameof(type));
        }
    }
}