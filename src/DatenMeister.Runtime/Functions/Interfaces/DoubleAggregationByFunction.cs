using System;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public abstract class DoubleAggregationByFunction : AggregatorByFunction<double>
    {
        public DoubleAggregationByFunction(
            object property, 
            Func<double, double, double> aggregation) 
            : base(property)
        {
            Start = 0.0;
            Aggregation = (a, b) => aggregation(Convert.ToDouble(a), Convert.ToDouble(b));
        }
    }
}