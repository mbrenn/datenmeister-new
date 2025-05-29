using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class TakeFirstQuery : ProxyReflectiveCollection
{
    private readonly int _number;

    public TakeFirstQuery(IReflectiveCollection collection, int number) : base(collection)
    {
        _number = number;
    }

    public override IEnumerator<object?> GetEnumerator()
    {
        var n = 0;
        foreach (var item in Collection)
        {
            if (n >= _number)
            {
                break;
            }

            yield return item;
            n++;
        }
    }

    public override int size()
    {
        return Math.Min(Collection.size(), _number);
    }
}