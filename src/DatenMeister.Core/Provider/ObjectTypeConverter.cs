using DatenMeister.Core.Helper;

namespace DatenMeister.Core.Provider
{
    /// <summary>
    /// Supports the conversion from an element depending on the specific object type
    /// This method should be used by every object provider
    /// </summary>
    public static class ObjectTypeConverter
    {
        /// <summary>
        /// Converts the given object according the rules of the ObjectTypeConverter
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="objectType">Type to be converted</param>
        /// <returns>The converted object</returns>
        public static object? Convert(object? value, ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.None:
                    return value;
                case ObjectType.Boolean:
                    return value != null && DotNetHelper.AsBoolean(value);
                case ObjectType.Enum:
                    return value;
                case ObjectType.String:
                    return value == null ? string.Empty : DotNetHelper.AsString(value);
                case ObjectType.Integer:
                    return value == null ? 0 : DotNetHelper.AsInteger(value);
                case ObjectType.Double:
                    return value == null ? 0.0 : DotNetHelper.AsDouble(value);
                case ObjectType.DateTime:
                    return value == null ? DateTime.MinValue : DotNetHelper.AsDateTime(value);
                case ObjectType.Element:
                    return value;
                case ObjectType.ReflectiveSequence:
                    return value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
        }
    }
}