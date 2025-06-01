using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation;

/// <summary>
/// Sums up a property
/// </summary>
public class SumAggregator() : DoubleAggregationByFunction<double>((a, b) => a + b);