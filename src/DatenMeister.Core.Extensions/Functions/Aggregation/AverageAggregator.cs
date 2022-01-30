using System;
using DatenMeister.Core.Extensions.Functions.Interfaces;

namespace DatenMeister.Core.Extensions.Functions.Aggregation
{
    public class AverageAggregator : Aggregator<double, double>
    {
        private int _itemCount;
        private double _totalSum;

        protected override void StartAggregation()
        {
            _itemCount = 0;
            _totalSum = 0;
        }

        protected override void AggregateValue(double value)
        {
            _itemCount++;
            _totalSum += Convert.ToDouble(value);
        }

        protected override double FinalizeAggregation()
        {
            return _totalSum / _itemCount;
        }
    }
}