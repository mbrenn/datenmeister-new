using DatenMeister.EMOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DatenMeister.EMOF.Proxy;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Queries
{
    public class FilterOnPropertyByPredicateCollection : ProxyReflectiveCollection
    {
        /// <summary>
        /// Stores the property
        /// </summary>
        private object _property;

        /// <summary>
        /// Stores the filter to filter on the property
        /// </summary>
        private Predicate<object> _filter;

        public FilterOnPropertyByPredicateCollection(
            IReflectiveCollection collection, 
            object property, 
            Predicate<object> filter)
            : base(collection)
        {
            _property = property;
            _filter = filter;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in _collection)
            {
                var valueAsObject = value as IObject;
                if (_filter(valueAsObject?.get(_property)))
                {
                    yield return valueAsObject;
                }
            }
        }

        public override int size()
        {
            var result = 0;
            foreach (var value in _collection)
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
