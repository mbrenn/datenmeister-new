using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Provider.CSV.Runtime;

namespace DatenMeister.Provider.CSV;

/// <summary>
/// This plugin is loaded during the bootup
/// </summary>
// ReSharper disable once UnusedMember.Global
// ReSharper disable once InconsistentNaming
[PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
public class CsvPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    private readonly ProviderToProviderLoaderMapper _providerToProviderLoaderMapper = scopeStorage.Get<ProviderToProviderLoaderMapper>();

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterBootstrapping:
                _providerToProviderLoaderMapper.AddMapping(
                    _ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig,
                    _ => new CsvProviderLoader());
                break;
        }

        return Task.CompletedTask;
    }
}