using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Aggregation;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public abstract class Aggregator<T> : IAggregator<T>
    {
        private object _property;

        internal Aggregator(object property)
        {
            _property = property;
        }

        /// <summary>
        /// Aggregates the values of a property into a single value
        /// </summary>
        /// <param name="sequence">Sequence to be evaluated</param>
        /// <returns>The aggregated value</returns>
        public T Aggregate(IReflectiveSequence sequence)
        {
            StartAggregation();

            foreach (var value in sequence)
            {
                var valueAsIObject = value as IObject;
                if (valueAsIObject != null && valueAsIObject.isSet(_property))
                {
                    var propertyValue = valueAsIObject.get(_property);
                    AggregateValue(propertyValue);
                }
            }

            return FinalizeAggregation();
        }

        protected abstract void StartAggregation();

        protected abstract void AggregateValue(object value);

        protected abstract T FinalizeAggregation();
    }
}