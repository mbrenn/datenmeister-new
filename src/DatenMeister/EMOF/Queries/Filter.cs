using DatenMeister.EMOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.EMOF.Queries
{
    public class Filter
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
    }
}
