using System.Collections.Generic;

namespace DatenMeister.Runtime.ExtentStorage.Configuration
{
    /// <summary>
    /// Defines a container class to store all configurations being loaded. 
    /// This is the file that is persisted to the filesystem and is used to reload the formerly loaded extents
    /// </summary>  
    public class ExtentLoaderConfigData
    {
        public List<ExtentLoaderConfig> Extents { get; } = new List<ExtentLoaderConfig>();
    }
}