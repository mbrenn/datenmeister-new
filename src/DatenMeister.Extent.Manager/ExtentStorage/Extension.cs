using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Extent.Manager.ExtentStorage;

public static class Extension
{
    /// <summary>
    /// Creates an xmi extent and adds it to the Data workspace
    /// </summary>
    /// <param name="extentManager">Extent Manager to be used</param>
    /// <param name="uri"></param>
    /// <param name="filename"></param>
    /// <param name="workspace">Name of the workspace</param>
    /// <returns></returns>
    public static async Task<ExtentStorageData.LoadedExtentInformation> CreateAndAddXmiExtent(
        this ExtentManager extentManager, string uri, string filename, string workspace = WorkspaceNames.WorkspaceData)
    {
        var configuration = InMemoryObject.CreateEmpty(
            _ExtentLoaderConfigs.TheOne.__XmiStorageLoaderConfig);
        configuration.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, uri);
        configuration.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId, workspace);
        configuration.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, filename);

        return await extentManager.LoadExtent(configuration, ExtentCreationFlags.LoadOrCreate);
    }
        
    /// <summary>
    /// Creates an xmi extent by using the text of the xml document.
    /// The xmi extent is just temporarily and will not be added to the extent manager
    /// </summary>
    /// <param name="extentManager">Extent Manager to be used</param>
    /// <param name="uri">Uri of the extent</param>
    /// <param name="xmlDocument">Xml Document being used</param>
    /// <param name="workspaceName">Workspace to which the extent is added</param>
    /// <returns>The created workspace</returns>
    public static IUriExtent CreateXmiExtentByDocument(
        this ExtentManager extentManager, string uri, string xmlDocument,
        string workspaceName = WorkspaceNames.WorkspaceData)
    {
        var workspace = extentManager.WorkspaceLogic.GetWorkspace(workspaceName)
                        ?? throw new InvalidOperationException("The workspace is not found");

        var document = XDocument.Parse(xmlDocument);
        var provider = new XmiProvider(document);
        var extent = new MofUriExtent(provider, uri, extentManager.ScopeStorage);
        workspace.AddExtent(extent);
        return extent;
    }
        
    /// <summary>
    /// Creates an xmi extent by using the text of the xml document.
    /// The xmi extent is just temporarily and will not be added to the extent manager
    /// </summary>
    /// <param name="extentManager">Extent Manager to be used</param>
    /// <param name="uri">Uri of the extent</param>
    /// <param name="xmlDocument">Xml Document being used</param>
    /// <param name="workspaceName">Workspace to which the extent is added</param>
    /// <returns>The created workspace</returns>
    public static IUriExtent CreateXmiExtentByDocument(
        this ExtentManager extentManager, string uri, XDocument xmlDocument,
        string workspaceName = WorkspaceNames.WorkspaceData)
    {
        var workspace = extentManager.WorkspaceLogic.GetWorkspace(workspaceName)
                        ?? throw new InvalidOperationException("The workspace is not found");

        var provider = new XmiProvider(xmlDocument);
        var extent = new MofUriExtent(provider, uri, extentManager.ScopeStorage);
        workspace.AddExtent(extent);
        return extent;
    }
        
        

    /// <summary>
    /// Loads the extent, if the extent is not already loaded.
    /// If the extent is already loaded, the already loaded extent will be directly returned
    /// If the extent is not loaded, the extent will be loaded according the configuration
    /// </summary>
    /// <param name="extentManager">The extent manager to be used</param>
    /// <param name="loaderConfiguration">The loader configuration being used to load the extent
    /// Of Type ExtentLoaderConfig</param>
    /// <param name="flags">The extent creation flags being used to load the extent</param>
    /// <returns>The found or loaded extent</returns>
    public static async Task<IUriExtent?> LoadExtentIfNotAlreadyLoaded(
        this ExtentManager extentManager,
        IElement loaderConfiguration,
        ExtentCreationFlags flags = ExtentCreationFlags.LoadOnly)
    {
        var asExtentManager = extentManager
                              ?? throw new InvalidOperationException("extentManager is not ExtentManager");
        var workspaceLogic = asExtentManager.WorkspaceLogic;
        var workspace = workspaceLogic.GetWorkspace(
            loaderConfiguration.getOrDefault<string>(_ExtentLoaderConfigs._ExtentLoaderConfig
                .workspaceId) ?? WorkspaceNames.WorkspaceData);

        var foundExtent = workspace?.extent.OfType<IUriExtent>().FirstOrDefault(
            x => x.contextURI() ==
                 loaderConfiguration.getOrDefault<string>(_ExtentLoaderConfigs._ExtentLoaderConfig
                     .extentUri));

        return foundExtent ?? (await extentManager.LoadExtent(loaderConfiguration, flags)).Extent;
    }

    /// <summary>
    /// Deletes the extent from the workspace and the extent manager
    /// </summary>
    /// <param name="manager">Manager to be used</param>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="extentUri">Uri of the extent</param>
    /// <returns>true, if successfully deleted</returns>
    public static async Task<bool> DeleteExtent(this ExtentManager manager, string workspaceId, string extentUri)
    {
        var found = manager.WorkspaceLogic.FindExtent(workspaceId, extentUri);
        if (found != null)
        {
            await manager.RemoveExtent(found);
            return true;
        }

        return false;
    }
}