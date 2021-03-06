﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.FastViewFilter
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
        public bool IsFiltered(object? value)
        {
            if (value is IObject valueAsObject)
            {
                var filterValue = _fastFilter.getOrDefault<string>(_FastViewFilters._PropertyContainsFilter.Value);
                var propertyName = _fastFilter.getOrDefault<string>(_FastViewFilters._PropertyContainsFilter.Property);

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
}