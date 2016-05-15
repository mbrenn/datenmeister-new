using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Plugins;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.CSV
{
    public class CSVPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentStorageConfigurationLoader _extentStorageLoader;

        public CSVPlugin(ExtentStorageConfigurationLoader extentStorageLoader)
        {
            _extentStorageLoader = extentStorageLoader;
        }

        public void Start()
        {
            _extentStorageLoader.AddAdditionalType(typeof(CSVStorageConfiguration));
        }
    }
}