using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using static DatenMeister.Models._DatenMeister._FastViewFilters;

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
                var property =
                    valueAsObject.getOrDefault<string>(_PropertyComparisonFilter
                        .Property);
                if (property == null)
                {
                    // To avoid exceptions
                    return true;
                }

                var propertyValue = valueAsObject.getOrDefault<string>(property);

                var comparisonType = valueAsObject.getOrDefault<___ComparisonType>(
                    _PropertyComparisonFilter.ComparisonType);
                var filterValue = valueAsObject.getOrDefault<string>(_PropertyComparisonFilter.Value);

                return comparisonType switch
                {
                    ___ComparisonType.Equal => propertyValue == filterValue,
                    ___ComparisonType.GreaterThan => string.CompareOrdinal(propertyValue, filterValue) > 0,
                    ___ComparisonType.LighterThan => string.CompareOrdinal(propertyValue, filterValue) < 0,
                    ___ComparisonType.GreaterOrEqualThan => string.CompareOrdinal(propertyValue, filterValue) >= 0,
                    ___ComparisonType.LighterOrEqualThan => string.CompareOrdinal(propertyValue, filterValue) <= 0,
                    _ => true
                };
            }

            return false;
        }
    }
}