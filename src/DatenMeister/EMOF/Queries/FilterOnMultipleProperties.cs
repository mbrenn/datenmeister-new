using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Proxy;

namespace DatenMeister.EMOF.Queries
{
    public class FilterOnMultipleProperties : ProxyReflectiveCollection
    {
        private readonly StringComparison _comparison;
        private readonly object[] _properties;
        private readonly string _searchString;

        public FilterOnMultipleProperties(
            IReflectiveCollection collection,
            object[] properties,
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
            foreach (var value in _collection)
            {
                var valueAsObject = value as IObject;
                foreach (var property in _properties)
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
            foreach (var value in _collection)
            {
                var valueAsObject = value as IObject;
                foreach (var property in _properties)
                {
                    if ((valueAsObject?.isSet(property) == true) &&
                        (valueAsObject?.get(property)?.ToString()?.Contains(_searchString) == true))
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}