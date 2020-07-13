using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Runtime.Extents.Configuration
{
    /// <summary>
    /// Stores the configurations for the extents themselves.
    /// Does not store the configuration of a specific extent
    /// </summary>
    public class ExtentSettings
    {
        public string name = "Extent Settings";
        
        /// <summary>
        /// Stores the extent type setting
        /// </summary>
        public List<ExtentTypeSetting> extentTypeSettings { get; } = new List<ExtentTypeSetting>();

        public ExtentTypeSetting? GetExtentTypeSetting(string extentType)
        {
            return extentTypeSettings.FirstOrDefault(x => x.name == extentType);
        }
    }
}