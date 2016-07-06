 using System;
using System.Collections.Generic;
using System.Linq;
 using DatenMeister.EMOF.InMemory;
 using DatenMeister.EMOF.Interface.Common;
 using DatenMeister.EMOF.Interface.Reflection;
 using DatenMeister.Runtime.Functions.Interfaces;
 using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class GroupByReflectiveCollection<T> : ProxyReflectiveCollection
    {
        public GroupByReflectiveCollection(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            string aggregateColumn,
            Func<IAggregator<T>> aggregator)
            : base(new MofReflectiveSequence())
        {
            Aggregate(collectionToBeAggregated,
                groupByColumn,
                aggregateColumn,
                aggregator);
        }

        /// <summary>
        /// Performs the aggregation of a reflective collection
        /// </summary>
        /// <param name="collectionToBeAggregated">
        /// The reflective collection that shall be aggregated</param>
        /// <param name="groupByColumn">The column that shall be 
        /// used to group the values</param>
        /// <param name="aggregateColumn">The value that is used</param>
        /// <param name="aggregatorFunc">The function being used to 
        /// create a new aggregator</param>
        private void Aggregate(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            string aggregateColumn,
            Func<IAggregator<T>> aggregatorFunc)
        {
            Dictionary<object, IAggregator<T>> aggregatedValues = 
                new Dictionary<object, IAggregator<T>>();
            foreach (var element in collectionToBeAggregated.Cast<IObject>())
            {
                if (!element.isSet(groupByColumn)
                    || !element.isSet(aggregateColumn))
                {
                    // Skip the elements which do not have both columns
                    continue;
                }

                var value = element.get(groupByColumn);
                IAggregator<T> aggregator;
                if (!aggregatedValues.TryGetValue(
                    value,
                    out aggregator))
                {
                    aggregator = aggregatorFunc();
                    aggregatedValues[value] = aggregator;
                }

                // Add the value to the result
                aggregator.Add(
                    (T) Convert.ChangeType(
                        element.get(aggregateColumn), 
                        typeof(T)));
            }

            // Now store the values into the aggregation
            var mofFactory = new MofFactory();
            foreach (var pair in aggregatedValues)
            {
                var element = mofFactory.create(null);
                element.set(groupByColumn, pair.Key);
                element.set(aggregateColumn, pair.Value.Result);
                add(element);
            }
        }
    }
}
