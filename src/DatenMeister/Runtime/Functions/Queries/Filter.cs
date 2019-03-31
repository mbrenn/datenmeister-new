using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
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

        public static IReflectiveCollection WhenMetaClassIsOneOf(
            this IReflectiveCollection collection,
            params IElement[] metaClasses)
        {
            return new FilterOnMetaClass(collection, metaClasses);
        }

        public static IReflectiveCollection WhenMetaClassIsNotSet(
            this IReflectiveCollection collection)
        {
            return new FilterOnMetaClassIsNotSet(collection);
        }

        public static IReflectiveCollection WhenPropertiyIsSet(
            this IReflectiveCollection collection,
            string property)
        {
            return new FilterOnPropertyIsSet(collection, property);
        }

        public static IReflectiveCollection WhenPropertyIsSet(
            this IReflectiveCollection collection,
            string propertyName)
        {
            return new FilterOnPropertyIsSet(collection, propertyName);
        }

        public static IReflectiveCollection WhenPropertyHasValue(
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

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllDescendants(
            this IReflectiveCollection collection)
        {
            return new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection).Cast<object>().ToList());
        }

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="byFollowingProperties">Columns that shall be followed. This prevents the following of properties</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllDescendants(
            this IReflectiveCollection collection,
            IEnumerable<string> byFollowingProperties)
        {
            return new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection, byFollowingProperties).Cast<object>().ToList());
        }

        /// <summary>
        /// Groups all properties by performing an aggregation
        /// </summary>
        /// <param name="collection">Collection to be aggregated</param>
        /// <param name="groupByColumn">The column which shall be used to identify same keys</param>
        /// <param name="aggregateColumn">The column which will be aggregated</param>
        /// <param name="aggregatorFunc">The function which creates an aggregator to combine the values</param>
        /// <param name="aggregatedColumn">The target column to which the property will be allocated</param>
        /// <returns></returns>
        public static IReflectiveCollection GroupProperties(
            this IReflectiveCollection collection,
            string groupByColumn,
            string aggregateColumn,
            Func<IAggregator> aggregatorFunc,
            string aggregatedColumn)
        {
            return new GroupByReflectiveCollection(
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

        public static IReflectiveCollection FilterDistinct(
            this IReflectiveCollection collection,
            string property)
        {
            return new DistinctReflectiveCollection(collection, property);
        }

        /// <summary>
        /// Unionizes two reflective collections into one collection
        /// </summary>
        /// <param name="first">First collection</param>
        /// <param name="second">Second collection</param>
        /// <returns>The resulting union</returns>
        public static IReflectiveCollection Union(
            this IReflectiveCollection first,
            IReflectiveCollection second)
        {
            return new UnionQuery(first, second);
        }
    }
}