using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Extent.Manager.ExtentStorage;

/// <summary>
/// Implements some helper methods to create extents
/// </summary>
public class ExtentCreator
{
    private static readonly ClassLogger Logger = new(typeof(ExtentCreator));
    private readonly ExtentManager _extentManager;
    private readonly IntegrationSettings _integrationSettings;
    private readonly IWorkspaceLogic _workspaceLogic;

    public ExtentCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _extentManager = new ExtentManager(workspaceLogic, scopeStorage);
        _integrationSettings = scopeStorage.Get<IntegrationSettings>();
    }

    public ExtentCreator(IWorkspaceLogic workspaceLogic, ExtentManager extentManager, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _extentManager = extentManager;
        _integrationSettings = scopeStorage.Get<IntegrationSettings>();
    }

    /// <summary>
    /// Gets or creates an Xmi Extent in the internal database
    /// </summary>
    /// <param name="scope">Dependency Container</param>
    /// <param name="workspace">Workspace in which the Xmi extent will be stored</param>
    /// <param name="uri">Uri of the extent</param>
    /// <param name="name">Name of the extent (being used to stored into database). The
    /// name needs to be unique</param>
    /// <returns>The uri extent being found</returns>
    public static async Task<IUriExtent?> GetOrCreateXmiExtentInInternalDatabase(ILifetimeScope scope, string workspace,
        string uri, string name)
    {
        var creator = scope.Resolve<ExtentCreator>();
        return await creator.GetOrCreateXmiExtentInInternalDatabase(workspace, uri, name);
    }

    public async Task<IUriExtent?> GetOrCreateXmiExtentInInternalDatabase(
        string? workspace,
        string uri,
        string name,
        string extentType = "",
        ExtentCreationFlags extentCreationFlags = ExtentCreationFlags.LoadOrCreate)
    {
        // Creates the user types, if not existing
        var foundExtent = _workspaceLogic.FindExtent(uri);
        if (foundExtent == null)
        {
            Logger.Debug("Creates the extent for the " + name);

            // Creates the extent for user types

            var storageConfiguration = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
                uri);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                Path.Combine(_integrationSettings.DatabasePath, Path.Combine("extents/", name + ".xml")));
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
                workspace);

            foundExtent = (await _extentManager.LoadExtent(storageConfiguration, extentCreationFlags)).Extent;

            if (foundExtent != null)
            {
                foundExtent.GetConfiguration().ExtentType = extentType;
            }

            return (IUriExtent?)foundExtent;
        }

        return (IUriExtent?)foundExtent;
    }
}