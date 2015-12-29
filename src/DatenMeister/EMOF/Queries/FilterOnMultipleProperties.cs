using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Proxy;

namespace DatenMeister.EMOF.Queries
{
    public class FilterOnMultipleProperties : ProxyReflectiveCollection
    {
        private readonly StringComparison _comparison;
        private readonly IEnumerable<object> _properties;
        private readonly string _searchString;

        public FilterOnMultipleProperties(
            IReflectiveCollection collection,
            IEnumerable<object> properties,
            string searchString,
            StringComparison comparison = StringComparison.CurrentCulture)
            : base(collection)
        {
            _properties = properties;
            _searchString = searchString;
            _comparison = comparison;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            var properties = _properties.ToList();
            foreach (var value in Collection)
            {
                var valueAsObject = value as IObject;
                foreach (var property in properties)
                {
                    if ((valueAsObject?.isSet(property) == true) &&
                        (valueAsObject
                            ?.get(property)
                            ?.ToString()
                            ?.IndexOf(_searchString, _comparison) >= 0))
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
                var valueAsObject = value as IObject;
                foreach (var property in _properties)
                {
                    if ((valueAsObject?.isSet(property) == true) &&
                        (valueAsObject.get(property)?.ToString()?.Contains(_searchString) == true))
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
