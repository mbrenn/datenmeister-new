using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Defines a container class to store all configurations being loaded. 
    /// This is the file that is persisted to the filesystem and is used to reload the formerly loaded extents
    /// </summary>
    public class ExtentStorageConfigurationCollection
    {
        public List<ExtentStorageConfiguration> Extents { get; } = new List<ExtentStorageConfiguration>();
    }
}