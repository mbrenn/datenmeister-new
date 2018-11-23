using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.FastViewFilter;

namespace DatenMeister.Modules.FastFilter
{
    /// <summary>
    /// Performs the conversion of the fast filter instances to the filter themselves
    /// </summary>
    public class FastFilterConverter
    {
        /// <summary>
        /// Converts the given element to a fast filter. 
        /// </summary>
        /// <param name="element">Element to be converted</param>
        /// <returns>The returned instance or </returns>
        public IFastFilter Convert(IObject element)
        {
            if (element.Equals(_FastViewFilters.TheOne.__PropertyContainsFilter))
            {
                return new PropertyComparison(element);
            }

            if (element.Equals(_FastViewFilters.TheOne.PropertyContainsFilter))
            {
                return new PropertyContains(element);
            }

            return null;
        }
    }
}