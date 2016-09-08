using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class MinAggregator : DoubleAggregationByFunction
    {
        public MinAggregator() : base(Math.Min)
        {
            Start = Double.MaxValue;
        }
    }
}