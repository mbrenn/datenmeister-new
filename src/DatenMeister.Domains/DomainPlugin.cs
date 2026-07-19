using DatenMeister.Actions;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Plugins.Helper;

namespace DatenMeister.Domains;

/// <summary>
/// Defines the domain Plugin which is used to load the Types and Management Information. 
/// </summary>
[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
public class DomainPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin {
    
    public const string DmInternTypesDomainsDatenmeister = "dm:///intern.types.domains.datenmeister/";
    public const string DmInternManagementDomainsDatenmeister = "dm:///intern.management.domains.datenmeister/";

    public async Task Start(PluginLoadingPosition position)
    {
        // Defines the pluginhelper
        var pluginHelper = new PluginHelper(
            new PluginHelperConfiguration
            {
                PluginType = typeof(DomainPlugin),
                Position = position,
                ScopeStorage = scopeStorage,
                WorkspaceLogic = workspaceLogic
            });
        
        pluginHelper.AddExtentForTypesFromManifest("Xmi.DatenMeister.Domains.Types.xmi", DmInternTypesDomainsDatenmeister);
        pluginHelper.AddExtentForManagementFromManifest("Xmi.DatenMeister.Domains.Management.xmi", DmInternManagementDomainsDatenmeister);
        pluginHelper.AddActionHandler(new DomainCreateFoundationActionHandler(workspaceLogic, scopeStorage));
        
        await Task.CompletedTask;
    }
}