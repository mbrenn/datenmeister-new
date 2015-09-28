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
        object[] properties;
        string searchString;

        public FilterOnMultipleProperties(IReflectiveCollection collection, object[] properties, string searchString)
            : base(collection)
        {
            this.properties = properties;
            this.searchString = searchString;
        }
        
        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in _collection)
            {
                var valueAsObject = value as IObject;
                foreach (var property in properties)
                {
                    if ((valueAsObject?.isSet(property) == true) &&
                        (valueAsObject?.get(property)?.ToString()?.Contains(searchString) == true))
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
                foreach (var property in properties)
                {
                    if ((valueAsObject?.isSet(property) == true) &&
                        (valueAsObject?.get(property)?.ToString()?.Contains(searchString) == true))
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
