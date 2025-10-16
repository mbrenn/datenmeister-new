using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class RowFilterOnMultipleProperties(
    IReflectiveCollection collection,
    IEnumerable<string> properties,
    string searchString,
    StringComparison comparison = StringComparison.CurrentCulture)
    : ProxyReflectiveCollection(collection)
{
    public override IEnumerator<object> GetEnumerator()
    {
        var properties1 = properties.ToList();
        foreach (var value in Collection)
        {
            var valueAsObject = value as IObject;
            if (valueAsObject == null)
            {
                continue;
            }

            foreach (var property in properties1)
            {
                if (valueAsObject.isSet(property) == false)
                    continue;

                var propertyAsText = valueAsObject.get(property);
                if (propertyAsText == null || !DotNetHelper.IsOfPrimitiveType(propertyAsText))
                    continue;

                if (propertyAsText.ToString()?.IndexOf(searchString, comparison) >= 0)
                {
                    yield return valueAsObject;
                    break;
                }
            }
        }
    }

    public override int size()
    {
        var result = 0;
        foreach (var value in Collection)
        {
            var valueAsObject = value as IObject;
            foreach (var property in properties)
            {
                if (valueAsObject?.isSet(property) == true &&
                    valueAsObject.get(property)?.ToString()?.Contains(searchString) == true)
                {
                    result++;
                    break;
                }
            }
        }

        return result;
    }
}