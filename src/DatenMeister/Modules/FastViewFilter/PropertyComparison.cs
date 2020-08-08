using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.FastViewFilter
{
    public class PropertyComparison : IFastFilter
    {
        private readonly IObject _filterObject;

        public PropertyComparison(IObject filterObject)
        {
            _filterObject = filterObject;
        }

        public bool IsFiltered(object? value)
        {
            if (value is IObject valueAsObject)
            {
                var filterObject = DotNetConverter.ConvertToDotNetObject<PropertyComparisonFilter>(_filterObject);
                if (filterObject.Property == null)
                {
                    // To avoid exceptions
                    return true;
                }

                var propertyValue = valueAsObject.getOrDefault<string>(filterObject.Property);

                return filterObject.ComparisonType switch
                {
                    ComparisonType.Equal => propertyValue == filterObject.Value,
                    ComparisonType.GreaterThan => string.CompareOrdinal(propertyValue, filterObject.Value) > 0,
                    ComparisonType.LighterThan => string.CompareOrdinal(propertyValue, filterObject.Value) < 0,
                    ComparisonType.GreaterOrEqualThan => string.CompareOrdinal(propertyValue, filterObject.Value) >= 0,
                    ComparisonType.LighterOrEqualThan => string.CompareOrdinal(propertyValue, filterObject.Value) <= 0,
                    _ => true
                };
            }

            return false;
        }
    }
}