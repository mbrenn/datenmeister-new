#nullable disable

namespace DatenMeister.Core.Extensions.Functions.Interfaces
{
    public abstract class AggregatorByFunction<T, TItem> : Aggregator<T, TItem> where T : notnull
    {
        private T _aggregationResult;

        public AggregatorByFunction()
        {
        }

        public AggregatorByFunction(
            T start, Func<T, TItem, T> aggregation)
        {
            Start = start;
            Aggregation = aggregation;
        }

        internal Func<T, TItem, T> Aggregation { get; set; }

        internal T Start { get; set; }

        protected override void StartAggregation()
        {
            _aggregationResult = Start;
        }

        protected override void AggregateValue(TItem value)
        {
            _aggregationResult = Aggregation(_aggregationResult, value);
        }

        protected override T FinalizeAggregation()
        {
            return _aggregationResult;
        }
    }
}