using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Excel.ProviderLoader;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Excel.Integration;

[PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
public class ExcelPlugin : IDatenMeisterPlugin
{
    private readonly IScopeStorage _scopeStorage;

    public ExcelPlugin(IScopeStorage scopeStorage)
    {
        _scopeStorage = scopeStorage;
    }
        
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterBootstrapping:
                var mapper = _scopeStorage.Get<ProviderToProviderLoaderMapper>();
                mapper.AddMapping(
                    _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelExtentLoaderConfig,
                    _ => new ExcelFileProviderLoader());
                mapper.AddMapping(
                    _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelReferenceLoaderConfig,
                    _ => new ExcelReferenceLoader());
                mapper.AddMapping(
                    _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelImportLoaderConfig,
                    _ => new ExcelImportLoader());
                mapper.AddMapping(
                    _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelHierarchicalLoaderConfig,
                    _ => new ExcelHierarchicalLoader());
                break;
        }

        return Task.CompletedTask;
    }
}