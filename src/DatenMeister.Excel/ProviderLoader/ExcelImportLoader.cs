
using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelImportLoaderConfig))]
    public class ExcelImportLoader : IProviderLoader
    {
        private readonly ExtentManager _extentManager;

        public ExcelImportLoader(ExtentManager extentManager)
        {
            _extentManager = extentManager;
        }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            if (!(configuration is ExcelImportLoaderConfig settings))
            {
                throw new InvalidOperationException(
                    $"Given configuration is not of type {typeof(ExcelImportLoaderConfig)}, is of {configuration.GetType().FullName}");
            }

            // Creates the XMI being used as a target
            var xmiConfiguration = new XmiStorageLoaderConfig(settings.extentUri)
            {
                filePath = settings.extentPath,
                workspaceId = settings.workspaceId
            };

            var extent = (MofExtent) _extentManager.LoadExtent(xmiConfiguration, extentCreationFlags);
            extent.elements().RemoveAll();

            // Loads the excelinformation into the extent
            ExcelReferenceLoader.ImportExcelIntoExtent(extent, settings);

            // Returns the values
            return new LoadedProviderInfo(extent.Provider, xmiConfiguration)
                {IsExtentAlreadyAddedToWorkspace = true};
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            throw new NotImplementedException();
        }
    }
}