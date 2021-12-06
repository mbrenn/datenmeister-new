using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using System.Linq;
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
                    JsonValueKind.Null => null,
                    JsonValueKind.Undefined => null,
                    JsonValueKind.Object => ConvertToObject(JsonSerializer.Deserialize<MofObjectAsJson>(jsonElement.GetRawText())),
                    JsonValueKind.Array => jsonElement.EnumerateArray().Select(x => ConvertJsonValue(x)).ToList(),
                    _ => jsonElement.GetString()
                };
            }

            return propertyValue ?? value;
        }

        /// <summary>
        /// Takes a MofObjectAsJson element and converts it back to an IObject element
        /// </summary>
        /// <param name="jsonObject">Json Object to be converted</param>
        /// <returns>The converted Json Object</returns>
        public static IObject ConvertToObject(MofObjectAsJson jsonObject)
        {
            var result = InMemoryObject.CreateEmpty();

            foreach (var pair in jsonObject.v)
            {
                result.set(pair.Key, ConvertJsonValue(pair.Value));
            }

            return result;
        }
    }
}
