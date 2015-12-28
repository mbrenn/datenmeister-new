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
    public class FilterOnPropertyCollection : ProxyReflectiveCollection
    {
        private object _property;

        private object _filterValue;

        public FilterOnPropertyCollection(
            IReflectiveSequence collection, 
            object property, 
            object filterValue)
            : base(collection)
        {
            _property = property;
            _filterValue = filterValue;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                var valueAsObject = value as IObject;
                if (valueAsObject?.get(_property)?.Equals(_filterValue) == true)
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
                if (valueAsObject?.get(_property)?.Equals(_filterValue) == true)
                {
                    result++;
                }
            }

            return result;
        }
    }
}
