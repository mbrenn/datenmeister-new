using System;
using System.Collections.Generic;
using DatenMeister.Models.ManagementProvider.FormViewModels;
using DatenMeister.Models.Runtime;
using DatenMeister.Runtime.ExtentStorage;

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
            typeof(ExtentLoadingState),
            typeof(Extent),
            typeof(Workspace),
            typeof(CreateNewWorkspaceModel),
            typeof(ExtentTypeSetting),
            typeof(ExtentProperties),
            typeof(ExtentPropertyDefinition),
            typeof(ExtentSettings)
        };
    }
}