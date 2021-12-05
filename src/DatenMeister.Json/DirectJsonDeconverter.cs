using System.Text.Json;

namespace DatenMeister.Json
{
    /// <summary>
    /// Some helper methods
    /// </summary>
    public class DirectJsonDeconverter
    {
        /// <summary>
        /// Converts the given json value to a .Net value
        /// </summary>
        /// <param name="value">Value to be converted. If
        /// type of the element is a JsonElement, then this element will
        /// be converted to a .Net value</param>
        /// <returns>Converted value</returns>
        public static object? ConvertJsonValue(object? value)
        {
            object? propertyValue = null;
            if (value is JsonElement jsonElement)
            {
                propertyValue = jsonElement.ValueKind switch
                {
                    JsonValueKind.String => jsonElement.GetString(),
                    JsonValueKind.Number => jsonElement.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    _ => jsonElement.GetString()
                };
            }

            return propertyValue ?? value;
        }
    }
}
