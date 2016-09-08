using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class CountAggregator : DoubleAggregationByFunction
    {
        public CountAggregator()
            : base((a, b) => a + 1)
        {
        }
    }
}