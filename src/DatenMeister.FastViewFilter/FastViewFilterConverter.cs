using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.FastViewFilter;

/// <summary>
/// Performs the conversion of the fast filter instances to the filter themselves
/// </summary>
public class FastViewFilterConverter
{
    /// <summary>
    /// Converts the given element to a fast filter.
    /// </summary>
    /// <param name="element">Element to be converted</param>
    /// <returns>The returned instance or </returns>
    public static IFastFilter? Convert(IElement element)
    {
        if (element.getMetaClass()?.Equals(_FastViewFilters.TheOne.__PropertyComparisonFilter) == true)
        {
            return new PropertyComparison(element);
        }

        if (element.getMetaClass()?.Equals(_FastViewFilters.TheOne.__PropertyContainsFilter) == true)
        {
            return new PropertyContains(element);
        }

        return null;
    }
}