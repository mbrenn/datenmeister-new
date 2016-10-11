using System;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public abstract class DoubleAggregationByFunction<TItem> : AggregatorByFunction<double, TItem>
    {
        public DoubleAggregationByFunction(
            Func<double, TItem, double> aggregation)
        {
            Start = 0.0;
            Aggregation = (a, b) => aggregation(Convert.ToDouble(a), (TItem) Convert.ChangeType(b, typeof(TItem)));
        }
    }
}