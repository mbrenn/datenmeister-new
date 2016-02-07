using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    /// <summary>
    /// Sums up a property
    /// </summary>
    public class SumAggregator : DoubleAggregationByFunction
    {
        public SumAggregator(object property) 
            : base(property, (a, b) => a + b)
        {
        }
    }
}