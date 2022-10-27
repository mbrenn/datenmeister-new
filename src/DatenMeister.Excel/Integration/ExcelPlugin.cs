using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Excel.ProviderLoader;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Excel.Integration
{
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
    public class ExcelPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public ExcelPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    var mapper = _scopeStorage.Get<ProviderToProviderLoaderMapper>();
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelExtentLoaderConfig,
                        manager => new ExcelFileProviderLoader());
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelReferenceLoaderConfig,
                        manager => new ExcelReferenceLoader());
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelImportLoaderConfig,
                        manager => new ExcelImportLoader());
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelHierarchicalLoaderConfig,
                        manager => new ExcelHierarchicalLoader());
                    break;
            }
        }
    }
}