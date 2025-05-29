using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation;

public class ConcatAggregator : AggregatorByFunction<string, string>
{
    public ConcatAggregator(string separator = ", ")
        : base(string.Empty, (x, y) => x == string.Empty ? y : $"{x}{separator}{y}")
    {
    }
}