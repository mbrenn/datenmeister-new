using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class RowFilterOnPosition : ProxyReflectiveCollection
{
    /// <summary>
    /// Defines the position of the first row to be returned
    /// </summary>
    private int Position { get; }
    
    /// <summary>
    /// Defines the amount of rows to be returned
    /// </summary>
    private int Amount { get; }

    public RowFilterOnPosition(IReflectiveCollection collection, int position, int amount)
        :base(collection)
    {
        if (position < 0)
        {
            throw new ArgumentException("position must be >= 0");
        }

        if (amount < 0)
        {
            throw new ArgumentException("amount must be >= 0");       
        }
        
        Position = position;
        Amount = amount;
    }

    public override IEnumerator<object?> GetEnumerator()
    {
        return Collection.Skip(Position).Take(Amount).GetEnumerator();   
    }

    public override int size()
    {
        var totalSize = Collection.size();
        return Position >= totalSize ? 0 : Math.Min(Amount, totalSize - Position);
    }
}