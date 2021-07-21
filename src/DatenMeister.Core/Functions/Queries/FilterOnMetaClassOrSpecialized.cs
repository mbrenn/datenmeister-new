using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.Functions.Queries
{
    public class FilterOnMetaClassOrSpecialized : ProxyReflectiveCollection
    {
        private readonly IElement[]? _filteredMetaClass;

        public FilterOnMetaClassOrSpecialized(IReflectiveCollection collection, IElement? filteredMetaClass)
            : base(collection)
        {
            _filteredMetaClass = filteredMetaClass == null ? null : new[] {filteredMetaClass};
        }

        public FilterOnMetaClassOrSpecialized(IReflectiveCollection collection, IElement[] filteredMetaClass)
            : base(collection)
        {
            _filteredMetaClass = filteredMetaClass;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                if (value is IElement valueAsObject && IsInList(valueAsObject))
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
                if (valueAsObject == null) 
                    continue;
                
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
            else
            {
                
                if (metaClass != null 
                    && _filteredMetaClass
                        ?.Any(x => ClassifierMethods.IsSpecializedClassifierOf(metaClass, x))
                    == true)
                {
                    isIn = true;
                }
            }

            return isIn;
        }
    }
}