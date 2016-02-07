using System;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class AverageAggregator : Aggregator<double>
    {
        private int _itemCount;
        private double _totalSum;

        public AverageAggregator(object property) : base(property)
        {
        }

        protected override void StartAggregation()
        {
            _itemCount = 0;
            _totalSum = 0;
        }

        protected override void AggregateValue(object value)
        {
            _itemCount++;
            _totalSum += Convert.ToDouble(value);
        }

        protected override double FinalizeAggregation()
        {
            return _totalSum/_itemCount;
        }
    }
}