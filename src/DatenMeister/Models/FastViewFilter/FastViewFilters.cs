using System;
using System.Collections.Generic;

namespace DatenMeister.Modules.FastViewFilter.Model
{
    public partial class FastViewFilters
    {
        /// <summary>
        /// Gets the types regarding fast view filters
        /// </summary>
        public static IEnumerable<Type> Types =>
            new[]
            {
                typeof(ComparisonType),
                typeof(PropertyComparisonFilter),
                typeof(PropertyContainsFilter)
            };
    }
}