using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Defines a container class to store all configuration
    /// </summary>
    public class ExtentStorageConfigurationCollection
    {
        private List<ExtentStorageConfiguration> _configurations = new List<ExtentStorageConfiguration>();

        public List<ExtentStorageConfiguration> Configurations
        {
            get { return _configurations; }
            set { _configurations = value; }
        }
    }
}