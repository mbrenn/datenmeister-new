using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using System.Linq;
using System.Text.Json;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Json
{
    /// <summary>
    /// Some helper methods
    /// </summary>
    public class DirectJsonDeconverter
    {
        private readonly IWorkspaceLogic? _workspaceLogic;

        public DirectJsonDeconverter()
        {
            
        }

        public DirectJsonDeconverter(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }
        
        /// <summary>
        /// Converts the given json value to a .Net value
        /// </summary>
        /// <param name="value">Value to be converted. If
        /// type of the element is a JsonElement, then this element will
        /// be converted to a .Net value</param>
        /// <returns>Converted value</returns>
        public object? ConvertJsonValue(object? value)
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
                    JsonValueKind.Object => ConvertToObject(
                        JsonSerializer.Deserialize<MofObjectAsJson>(jsonElement.GetRawText())
                        ?? throw new InvalidOperationException("Invalid Json for Conversion to MofObjectAsJson")),
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
        public IElement? ConvertToObject(MofObjectAsJson jsonObject)
        {
            if (!string.IsNullOrEmpty(jsonObject.r) && !string.IsNullOrEmpty(jsonObject.w))
            {
                if (_workspaceLogic != null)
                {
                    // We have a reference
                    return _workspaceLogic.FindItem(
                        jsonObject.w,
                        jsonObject.r
                    );
                }

                throw new InvalidOperationException("No workspace collection is given for reference");
            }
            else
            {
                var result = InMemoryObject.CreateEmpty(jsonObject.m?.uri ?? string.Empty);

                foreach (var pair in jsonObject.v)
                {
                    result.set(pair.Key, ConvertJsonValue(pair.Value));
                }

                return result;
            }
        }
    }
}