using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.ExtentManager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Provider.XMI.ExtentStorage;

namespace DatenMeister.Provider.XMI
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
    public class XmiPlugin : IDatenMeisterPlugin
    {
        private readonly ConfigurationToExtentStorageMapper _storageMapper;

        public XmiPlugin(IScopeStorage scopeStorage)
        {
            _storageMapper = scopeStorage.Get<ConfigurationToExtentStorageMapper>();
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    _storageMapper.AddMapping(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig,
                        extentManager => new XmiProviderLoader());
                    break;
            }
        }
    }
}