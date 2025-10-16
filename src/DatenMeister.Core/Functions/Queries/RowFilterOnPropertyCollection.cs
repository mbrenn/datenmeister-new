using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class RowFilterOnPropertyCollection(
    IReflectiveSequence collection,
    string property,
    object filterValue)
    : ProxyReflectiveCollection(collection)
{
    public override IEnumerator<object> GetEnumerator()
    {
        foreach (var value in Collection)
        {
            var valueAsObject = value as IObject;
            if (valueAsObject?.get(property)?.Equals(filterValue) == true)
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
            var valueAsObject = value as IObject;
            if (valueAsObject?.get(property)?.Equals(filterValue) == true)
            {
                result++;
            }
        }

        return result;
    }
}