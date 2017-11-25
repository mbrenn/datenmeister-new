using System;
using System.Collections.Generic;

namespace DatenMeister.Provider.ManagementProviders.Model
{
    /// <summary>
    /// Class referring to all management provider models
    /// </summary>
    public static class ManagementProviderModel
    {
        /// <summary>
        /// Gets the enumeration of all types
        /// </summary>
        public static IEnumerable<Type> AllTypes => new[]
        {
            typeof(Extent),
            typeof(Workspace)
        };
    }
}