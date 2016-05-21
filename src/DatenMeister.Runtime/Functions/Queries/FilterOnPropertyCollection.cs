using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnPropertyCollection : ProxyReflectiveCollection
    {
        private readonly object _filterValue;
        private readonly object _property;

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
