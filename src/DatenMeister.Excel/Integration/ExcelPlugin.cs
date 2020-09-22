using DatenMeister.Excel.ProviderLoader;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Plugins;

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
                    var mapper = _scopeStorage.Get<ConfigurationToExtentStorageMapper>();
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelExtentLoaderConfig,
                        manager => new ExcelFileProviderLoader());
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelReferenceLoaderConfig,
                        manager => new ExcelReferenceLoader());
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelImportLoaderConfig,
                        manager => new ExcelImportLoader());
                    break;
            }
        }
    }
}