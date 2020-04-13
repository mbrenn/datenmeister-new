using System.Collections.Generic;

namespace DatenMeister.Runtime.Extents.Configuration
{
    /// <summary>
    /// Stores the configurations for the extents themselves.
    /// Does not store the configuration of a specific extent
    /// </summary>
    public class ExtentSettings
    {
        /// <summary>
        /// Stores the extent type setting
        /// </summary>
        public List<ExtentTypeSetting> extentTypeSettings { get; } = new List<ExtentTypeSetting>();
    }
}