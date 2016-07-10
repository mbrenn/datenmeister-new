using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnPropertyByPredicateCollection : ProxyReflectiveCollection
    {
        /// <summary>
        ///     Stores the filter to filter on the property
        /// </summary>
        private readonly Predicate<object> _filter;

        /// <summary>
        ///     Stores the property
        /// </summary>
        private readonly string _property;

        public FilterOnPropertyByPredicateCollection(
            IReflectiveCollection collection,
            string property,
            Predicate<object> filter)
            : base(collection)
        {
            _property = property;
            _filter = filter;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                var valueAsObject = value as IObject;
                var property = valueAsObject?.get(_property);
                if (_filter(property))
                {
                    yield return valueAsObject;
                }
            }
        }

        public override int size()
        {
            var result = 0;
            foreach (var value in Collection)
            {
                var valueAsObject = value as IObject;
                if (_filter(valueAsObject?.get(_property)))
                {
                    result++;
                }
            }

            return result;
        }
    }
}
