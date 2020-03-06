#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public static class Extensions
    {
        /// <summary>
        /// Aggregates the list by using the aggregator
        /// </summary>
        /// <typeparam name="T">Type of the enumeration and
        /// the aggregation</typeparam>
        /// <param name="aggregator">Aggregator function to be used</param>
        /// <param name="items">Items to be aggregated</param>
        /// <returns>The total result</returns>
        public static T Aggregate<T>(
            this IAggregator aggregator,
            IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    aggregator.Add(item);
                }
            }

            return (T) aggregator.Result;
        }

        /// <summary>
        /// Aggregates the list by using the aggregator
        /// </summary>
        /// <typeparam name="T">Type of the enumeration and
        /// the aggregation</typeparam>
        /// <param name="items">Items to be aggregated</param>
        /// <param name="aggregator">Aggregator function to be used</param>
        /// <returns>The total result</returns>
        public static T Aggregate<T>(
            this IReflectiveCollection items,
            IAggregator aggregator)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    aggregator.Add((T) item);
                }
            }

            return (T) aggregator.Result;
        }

        /// <summary>
        /// Aggregates the list by using the aggregator
        /// </summary>
        /// <typeparam name="T">Type of the enumeration and
        /// the aggregation</typeparam>
        /// <param name="items">Items to be aggregated</param>
        /// <param name="property">Property to be used that retrieve
        /// the values</param>
        /// <param name="aggregator">Aggregator function to be used</param>
        /// <returns>The total result</returns>
        public static T Aggregate<T>(
            this IReflectiveCollection items,
            IAggregator aggregator,
            string property)
        {
            foreach (var item in items
                .Cast<IObject>()
                .Where(item => item.isSet(property)))
            {
                aggregator.Add(
                    (T) Convert.ChangeType(item.get(property), typeof(T)));
            }

            return (T) aggregator.Result;
        }
    }
}