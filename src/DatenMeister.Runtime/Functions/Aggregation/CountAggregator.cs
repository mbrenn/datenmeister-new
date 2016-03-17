using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class CountAggregator : DoubleAggregationByFunction
    {
        public CountAggregator(object property)
            : base(property, (a, b) => a + 1)
        {
        }
    }
}