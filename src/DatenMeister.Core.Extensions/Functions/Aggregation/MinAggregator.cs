using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation
{
    public class MinAggregator : DoubleAggregationByFunction<double>
    {
        public MinAggregator() : base(Math.Min)
        {
            Start = double.MaxValue;
        }
    }
}