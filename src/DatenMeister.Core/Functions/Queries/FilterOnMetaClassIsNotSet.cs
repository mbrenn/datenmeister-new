using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnMetaClassIsNotSet : ProxyReflectiveCollection
    {
        public FilterOnMetaClassIsNotSet(IReflectiveCollection collection) : base(collection)
        {
        }

        public override IEnumerator<object?> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                var valueAsObject = value as IElement;
                if (valueAsObject?.getMetaClass() == null)
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
                if (valueAsObject?.getMetaClass() == null)
                {
                    result++;
                }
            }

            return result;
        }
    }
}