using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class FilterOnElementType<T>(IReflectiveCollection collection) : ProxyReflectiveCollection(collection)
{
    public override IEnumerator<object> GetEnumerator()
    {
        foreach (var value in Collection)
        {
            if (value is T)
            {
                yield return value;
            }
        }
    }

    public override int size()
    {
        var result = 0;
        foreach (var value in Collection)
        {
            if (value is T) 
            {
                result++;
            }
        }

        return result;
    }
}