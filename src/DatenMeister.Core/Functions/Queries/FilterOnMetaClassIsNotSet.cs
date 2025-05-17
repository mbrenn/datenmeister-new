using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries
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