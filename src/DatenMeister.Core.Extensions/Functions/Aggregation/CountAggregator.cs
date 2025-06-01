using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation;

public class CountAggregator() : DoubleAggregationByFunction<object>((a, _) => a + 1);