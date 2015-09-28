using DatenMeister.EMOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.EMOF.Queries
{
    /// <summary>
    /// Defines a static helper class, which eases
    /// the access to the filters.
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
                (x) => ((string)x)?.StartsWith(value) == true);
        }

        public static IReflectiveCollection WhenPropertyIs(
                        IReflectiveCollection collection,
            object property,
            string value)
        {
            return new FilterOnPropertyByPredicateCollection(
                collection,
                property,
                (x) => ((string)x) == value);
        }

        public static IReflectiveCollection WhenOneOfThePropertyContains(
            IReflectiveCollection collection,
            object[] properties,
            string value)
        {
            return new FilterOnMultipleProperties(
                collection,
                properties,
                value);
        }
    }
}
