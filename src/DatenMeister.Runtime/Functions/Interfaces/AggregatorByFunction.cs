using System;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public abstract class AggregatorByFunction<T> : Aggregator<T>
    {
        private T _start;
        private Func<object, object, T> _aggregation;
        private T _aggregationResult;

        internal Func<object, object, T> Aggregation
        {
            get { return _aggregation; }
            set { _aggregation = value; }
        }

        internal T Start
        {
            get { return _start; }
            set { _start = value; }
        }

        internal AggregatorByFunction(object property) : base(property)
        {
        }

        public AggregatorByFunction(object property, T start, Func<object, object, T> aggregation)
            : this(property)
        {
            _start = start;
            _aggregation = aggregation;
        }

        protected override void StartAggregation()
        {
            _aggregationResult = _start;
        }

        protected override void AggregateValue(object value)
        {
            _aggregationResult = _aggregation(_aggregationResult, value);
        }

        protected override T FinalizeAggregation()
        {
            return _aggregationResult;
        }
    }
}