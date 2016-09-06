using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnMetaClass : ProxyReflectiveCollection
    {
        private readonly IElement _filteredMetaClass;

        public FilterOnMetaClass(IReflectiveCollection collection, IElement filteredMetaClass)
            : base(collection)
        {
            _filteredMetaClass = filteredMetaClass;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                var valueAsObject = value as IElement;
                if (Equals(valueAsObject?.getMetaClass(), _filteredMetaClass))
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
                var valueAsObject = value as IElement;
                if (Equals(valueAsObject?.getMetaClass(), _filteredMetaClass))
                {
                    result++;
                }
            }

            return result;
        }
    }
}