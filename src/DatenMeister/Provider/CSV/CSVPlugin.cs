using DatenMeister.Core.Plugins;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class CSVPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentConfigurationLoader _extentLoader;

        public CSVPlugin(ExtentConfigurationLoader extentLoader)
        {
            _extentLoader = extentLoader;
        }

        public void Start()
        {
            _extentLoader.AddAdditionalType(typeof(CSVExtentLoaderConfig));
        }
    }
}