using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class TakeFirstQuery(IReflectiveCollection collection, int number) : ProxyReflectiveCollection(collection)
{
    public override IEnumerator<object?> GetEnumerator()
    {
        var n = 0;
        foreach (var item in Collection)
        {
            if (n >= number)
            {
                break;
            }

            yield return item;
            n++;
        }
    }

    public override int size()
    {
        return Math.Min(Collection.size(), number);
    }
}