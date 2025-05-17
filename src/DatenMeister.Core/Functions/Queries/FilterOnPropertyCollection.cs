using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries
{
    public class FilterOnPropertyCollection : ProxyReflectiveCollection
    {
        private readonly object _filterValue;
        private readonly string _property;

        public FilterOnPropertyCollection(
            IReflectiveSequence collection,
            string property,
            object filterValue)
            : base(collection)
        {
            _property = property;
            _filterValue = filterValue;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var value in Collection)
            {
                var valueAsObject = value as IObject;
                if (valueAsObject?.get(_property)?.Equals(_filterValue) == true)
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
                var valueAsObject = value as IObject;
                if (valueAsObject?.get(_property)?.Equals(_filterValue) == true)
                {
                    result++;
                }
            }

            return result;
        }
    }
}
