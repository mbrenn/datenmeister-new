using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation;

public class CountAggregator : DoubleAggregationByFunction<object>
{
    public CountAggregator()
        : base((a, b) => a + 1)
    {
    }
}