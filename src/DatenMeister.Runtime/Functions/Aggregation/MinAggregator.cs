using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class MinAggregator : DoubleAggregationByFunction
    {
        public MinAggregator(object property) : base(property, Math.Min)
        {
            Start = Double.MaxValue;
        }
    }
}