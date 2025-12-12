using DatenMeister.Core.Interfaces.MOF.Reflection;
using static DatenMeister.Core.Models._FastViewFilters;

namespace DatenMeister.FastViewFilter;

public class PropertyComparison(IObject filterObject) : IFastFilter
{
    private readonly IObject _filterObject = filterObject;

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