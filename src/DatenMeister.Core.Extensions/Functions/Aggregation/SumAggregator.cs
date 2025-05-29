using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation;

/// <summary>
/// Sums up a property
/// </summary>
public class SumAggregator : DoubleAggregationByFunction<double>
{
    public SumAggregator()
        : base((a, b) => a + b)
    {
    }
}