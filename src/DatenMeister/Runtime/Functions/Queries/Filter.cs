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
        /// <summary>
        /// Gets the elements out of the reflective collection in which a certain proprety starts
        /// with a the given value.
        /// </summary>
        /// <param name="collection">Collection to be queried</param>
        /// <param name="property">Property to be set</param>
        /// <param name="value">The property needs to start with the given value</param>
        /// <returns></returns>
        public static IReflectiveCollection WhenPropertyStartsWith(
            this IReflectiveCollection collection,
            string property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection<string>(
                collection,
                property,
                x => x?.StartsWith(value) == true);
        }
        

        public static IReflectiveCollection WhenElementIsObject(
            this IReflectiveCollection collection)
            =>
                new FilterOnElementType<IObject>(collection);

        public static IReflectiveCollection WhenMetaClassIs(
            this IReflectiveCollection collection,
            IElement? metaClass)
            =>
                new FilterOnMetaClass(collection, metaClass);

        public static IReflectiveCollection WhenMetaClassIsOneOf(
            this IReflectiveCollection collection,
            params IElement[] metaClasses)
            =>
                new FilterOnMetaClass(collection, metaClasses);

        public static IReflectiveCollection WhenMetaClassIsNotSet(
            this IReflectiveCollection collection)
            =>
                new FilterOnMetaClassIsNotSet(collection);

        public static IReflectiveCollection WhenPropertyIsSet(
            this IReflectiveCollection collection,
            string propertyName)
            =>
                new FilterOnPropertyIsSet(collection, propertyName);

        public static IReflectiveCollection WhenPropertyHasValue<T>(
            this IReflectiveCollection collection,
            string property,
            T value)
        {
            return new FilterOnPropertyByPredicateCollection<T>(
                collection,
                property,
                x => x?.Equals(value) == true);
        }

        public static IReflectiveCollection WhenPropertyIsOneOf<T>(
            this IReflectiveCollection collection,
            string property,
            IEnumerable<T> values)
        {
            var valuesAsList = values.ToList();
            return new FilterOnPropertyByPredicateCollection<T>(
                collection,
                property,
                x => valuesAsList.Any(y => x?.Equals(y) == true));
        }

        public static IReflectiveCollection WhenOneOfThePropertyContains(
            this IReflectiveCollection collection,
            IEnumerable<string> properties,
            string value,
            StringComparison comparer = StringComparison.CurrentCulture)
            =>
                new FilterOnMultipleProperties(
                    collection,
                    properties,
                    value,
                    comparer);


        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="filter">Filter to be added</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence WhenFiltered(
            this IReflectiveCollection collection,
            Func<object?, bool> filter)
            =>
                new TemporaryReflectiveSequence(collection.Where(filter).ToList());

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllDescendants(
            this IReflectiveCollection collection)
            =>
                new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection).Cast<object>().ToList());

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetCompositeDescendents(
            this IReflectiveCollection collection)
            =>
                new TemporaryReflectiveSequence(AllDescendentsQuery.GetCompositeDescendents(collection).Cast<object>().ToList());

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively.
        /// The elements of the collection themselves will also be returned
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllDescendantsIncludingThemselves(
            this IReflectiveCollection collection)
        {
            return new TemporaryReflectiveSequence(
                collection.AsEnumerable().Union(
                    AllDescendentsQuery.GetDescendents(collection).Cast<object>().ToList()));
        }

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively.
        /// The elements of the collection themselves will also be returned
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllCompositesIncludingThemselves(
            this IReflectiveCollection collection)
        {
            return new TemporaryReflectiveSequence(
                AllDescendentsQuery.GetCompositeDescendents(collection).Cast<object>().ToList());
        }

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="byFollowingProperties">Properties that shall be followed. This prevents the following of properties</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllDescendants(
            this IReflectiveCollection collection,
            IEnumerable<string> byFollowingProperties)
            =>
                new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection, byFollowingProperties).Cast<object>().ToList());

        /// <summary>
        /// Gets all descendents of a reflective collection by opening all properties recursively
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="byFollowingProperty">Property that shall be followed. This prevents the following of properties</param>
        /// <returns>A reflective collection, containing all items</returns>
        public static IReflectiveSequence GetAllDescendants(
            this IReflectiveCollection collection,
            string byFollowingProperty)
            =>
                new TemporaryReflectiveSequence(AllDescendentsQuery
                    .GetDescendents(collection, new[] {byFollowingProperty}).Cast<object>().ToList());

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
            =>
                new GroupByReflectiveCollection(
                    collection,
                    groupByColumn,
                    aggregateColumn,
                    aggregatorFunc,
                    aggregatedColumn);

        /// <summary>
        /// Orders the reflective section by the given property
        /// </summary>
        /// <param name="collection">Collection to be ordered</param>
        /// <param name="property">Property being used as key</param>
        /// <returns>Ordered reflective collection</returns>
        public static IReflectiveCollection OrderBy(
            this IReflectiveCollection collection,
            string property)
            =>
                new OrderByProperty(collection, property);

        public static IReflectiveCollection FilterDistinct(
            this IReflectiveCollection collection,
            string property)
            =>
                new DistinctReflectiveCollection(collection, property);

        /// <summary>
        /// Unionizes two reflective collections into one collection
        /// </summary>
        /// <param name="first">First collection</param>
        /// <param name="second">Second collection</param>
        /// <returns>The resulting union</returns>
        public static IReflectiveCollection Union(
            this IReflectiveCollection first,
            IReflectiveCollection second)
            =>
                new UnionQuery(first, second);
    }
}