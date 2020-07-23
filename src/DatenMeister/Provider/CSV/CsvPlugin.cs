using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.BeforeBootstrapping)]
    public class CsvPlugin : IDatenMeisterPlugin
    {
        private readonly ConfigurationToExtentStorageMapper _configurationToExtentStorageMapper;

        public CsvPlugin(ConfigurationToExtentStorageMapper configurationToExtentStorageMapper)
        {
            _configurationToExtentStorageMapper = configurationToExtentStorageMapper;
        }

        public void Start(PluginLoadingPosition position)
        {
            _configurationToExtentStorageMapper.MapExtentLoaderType(typeof(CsvProviderLoader));
        }
    }
}