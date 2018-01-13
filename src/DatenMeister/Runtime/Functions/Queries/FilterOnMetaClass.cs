using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class FilterOnMetaClass : ProxyReflectiveCollection
    {
        private readonly IElement[] _filteredMetaClass;

        public FilterOnMetaClass(IReflectiveCollection collection, IElement filteredMetaClass)
            : base(collection)
        {
            _filteredMetaClass = new[] {filteredMetaClass};
        }

        public FilterOnMetaClass(IReflectiveCollection collection, IElement[] filteredMetaClass)
            : base(collection)
        {
            _filteredMetaClass = filteredMetaClass;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                var valueAsObject = value as IElement;

                if (IsInList(valueAsObject))
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
                if (IsInList(valueAsObject))
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Verifies whether the element shall be given in the list
        /// </summary>
        /// <param name="valueAsObject">Value to be shown</param>
        /// <returns>true, if value is in</returns>
        private bool IsInList(IElement valueAsObject)
        {
            var isIn = false;
            var metaClass = valueAsObject?.getMetaClass();
            if (metaClass == null && _filteredMetaClass == null)
            {
                isIn = true;
            }
            else if (metaClass != null && _filteredMetaClass.Contains(metaClass))
            {
                isIn = true;
            }
            return isIn;
        }
    }
}