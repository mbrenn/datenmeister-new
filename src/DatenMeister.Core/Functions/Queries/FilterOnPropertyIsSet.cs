using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

/// <summary>
/// Performs a filtering on all properties
/// </summary>
public class FilterOnPropertyIsSet : ProxyReflectiveCollection
{
    private readonly string _property;

    public FilterOnPropertyIsSet(IReflectiveCollection collection, string property) : base(collection)
    {
        _property = property;
    }

    public override IEnumerator<object> GetEnumerator()
    {
        foreach (var element in Collection.OfType<IObject>())
        {
            if (element.isSet(_property))
            {
                yield return element;
            }
        }
    }

    public override int size()
    {
        return Collection.OfType<IObject>().Count(x => x.isSet(_property));
    }
}