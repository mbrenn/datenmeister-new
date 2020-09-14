
using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelImportLoaderConfig))]
    public class ExcelImportLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            var extentManager = new ExtentManager(
                WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"),
                ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));
            
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

            var loadedInfo = extentManager.LoadExtent(xmiConfiguration, extentCreationFlags);
            if (loadedInfo.LoadingState == ExtentLoadingState.Failed || loadedInfo.Extent == null)
            {
                throw new InvalidOperationException("Loading of the extent has failed");
            }

            var extent = loadedInfo.Extent as MofExtent ?? throw new InvalidOperationException("Not a MofExtent");
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