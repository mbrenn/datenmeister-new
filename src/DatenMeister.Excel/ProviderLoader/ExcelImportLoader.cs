using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Excel.ProviderLoader
{
    public class ExcelImportLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        
        public IScopeStorage? ScopeStorage { get; set; }

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            configuration = ObjectCopier.CopyForTemporary(configuration) as IElement
                            ?? throw new InvalidOperationException("Element is not of type IElement");
            
            var extentManager = new ExtentManager(
                WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"),
                ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));

            // Creates the XMI being used as a target
            var factory = new MofFactory(configuration);
            var xmiConfiguration = factory.create(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
            xmiConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentPath));
            xmiConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentUri));
            xmiConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.workspaceId));

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