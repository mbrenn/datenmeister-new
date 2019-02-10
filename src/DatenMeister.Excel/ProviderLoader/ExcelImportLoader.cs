
using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelImportSettings))]
    public class ExcelImportLoader : IProviderLoader
    {
        private readonly IExtentManager _extentManager;

        public ExcelImportLoader(IExtentManager extentManager)
        {
            _extentManager = extentManager;
        }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, bool createAlsoEmpty)
        {
            if (!(configuration is ExcelImportSettings settings))
            {
                throw new InvalidOperationException("Given configuration is not of type ExcelReferenceSettings");
            }

            // Creates the XMI being used as a target
            var xmiConfiguration = new XmiStorageConfiguration
            {
                extentUri = settings.extentUri,
                filePath = settings.extentPath,
                workspaceId = settings.workspaceId
            };

            var extent = (MofExtent) _extentManager.LoadExtent(xmiConfiguration, true);

            // Loads the excelinformation into the extent
            ExcelReferenceLoader.ImportExcelIntoExtent(extent, settings);

            // Returns the values
            return new LoadedProviderInfo(extent.Provider, xmiConfiguration)
                {IsExtentAlreadyAddedToWorkspace = true};
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}