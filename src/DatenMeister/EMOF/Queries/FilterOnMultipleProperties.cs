using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Queries
{
    public class FilterOnMultipleProperties : Proxy.ProxyReflectiveCollection
    {
        private StringComparison _comparison;
        object[] _properties;
        string _searchString;

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
            foreach (var value in Collection)
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
                        continue;
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
