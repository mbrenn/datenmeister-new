using System;
using System.Collections.Generic;
using DatenMeister.Models.ManagementProvider.FormViewModels;
using DatenMeister.Runtime.Extents.Configuration;

namespace DatenMeister.Models.ManagementProvider
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
            typeof(Workspace),
            typeof(CreateNewWorkspaceModel),
            typeof(ExtentTypeSetting),
            typeof(ExtentSettings)
        };
    }
}