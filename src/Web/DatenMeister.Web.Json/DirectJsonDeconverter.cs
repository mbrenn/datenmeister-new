using DatenMeister.Core.Provider.InMemory;
using System.Text.Json;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Web.Json;

/// <summary>
/// Some helper methods for converting json values to Xmi
/// </summary>
public class DirectJsonDeconverter
{
    private static long _counter = 0;
    
    private static string GetNextCounter()
    {
        return Interlocked.Increment(ref _counter).ToString();
    }
    
    /// <summary>
    /// This helper class supports the dereferencing of shadows to actual objects
    /// </summary>
    /// <param name="Shadow">Shadow being used</param>
    /// <param name="ActionToSetShadow">This method sets the new object instead of the shadow</param>
    private class ShadowInformation(MofObjectShadow Shadow, Action<IObject> ActionToSetShadow)
    {
        public MofObjectShadow Shadow { get; } = Shadow;
        public Action<IObject> ActionToSetShadow { get; } = ActionToSetShadow;
    }

    private readonly IWorkspaceLogic? _workspaceLogic;
    private readonly IScopeStorage? _scopeStorage;

    private string prefixCounter = string.Empty;

    /// <summary>
    /// Defines a list of shadow object that are created during the conversion and are used to resolve references within each conversion
    /// </summary>
    private List<ShadowInformation> Shadows { get; set; } = [];

    // Stores a list of all references which are used to replace the shadows with the actual objects
    private List<IElement> References { get; set; } = [];

    public DirectJsonDeconverter()
    {
            
    }

    public DirectJsonDeconverter(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
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
                JsonValueKind.Object when jsonElement.TryGetProperty("0", out JsonElement _) =>
                    ConvertFromArray(jsonElement),
                JsonValueKind.Object when !jsonElement.TryGetProperty("0", out JsonElement _) =>
                    ConvertToObjectInternal(
                        JsonSerializer.Deserialize<MofObjectAsJson>(jsonElement.GetRawText())
                        ?? throw new InvalidOperationException("Invalid Json for Conversion to MofObjectAsJson")),
                JsonValueKind.Array => jsonElement.EnumerateArray().Select(x => ConvertJsonValue(x)).ToList(),
                _ => jsonElement.GetString()
            };
        }

        return propertyValue ?? value;
    }

    /// <summary>
    /// Converts the given json element being an object to an array
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <returns></returns>
    private List<object> ConvertFromArray(JsonElement jsonElement)
    {
        var result = new List<object>();

        // Walk through the object by starting at 0 until the last element is retrieved
        var index = 0;
        while (jsonElement.TryGetProperty(index.ToString(), out var element))
        {
            var lineResult = ConvertJsonValue(element);

            // Allows to update the shadow object in case the reference can be resolved
            if (lineResult is MofObjectShadow shadow)
            {
                var indexInner = index;
                Shadows.Add(new ShadowInformation(shadow, x => result[indexInner] = x));
            }

            if (lineResult is not null)
            {
                result.Add(lineResult);
            }

            index++;
        }

        return result;
    }

    /// <summary>
    /// Flag to be sure that there is only one caller within that instance. 
    /// </summary>
    private bool _isInCall;

    /// <summary>
    /// Takes a MofObjectAsJson element and converts it back to an IObject element
    /// </summary>
    /// <param name="jsonObject">Json Object to be converted</param>
    /// <returns>The converted Json Object</returns>
    public IObject? ConvertToObject(MofObjectAsJson jsonObject)
    {
        if (_isInCall)
        {
            throw new InvalidOperationException("The method is not allowed to be multiple times in different threads");
        }

        try
        {
            _isInCall = true;
            prefixCounter = GetNextCounter();
            Shadows.Clear();
            References.Clear();

            var result = ConvertToObjectInternal(jsonObject, true);

            // Now replaces all the shadows with the actual instances
            foreach (var shadowInfo in Shadows)
            {
                var shadow = shadowInfo.Shadow;
                var actualObject = References.FirstOrDefault(x => "#" + x.GetId() == shadow.Uri);
                if (actualObject != null)
                {
                    shadowInfo.ActionToSetShadow(actualObject);
                }
            }

            // Cleans up the memory at the end of the call
            Shadows.Clear();
            References.Clear();
            return result;
        }
        finally
        {
            _isInCall = false;
        }
    }

    /// <summary>
    /// Converts the object to an IObject element and tries to resolve the references in case the rootcall is being exiecte
    /// </summary>
    /// <param name="jsonObject">Json Object to be converted</param>
    /// <param name="isRoot">true, only if it is the first call to add the element</param>
    /// <returns>The converted object as a Mof Object</returns>
    private IObject? ConvertToObjectInternal(MofObjectAsJson jsonObject, bool isRoot = false)
    { 
        IObject? result = null;
        // Checks, that we are having a reference            
        if (!string.IsNullOrEmpty(jsonObject.r))
        {
            if (_workspaceLogic != null && !string.IsNullOrEmpty(jsonObject.w))
            {
                // We have a reference
                result = _workspaceLogic.FindObject(
                    jsonObject.w,
                    jsonObject.r
                );
            }

            if (result == null)
            {
                // Create a shadow object, so the final object can be resolved later
                return new MofObjectShadow(jsonObject.r);                    
            }
        }
        else
        {
            if (_workspaceLogic != null && _scopeStorage != null)
            {
                var temporaryExtentLogic = new TemporaryExtentLogic(_workspaceLogic, _scopeStorage);
                var temporaryExtent = temporaryExtentLogic.TryGetTemporaryExtent();
                if (temporaryExtent != null)
                {
                    if (isRoot)
                    {
                        result = temporaryExtentLogic.CreateTemporaryElementByUri(
                            jsonObject.m?.uri ?? string.Empty);
                    }
                    else
                    {
                        result = MofFactory.CreateElementWithMetaClassUri(
                            temporaryExtentLogic.TemporaryExtent,
                            jsonObject.m?.uri ?? string.Empty);
                    }
                }
            }

            result ??= InMemoryObject.CreateEmpty(jsonObject.m?.uri ?? string.Empty);
            
            // Sets the id, if the id is given
            if (!string.IsNullOrEmpty(jsonObject.id) && result.GetId() != jsonObject.id)
            {
                result.SetId(prefixCounter + jsonObject.id);
            }

            // Adds the element to the references
            if (result is IElement asElement)
            {
                References.Add(asElement);
            }
                
            foreach (var pair in jsonObject.v)
            {
                var value = ConvertJsonValue(pair.Value);
                if(value is MofObjectShadow shadow)
                {
                    shadow.Uri = "#" + prefixCounter + shadow.Uri.Replace("#", "");
                    Shadows.Add(new ShadowInformation(
                        shadow, 
                        x => result.set(pair.Key, x)));
                }

                result.set(pair.Key, value);
            }   
        }

        return result;
    }
}