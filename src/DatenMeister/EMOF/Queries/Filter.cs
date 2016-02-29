using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.EMOF.Queries
{
    /// <summary>
    ///     Defines a static helper class, which eases
    ///     the access to the filters.
    /// </summary>
    public static class Filter
    {
        public static IReflectiveCollection WhenPropertyStartsWith(
            this IReflectiveCollection collection,
            object property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => ((string) x)?.StartsWith(value) == true);
        }

        public static IReflectiveCollection WhenPropertyIs(
            this IReflectiveCollection collection,
            object property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => (string) x == value);
        }

        public static IReflectiveCollection WhenOneOfThePropertyContains(
            this IReflectiveCollection collection,
            IEnumerable<object> properties,
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
    }
}