
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelImportSettings))]
    public class ExcelImportLoader : IProviderLoader
    {
        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, bool createAlsoEmpty)
        {
            throw new System.NotImplementedException();
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}