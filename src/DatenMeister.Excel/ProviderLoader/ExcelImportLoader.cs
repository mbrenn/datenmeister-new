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

namespace DatenMeister.Excel.ProviderLoader;

public class ExcelImportLoader : IProviderLoader
{
    public IWorkspaceLogic? WorkspaceLogic { get; set; }
        
    public IScopeStorage? ScopeStorage { get; set; }

    public async Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        configuration = ObjectCopier.CopyForTemporary(configuration) as IElement
                        ?? throw new InvalidOperationException("Element is not of type IElement");
            
        var extentManager = new ExtentManager(
            WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"),
            ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));

        var extentPath =
            configuration.getOrDefault<string>(
                _ExtentLoaderConfigs._ExcelImportLoaderConfig
                    .extentPath) ?? throw new InvalidOperationException("extentPath == null");
            
        // Creates the XMI being used as a target
        var factory = new MofFactory(configuration);
        var xmiConfiguration = factory.create(_ExtentLoaderConfigs.TheOne.__XmiStorageLoaderConfig);
        xmiConfiguration.set(
            _ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
            extentPath);
        xmiConfiguration.set(
            _ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
            configuration.getOrDefault<string>(_ExtentLoaderConfigs._ExcelImportLoaderConfig.extentUri));
        xmiConfiguration.set(
            _ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
            configuration.getOrDefault<string>(_ExtentLoaderConfigs._ExcelImportLoaderConfig.workspaceId));

        var loadedInfo = await extentManager.LoadExtent(xmiConfiguration, extentCreationFlags);
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

    public Task StoreProvider(IProvider extent, IElement configuration)
    {
        throw new NotImplementedException();
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new()
    {
        IsPersistant = true,
        AreChangesPersistant = false
    };
}