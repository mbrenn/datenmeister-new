using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class RowFilterOnAnyProperty(IReflectiveCollection collection) : ProxyReflectiveCollection(collection)
{
    public string FreeText { get; set; } = string.Empty;
    


    public override IEnumerator<object> GetEnumerator()
    {
        foreach (var value in Collection)
        {
            if (value is IElement valueAsObject && IsInList(valueAsObject))
            {
                yield return valueAsObject;
            }
        }
    }

    public override int size()
    {
        var result = 0;
        foreach (var value in Collection)
        {
            var valueAsObject = value as IElement;
            if (valueAsObject == null) 
                continue;
                
            if (IsInList(valueAsObject))
            {
                result++;
            }
        }

        return result;
    }
    
    /// <summary>
    /// Verifies whether the element shall be given in the list
    /// </summary>
    /// <param name="valueAsObject">Value to be shown</param>
    /// <returns>true, if value is in</returns>
    private bool IsInList(IElement valueAsObject)
    {
        if (string.IsNullOrEmpty(FreeText))
        {
            return true;
        }

        if (valueAsObject is not IObjectAllProperties properties)
        {
            return false;
        }

        foreach (var property in properties.getPropertiesBeingSet())
        {
            var content = valueAsObject.get(property)?.ToString();
            content = content?.ToLower();
            if (content?.Contains(FreeText.ToLower()) == true)
            {
                return true;
            }
        }
        
        return false;
    }
}