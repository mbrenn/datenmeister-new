using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation;

public class ConcatAggregator(string separator = ", ")
    : AggregatorByFunction<string, string>(string.Empty, (x, y) => x == string.Empty ? y : $"{x}{separator}{y}");