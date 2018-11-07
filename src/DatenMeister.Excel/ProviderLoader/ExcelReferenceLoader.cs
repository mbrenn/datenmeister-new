using System;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelReferenceSettings))]
    public class ExcelReferenceLoader : IProviderLoader
    {
        private readonly IDatenMeisterScope _scope;

        public ExcelReferenceLoader(IDatenMeisterScope scope)
        {
            _scope = scope;
        }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, bool createAlsoEmpty)
        {
            if (!(configuration is ExcelReferenceSettings excelReferenceSettings))
            {
                throw new InvalidOperationException("Given configuration is not of type ExcelReferenceSettings");
            }

            // Now load the stuff
            return new LoadedProviderInfo(ExcelImporter.GetProviderForExcelAsReference(excelReferenceSettings));
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}