using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Provider.Excel.ProviderLoader;

namespace DatenMeister.Provider.Excel.Integration;

[PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
public class ExcelPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterBootstrapping:
                var mapper = scopeStorage.Get<ProviderToProviderLoaderMapper>();
                mapper.AddMapping(
                    _ExtentLoaderConfigs.TheOne.__ExcelExtentLoaderConfig,
                    _ => new ExcelFullSyncLoader());
                mapper.AddMapping(
                    _ExtentLoaderConfigs.TheOne.__ExcelReferenceLoaderConfig,
                    _ => new ExcelReadOnlyLoader());
                mapper.AddMapping(
                    _ExtentLoaderConfigs.TheOne.__ExcelImportLoaderConfig,
                    _ => new ExcelConvertToXmiOnceLoader());
                mapper.AddMapping(
                    _ExtentLoaderConfigs.TheOne.__ExcelHierarchicalLoaderConfig,
                    _ => new ExcelHierarchicalLoader());
                break;
        }

        return Task.CompletedTask;
    }
}