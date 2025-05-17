using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Provider.Xmi.Provider.XMI.ExtentStorage;

namespace DatenMeister.Provider.Xmi.Provider.XMI
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
    public class XmiPlugin : IDatenMeisterPlugin
    {
        private readonly ProviderToProviderLoaderMapper _storageMapper;

        public XmiPlugin(IScopeStorage scopeStorage)
        {
            _storageMapper = scopeStorage.Get<ProviderToProviderLoaderMapper>();
        }

        public Task Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    _storageMapper.AddMapping(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig,
                        _ => new XmiStorageProviderLoader());
                    break;
            }

            return Task.CompletedTask;
        }
    }
}