 using System;
 using System.Collections.Generic;
 using System.Linq;
 using DatenMeister.Core.EMOF.Interface.Common;
 using DatenMeister.Core.EMOF.Interface.Reflection;
 using DatenMeister.Provider.InMemory;
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
            : base(new InMemoryReflectiveSequence())
        {
            Aggregate(collectionToBeAggregated,
                groupByColumn,
                new[] {aggregateColumn},
                new[] {aggregator});
        }

        public GroupByReflectiveCollection(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            IEnumerable<string> aggregateColumns,
            IEnumerable<Func<IAggregator<T>>> aggregators)
            : base(new InMemoryReflectiveSequence())
        {
            Aggregate(collectionToBeAggregated,
                groupByColumn,
                aggregateColumns,
                aggregators);
        }

        /// <summary>
        /// Performs the aggregation of a reflective collection
        /// </summary>
        /// <param name="collectionToBeAggregated">
        /// The reflective collection that shall be aggregated</param>
        /// <param name="groupByColumn">The column that shall be 
        /// used to group the values</param>
        /// <param name="aggregateColumns">The value that is used</param>
        /// <param name="aggregatorFunc">The function being used to 
        /// create a new aggregator</param>
        private void Aggregate(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            IEnumerable<string> aggregateColumns,
            IEnumerable<Func<IAggregator<T>>> aggregatorFunc)
        {
            var listColumns = aggregateColumns.ToList();
            var listAggregators = aggregatorFunc.ToList();

            if (listColumns.Count != listAggregators.Count)
            {
                throw new InvalidOperationException("The number of columns to functions are not equal");
            }

            Dictionary<object, List<IAggregator<T>>> aggregatedValues = 
                new Dictionary<object, List<IAggregator<T>>>();
            foreach (var element in collectionToBeAggregated.Cast<IObject>())
            {
                if (!element.isSet(groupByColumn))
                {
                    continue;
                }

                var groupByValue = element.get(groupByColumn);

                List<IAggregator<T>> aggregators;
                if (!aggregatedValues.TryGetValue(
                    groupByValue,
                    out aggregators))
                {
                    aggregators = new List<IAggregator<T>>();

                    foreach (var aggregateFactory in listAggregators)
                    {
                        var aggregator = aggregateFactory();
                        aggregators.Add(aggregator);
                    }

                    aggregatedValues[groupByValue] = aggregators;
                }

                var n = 0;
                // Checks if the Aggregators have been created
                foreach (var aggregateColumn in listColumns)
                {
                    // Add the value to the result
                    aggregators[n].Add(
                        (T) Convert.ChangeType(
                            element.getOrDefault(aggregateColumn),
                            typeof(T)));

                    n++;
                }
            }

            // Now store the values into the aggregation
            var mofFactory = new InMemoryFactory();
            foreach (var pair in aggregatedValues)
            {
                var element = mofFactory.create(null);
                element.set(groupByColumn, pair.Key);

                var n = 0;
                foreach (var aggregateColumn in listColumns)
                {
                    element.set(aggregateColumn, pair.Value[n].Result);
                    n++;
                }

                add(element);
            }
        }
    }
}
