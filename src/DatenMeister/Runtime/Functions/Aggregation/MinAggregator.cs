using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class MinAggregator : DoubleAggregationByFunction<double>
    {
        public MinAggregator() : base(Math.Min)
        {
            Start = double.MaxValue;
        }
    }
}