using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class FilterOnPropertyByPredicateCollection<T>(
    IReflectiveCollection collection,
    string property,
    Predicate<T> filter)
    : ProxyReflectiveCollection(collection)
{
    /// <summary>
    ///     Stores the filter to filter on the property
    /// </summary>
    private readonly Predicate<T> _filter = filter;

    /// <summary>
    ///     Stores the property
    /// </summary>
    private readonly string _property = property;

    public override IEnumerator<object> GetEnumerator()
    {
        foreach (var value in Collection)
        {
            if (value is IObject valueAsObject && valueAsObject.isSet(_property))
            {
                var property = valueAsObject.get<T>(_property);
                if (_filter(property))
                {
                    yield return valueAsObject;
                }
            }
        }
    }

    public override int size()
    {
        var result = 0;
        foreach (var value in Collection)
        {
            if (value is IObject valueAsObject && valueAsObject.isSet(_property))
            {
                var property = valueAsObject.get<T>(_property);
                if (_filter(property))
                {
                    result++;
                }
            }
        }

        return result;
    }
}