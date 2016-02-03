using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Defines a container class to store all configurations being loaded
    /// </summary>
    public class ExtentStorageConfigurationCollection
    {
        public List<ExtentStorageConfiguration> Extents { get; set; } = new List<ExtentStorageConfiguration>();
    }
}