using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;

namespace DatenMeister.Modules.FastViewFilter
{
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
            if (element.getMetaClass()?.Equals(_DatenMeister.TheOne.FastViewFilters.__PropertyComparisonFilter) == true)
            {
                return new PropertyComparison(element);
            }

            if (element.getMetaClass()?.Equals(_DatenMeister.TheOne.FastViewFilters.@__PropertyContainsFilter) == true)
            {
                return new PropertyContains(element);
            }

            return null;
        }
    }
}