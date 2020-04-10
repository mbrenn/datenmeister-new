using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.FastViewFilter
{
    /// <summary>
    /// Gets the interface for the fast filter
    /// </summary>
    public interface IFastFilter
    {
        /// <summary>
        /// Gets whether the element shall be filtered, that means whether the item shall be shown
        /// </summary>
        /// <param name="value">Value to be filtered</param>
        /// <returns>true, if the value shall be passing the filter</returns>
        bool IsFiltered(object value);
    }
}