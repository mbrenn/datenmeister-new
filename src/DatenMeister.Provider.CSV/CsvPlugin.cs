using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.ExtentManager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Provider.CSV.Runtime;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
    public class CsvPlugin : IDatenMeisterPlugin
    {
        private readonly ConfigurationToExtentStorageMapper _configurationToExtentStorageMapper;

        public CsvPlugin(IScopeStorage scopeStorage)
        {
            _configurationToExtentStorageMapper = scopeStorage.Get<ConfigurationToExtentStorageMapper>();
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    _configurationToExtentStorageMapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig,
                        manager => new CsvProviderLoader());
                    break;
            }
        }
    }
}