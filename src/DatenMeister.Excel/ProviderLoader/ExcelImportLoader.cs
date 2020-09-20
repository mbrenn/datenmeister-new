
using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Models;
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

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var extentManager = new ExtentManager(
                WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"),
                ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));
            

            // Creates the XMI being used as a target
            var factory = new MofFactory(configuration);
            var xmiConfiguration = factory.create(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
            xmiConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                configuration.get(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.filePath));
            xmiConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
                configuration.get(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.workspaceId));

            var loadedInfo = extentManager.LoadExtent(xmiConfiguration, extentCreationFlags);
            if (loadedInfo.LoadingState == ExtentLoadingState.Failed || loadedInfo.Extent == null)
            {
                throw new InvalidOperationException("Loading of the extent has failed");
            }

            var extent = loadedInfo.Extent as MofExtent ?? throw new InvalidOperationException("Not a MofExtent");
            extent.elements().RemoveAll();

            // Loads the excelinformation into the extent
            ExcelReferenceLoader.ImportExcelIntoExtent(extent, configuration);

            // Returns the values
            return new LoadedProviderInfo(extent.Provider, xmiConfiguration)
                {IsExtentAlreadyAddedToWorkspace = true};
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            throw new NotImplementedException();
        }
    }
}