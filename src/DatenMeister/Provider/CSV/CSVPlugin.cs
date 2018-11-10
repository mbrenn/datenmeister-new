using DatenMeister.Core.Plugins;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.BeforeBootstrapping)]
    public class CSVPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentConfigurationLoader _extentLoader;
        private readonly IConfigurationToExtentStorageMapper _configurationToExtentStorageMapper;

        public CSVPlugin(ExtentConfigurationLoader extentLoader, IConfigurationToExtentStorageMapper configurationToExtentStorageMapper)
        {
            _extentLoader = extentLoader;
            _configurationToExtentStorageMapper = configurationToExtentStorageMapper;
        }

        public void Start(PluginLoadingPosition position)
        {
            _extentLoader.AddAdditionalType(typeof(CSVExtentLoaderConfig));

            ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(_configurationToExtentStorageMapper, typeof(CsvProviderLoader));
        }
    }
}