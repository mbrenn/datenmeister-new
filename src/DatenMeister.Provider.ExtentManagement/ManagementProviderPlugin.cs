using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Plugins;

namespace DatenMeister.Provider.ExtentManagement;

/// <summary>
///     Defines a plugin for the management provider
/// </summary>
[PluginLoading(PluginLoadingPosition.AfterInitialization)]
public class ManagementProviderPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    /// <summary>
    ///     Extent of workspaces
    /// </summary>
    public const string UriExtentWorkspaces = WorkspaceNames.UriExtentWorkspaces;

    private readonly ExtentSettings _extentSettings = scopeStorage.Get<ExtentSettings>();

    public const string WorkspaceExtentType = "DatenMeister.Workspaces";

    public Task Start(PluginLoadingPosition position)
    {
        // Adds the extent setting
        _extentSettings.extentTypeSettings.Add(new ExtentType(WorkspaceExtentType));
            
        // Adds the extent
        var managementExtent = new MofUriExtent(
            new ExtentOfWorkspaceProvider(workspaceLogic, scopeStorage),
            WorkspaceNames.UriExtentWorkspaces, scopeStorage);
            
        managementExtent.ExtentConfiguration.ExtentType = WorkspaceExtentType;
            
        // Adds the extent containing the workspaces
        workspaceLogic.GetManagementWorkspace().AddExtent(managementExtent);

        return Task.CompletedTask;
    }
}