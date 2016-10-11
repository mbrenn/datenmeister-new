using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class MaxAggregator : DoubleAggregationByFunction<double>
    {
        public MaxAggregator() : base(Math.Max)
        {
            Start = Double.MinValue;
        }
    }
}