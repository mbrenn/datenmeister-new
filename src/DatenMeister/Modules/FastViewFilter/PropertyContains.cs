using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.FastFilter
{
    public class PropertyContains : IFastFilter
    {
        private readonly IObject _fastFilter;

        public PropertyContains(IObject fastFilter)
        {
            _fastFilter = fastFilter;
        }

        /// <summary>
        /// True, if the element shall be shown
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsFiltered(IObject value)
        {
            var filterValue = DotNetHelper.AsString(_fastFilter.get(_FastViewFilters._PropertyContainsFilter.Value));
            if (filterValue == null)
            {
                return true;
            }

            var propertyName = DotNetHelper.AsString(_fastFilter.get(_FastViewFilters._PropertyContainsFilter.Property));

            var propertyValue = DotNetHelper.AsString(value.GetOrDefault(propertyName));
            var result = propertyValue?.Contains(filterValue);
            return result == null || result == true;
        }
    }
}