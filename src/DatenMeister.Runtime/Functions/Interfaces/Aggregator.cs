using System.Threading;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public abstract class Aggregator<T> : IAggregator<T>
    {
        private bool _isStarted;

        internal Aggregator()
        {
        }

        /// <summary>
        /// Aggregates the values of a property into a single value
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <returns>The aggregated value</returns>
        public void Add(T value)
        {
            if (!_isStarted)
            {
                StartAggregation();
                _isStarted = true;
            }

            AggregateValue(value);
        }

        public T Result
        {
            get { return FinalizeAggregation(); }
        }

        protected abstract void StartAggregation();

        protected abstract void AggregateValue(object value);

        protected abstract T FinalizeAggregation();
    }
}