using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
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

        public static IReflectiveCollection GetAllDecendants(
            this IReflectiveCollection collection)
        {
            return new MofReflectiveSequence(AllDescendentsQuery.getDescendents(collection).Cast<object>().ToList());
        }

        public static IReflectiveCollection GroupBy<T>(
            this IReflectiveCollection collection,
            string groupByColumn,
            string aggregateColumn,
            Func<IAggregator<T>> aggregatorFunc)
        {
            return  new GroupByReflectiveCollection<T>(
                collection,
                groupByColumn,
                aggregateColumn,
                aggregatorFunc);
        }
    }
}