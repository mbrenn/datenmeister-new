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
        /// <summary>
        /// Defines the name of the extent settings
        /// </summary>
        public string name = "Extent Settings";
        
        /// <summary>
        /// Stores the extent type setting
        /// </summary>
        public List<ExtentTypeSetting> extentTypeSettings { get; } = new List<ExtentTypeSetting>();
        
        public List<ExtentPropertyDefinition> propertyDefinitions { get; } = new List<ExtentPropertyDefinition>();

        public ExtentTypeSetting? GetExtentTypeSetting(string extentType)
        {
            return extentTypeSettings.FirstOrDefault(x => x.name == extentType);
        }
    }
}