using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public class ExtentStorageCollection
    {
        private List<ExtentStorageConfiguration> _configurations = new List<ExtentStorageConfiguration>();

        public List<ExtentStorageConfiguration> Configurations
        {
            get { return _configurations; }
            set { _configurations = value; }
        }
    }
}