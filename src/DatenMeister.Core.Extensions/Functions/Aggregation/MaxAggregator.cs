using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation
{
    public class MaxAggregator : DoubleAggregationByFunction<double>
    {
        public MaxAggregator() : base(Math.Max)
        {
            Start = double.MinValue;
        }
    }
}