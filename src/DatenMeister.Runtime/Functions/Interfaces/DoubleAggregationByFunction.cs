using System;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public abstract class DoubleAggregationByFunction : AggregatorByFunction<double>
    {
        public DoubleAggregationByFunction(
            Func<double, double, double> aggregation)
        {
            Start = 0.0;
            Aggregation = (a, b) => aggregation(Convert.ToDouble(a), Convert.ToDouble(b));
        }
    }
}