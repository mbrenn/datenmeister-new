using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Functions.Aggregation;
using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    ///     Defines a static helper class, which eases
    ///     the access to the filters.
    /// </summary>
    public static class Filter
    {
        public static IReflectiveCollection WhenPropertyStartsWith(
            this IReflectiveCollection collection,
            string property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => ((string) x)?.StartsWith(value) == true);
        }

        public static IReflectiveCollection WhenMetaClassIs(
            this IReflectiveCollection collection,
            IElement metaClass)
        {
            return new FilterOnMetaClass(collection, metaClass);
        }

        public static IReflectiveCollection WhenPropertyIs(
            this IReflectiveCollection collection,
            string property,
            object value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => x?.Equals(value) == true);
        }

        public static IReflectiveCollection WhenPropertyIsOneOf(
            this IReflectiveCollection collection,
            string property,
            IEnumerable<object> values)
        {
            var valuesAsList = values.ToList();
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => valuesAsList.Any(y => x?.Equals(y) == true));
        }

        public static IReflectiveCollection WhenOneOfThePropertyContains(
            this IReflectiveCollection collection,
            IEnumerable<string> properties,
            string value,
            StringComparison comparer = StringComparison.CurrentCulture)
        {
            return new FilterOnMultipleProperties(
                collection,
                properties,
                value,
                comparer);
        }

        public static IReflectiveCollection GetAllDescendants(
            this IReflectiveCollection collection)
        {
            return new InMemoryReflectiveSequence(null, null, AllDescendentsQuery.GetDescendents(collection).Cast<object>().ToList());
        }

        public static IReflectiveCollection GroupProperties(
            this IReflectiveCollection collection,
            string groupByColumn,
            string aggregateColumn,
            Func<IAggregator> aggregatorFunc, 
            string aggregatedColumn)
        {
            return  new GroupByReflectiveCollection(
                collection,
                groupByColumn,
                aggregateColumn,
                aggregatorFunc, 
                aggregatedColumn);
        }

        /// <summary>
        /// Orders the reflective section by the given property
        /// </summary>
        /// <param name="collection">Collection to be ordered</param>
        /// <param name="property">Property being used as key</param>
        /// <returns>Ordered reflective collection</returns>
        public static IReflectiveCollection OrderBy(
            this IReflectiveCollection collection,
            string property)
        {
            return new OrderByProperty(collection, property);
        }
    }
}