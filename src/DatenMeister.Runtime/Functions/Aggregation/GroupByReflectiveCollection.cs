 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            object groupByColumn,
            object aggregateColumn,
            IAggregator<T> aggregator)
            : base(new MofReflectiveSequence())
        {
            Aggregate(collectionToBeAggregated,
                groupByColumn,
                aggregateColumn,
                aggregator);
        }

        private void Aggregate(
            IReflectiveCollection collectionToBeAggregated, 
            object groupByColumn, 
            object aggregateColumn,
            IAggregator<T> aggregator)
        {
            Dictionary<object, T> aggregatedValues = 
                new Dictionary<object, T>();
            foreach (var element in collectionToBeAggregated.Cast<IObject>())
            {
                if (!element.isSet(groupByColumn)
                    || !element.isSet(aggregateColumn))
                {
                    // Skip the elements which do not have both columns
                    continue;
                }

                T value;
            }
        }
    }
}
