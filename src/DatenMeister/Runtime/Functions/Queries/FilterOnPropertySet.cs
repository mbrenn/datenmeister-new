using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    /// Performs a filtering on all properties
    /// </summary>
    public class FilterOnPropertySet : ProxyReflectiveCollection
    {
        private readonly string _property;

        public FilterOnPropertySet(IReflectiveCollection collection, string property) : base(collection)
        {
            _property = property;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var element in Collection.OfType<IObject>())
            {
                if (element.isSet(_property))
                {
                    yield return element;
                }
            }
        }

        public override int size()
        {
            return Collection.OfType<IObject>().Where(x => x.isSet(_property));
        }
    }
}