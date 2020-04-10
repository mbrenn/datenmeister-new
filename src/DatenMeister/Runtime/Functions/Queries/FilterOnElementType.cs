using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnElementType<T> : ProxyReflectiveCollection
    {
        public FilterOnElementType(IReflectiveCollection collection)
            : base(collection)
        {
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                if (value is T)
                {
                    yield return value;
                }
            }
        }

        public override int size()
        {
            var result = 0;
            foreach (var value in Collection)
            {
                if (value is T) 
                {
                    result++;
                }
            }

            return result;
        }
    }
}