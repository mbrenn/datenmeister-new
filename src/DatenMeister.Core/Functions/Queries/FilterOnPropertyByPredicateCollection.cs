using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnPropertyByPredicateCollection<T> : ProxyReflectiveCollection
    {
        /// <summary>
        ///     Stores the filter to filter on the property
        /// </summary>
        private readonly Predicate<T> _filter;

        /// <summary>
        ///     Stores the property
        /// </summary>
        private readonly string _property;

        public FilterOnPropertyByPredicateCollection(
            IReflectiveCollection collection,
            string property,
            Predicate<T> filter)
            : base(collection)
        {
            _property = property;
            _filter = filter;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                if (value is IObject valueAsObject && valueAsObject.isSet(_property))
                {
                    var property = valueAsObject.get<T>(_property);
                    if (_filter(property))
                    {
                        yield return valueAsObject;
                    }
                }
            }
        }

        public override int size()
        {
            var result = 0;
            foreach (var value in Collection)
            {
                if (value is IObject valueAsObject && valueAsObject.isSet(_property))
                {
                    var property = valueAsObject.get<T>(_property);
                    if (_filter(property))
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
