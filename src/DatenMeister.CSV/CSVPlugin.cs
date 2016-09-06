using DatenMeister.Core.Plugins;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.CSV
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
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