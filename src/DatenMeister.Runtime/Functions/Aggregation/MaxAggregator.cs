using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class MaxAggregator : DoubleAggregationByFunction
    {
        public MaxAggregator(object property) : base(property, Math.Max)
        {
            Start = Double.MinValue;
        }
    }
}