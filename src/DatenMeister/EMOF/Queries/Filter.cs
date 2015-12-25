using System;
using System.Collections.Generic;
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
            IReflectiveCollection collection,
            object property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => ((string) x)?.StartsWith(value) == true);
        }

        public static IReflectiveCollection WhenPropertyIs(
            IReflectiveCollection collection,
            object property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                x => (string) x == value);
        }

        public static IReflectiveCollection WhenOneOfThePropertyContains(
            IReflectiveCollection collection,
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
    }
}