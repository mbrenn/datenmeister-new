using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    /// <summary>
    /// Sums up a property
    /// </summary>
    public class SumAggregator : DoubleAggregationByFunction
    {
        public SumAggregator() 
            : base((a, b) => a + b)
        {
        }
    }
}