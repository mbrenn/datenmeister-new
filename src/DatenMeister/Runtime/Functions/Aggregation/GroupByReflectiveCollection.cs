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
    public class GroupByReflectiveCollection : ProxyReflectiveCollection
    {
        public GroupByReflectiveCollection(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            string aggregateColumn,
            Func<IAggregator> aggregator,
            string aggregatedColumn)
            : base(new InMemoryReflectiveSequence(null, null))
        {
            Aggregate(
                collectionToBeAggregated,
                groupByColumn,
                new[] {aggregateColumn},
                new[] {aggregator},
                new[] { aggregatedColumn });
        }

        public GroupByReflectiveCollection(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            IEnumerable<string> aggregateColumns,
            IEnumerable<Func<IAggregator>> aggregators,
            IEnumerable<string> aggregatedColumns)
            : base(new InMemoryReflectiveSequence(null, null))
        {
            Aggregate(
                collectionToBeAggregated,
                groupByColumn,
                aggregateColumns,
                aggregators, 
                aggregatedColumns);
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
        /// <param name="aggregatedColumns">List of property names to which the aggregated values wll be stored</param>
        private void Aggregate(
            IReflectiveCollection collectionToBeAggregated,
            string groupByColumn,
            IEnumerable<string> aggregateColumns,
            IEnumerable<Func<IAggregator>> aggregatorFunc,
            IEnumerable<string> aggregatedColumns)
        {
            if (aggregateColumns == null) throw new ArgumentNullException(nameof(aggregateColumns));
            if (aggregatorFunc == null) throw new ArgumentNullException(nameof(aggregatorFunc));
            if (aggregatedColumns == null) throw new ArgumentNullException(nameof(aggregatedColumns));

            var listAggregateColumns = aggregateColumns.ToList();
            var listAggregatedColumns = aggregatedColumns?.ToList();
            var listAggregators = aggregatorFunc.ToList();

            if (listAggregateColumns.Count != listAggregators.Count)
            {
                throw new InvalidOperationException(
                    "The number of columns to functions are not equal: listAggregateColumns.Count != listAggregators.Count");
            }

            if (listAggregatedColumns.Count != listAggregators.Count)
            {
                throw new InvalidOperationException(
                    "The number of columns to functions are not equal: listAggregatedColumns.Count != listAggregators.Count");
            }

            var aggregatedValues = 
                new Dictionary<object, List<IAggregator>>();
            foreach (var element in collectionToBeAggregated.Cast<IObject>())
            {
                if (!element.isSet(groupByColumn))
                {
                    continue;
                }

                var groupByValue = element.get(groupByColumn);

                List<IAggregator> aggregators;
                if (!aggregatedValues.TryGetValue(
                    groupByValue,
                    out aggregators))
                {
                    aggregators = new List<IAggregator>();

                    foreach (var aggregateFactory in listAggregators)
                    {
                        var aggregator = aggregateFactory();
                        aggregators.Add(aggregator);
                    }

                    aggregatedValues[groupByValue] = aggregators;
                }

                var n = 0;
                // Checks if the Aggregators have been created
                foreach (var aggregateColumn in listAggregateColumns)
                {
                    // Add the value to the result
                    aggregators[n].Add(
                        element.getOrDefault(aggregateColumn));

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
                foreach (var aggregateColumn in listAggregatedColumns)
                {
                    element.set(aggregateColumn, pair.Value[n].Result);
                    n++;
                }

                add(element);
            }
        }
    }
}
