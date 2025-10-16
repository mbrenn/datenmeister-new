using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.FastViewFilter;

public class PropertyContains(IObject fastFilter) : IFastFilter
{
    /// <summary>
    /// True, if the element shall be shown
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsFiltered(object? value)
    {
        if (value is IObject valueAsObject)
        {
            var filterValue = fastFilter.getOrDefault<string>(_FastViewFilters._PropertyContainsFilter.Value);
            var propertyName = fastFilter.getOrDefault<string>(_FastViewFilters._PropertyContainsFilter.Property);

            if (filterValue == null || propertyName == null)
            {
                return true;
            }

            var propertyValue = valueAsObject.getOrDefault<string>(propertyName);
            var result = propertyValue?.Contains(filterValue);
            return result == null || result == true;
        }

        return false;
    }
}