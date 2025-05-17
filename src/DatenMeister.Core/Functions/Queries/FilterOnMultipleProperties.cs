using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries
{
    public class FilterOnMultipleProperties : ProxyReflectiveCollection
    {
        private readonly StringComparison _comparison;
        private readonly IEnumerable<string> _properties;
        private readonly string _searchString;

        public FilterOnMultipleProperties(
            IReflectiveCollection collection,
            IEnumerable<string> properties,
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
            var properties = _properties.ToList();
            foreach (var value in Collection)
            {
                var valueAsObject = value as IObject;
                if (valueAsObject == null)
                {
                    continue;
                }

                foreach (var property in properties)
                {
                    if (valueAsObject.isSet(property) == false)
                        continue;

                    var propertyAsText = valueAsObject.get(property);
                    if (propertyAsText == null || !DotNetHelper.IsOfPrimitiveType(propertyAsText))
                        continue;

                    if (propertyAsText.ToString()?.IndexOf(_searchString, _comparison) >= 0)
                    {
                        yield return valueAsObject;
                        break;
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
                    if (valueAsObject?.isSet(property) == true &&
                        valueAsObject?.get(property)?.ToString()?.Contains(_searchString) == true)
                    {
                        result++;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
