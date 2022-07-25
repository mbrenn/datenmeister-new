using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Extent.Manager.Extents.Configuration
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
        /// Stores an enumeration of possible standard extentTypeSettings.
        /// This enumeration can be used to figure out which extent types are supported by the
        /// current instance of DatenMeister. 
        /// </summary>
        public List<ExtentType> extentTypeSettings { get; } = new();
        
        public List<ExtentPropertyDefinition> propertyDefinitions { get; } = new();

        public ExtentType? GetExtentTypeSetting(string extentType)
        {
            return extentTypeSettings.FirstOrDefault(x => x.name == extentType);
        }
    }
}