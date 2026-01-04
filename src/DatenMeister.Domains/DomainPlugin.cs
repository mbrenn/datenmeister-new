using System.Xml.Linq;
using BurnSystems;
using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

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
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:
               
                // Add the two xmi-extents to the workspace
                var assemblyType = typeof(DomainPlugin);
                var resourcePathTypes = "DatenMeister.Domains.Xmi.DatenMeister.Domains.Types.xmi";
                var resourcePathManagement = "DatenMeister.Domains.Xmi.DatenMeister.Domains.Management.xmi";
                
                // Create the extentManager
                var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
                extentManager.LoadNonPersistentExtentFromResources(assemblyType, resourcePathTypes, WorkspaceNames.WorkspaceTypes, DmInternTypesDomainsDatenmeister);
                extentManager.LoadNonPersistentExtentFromResources(assemblyType, resourcePathManagement, WorkspaceNames.WorkspaceManagement, DmInternManagementDomainsDatenmeister);
                
                // Add the Action Handler
                var actionLogicState = scopeStorage.Get<ActionLogicState>();
                actionLogicState.AddActionHandler(new DomainCreateFoundationActionHandler(workspaceLogic, scopeStorage));
                break;
        }
        
        await Task.CompletedTask;
    }
}