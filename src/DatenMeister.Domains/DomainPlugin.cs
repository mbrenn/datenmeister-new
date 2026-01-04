using System.Xml.Linq;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
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
                var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
                // Loads the Documents
                var typesXmi = ResourceHelper.LoadStringFromAssembly(typeof(DomainPlugin), "DatenMeister.Domains.Xmi.DatenMeister.Domains.Types.xmi");
                var managementXmi = ResourceHelper.LoadStringFromAssembly(typeof(DomainPlugin), "DatenMeister.Domains.Xmi.DatenMeister.Domains.Management.xmi");
                
                // Loads the XDocument from the loaded Resource Document, reflecting the content. 
                var xmiTypesDocument = XDocument.Parse(typesXmi);
                var xmiManagementDocument = XDocument.Parse(managementXmi);

                // Loads the providers
                var xmiTypesProvider = new XmiProvider(xmiTypesDocument);
                var xmiManagementProvider = new XmiProvider(xmiTypesDocument);
                
                // Now, loads the UriExtents
                var typesExtent = new MofUriExtent(xmiTypesProvider, DmInternTypesDomainsDatenmeister, scopeStorage);
                var managementExtent = new MofUriExtent(xmiManagementProvider, DmInternManagementDomainsDatenmeister, scopeStorage);
                
                // Adds the extents to the workspaces directly, so they won't be loaded by the ExtentManager on Application Start-Up
                extentManager.AddNonPersistentExtent(WorkspaceNames.WorkspaceManagement, managementExtent);
                extentManager.AddNonPersistentExtent(WorkspaceNames.WorkspaceTypes, typesExtent);
                break;
            
        }
        
        await Task.CompletedTask;
    }
}