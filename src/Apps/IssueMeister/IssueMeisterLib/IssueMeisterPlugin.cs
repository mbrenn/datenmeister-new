using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using JetBrains.Annotations;

namespace IssueMeisterLib;

[UsedImplicitly]
public class IssueMeisterPlugin : IDatenMeisterPlugin
{
    public const string DmInternTypesDomainsDatenmeister = "dm:///intern.types.issues.datenmeister/";
    public const string DmInternManagementDomainsDatenmeister = "dm:///intern.management.issues.datenmeister/";

    public const string ExtentTypeName = "IssueMeister";
    private readonly IScopeStorage _scopeStorage;
    private readonly ExtentSettings _extentSettings;
    private readonly IWorkspaceLogic _workspaceLogic;

    /// <summary>
    /// Initializes a new instance of the IssueMeisterPlugin
    /// </summary>
    /// <param name="workspaceLogic">The workspace logic</param>
    /// <param name="scopeStorage">The settings for the extent</param>
    public IssueMeisterPlugin(
        IWorkspaceLogic workspaceLogic,
        IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
        _extentSettings = scopeStorage.Get<ExtentSettings>();
    }

    public Task Start(PluginLoadingPosition position)
    {
        // Add the two xmi-extents to the workspace
        var assemblyType = typeof(IssueMeisterPlugin);
        var resourcePathTypes = "IssueMeisterLib.Xmi.IssueMeister.Types.xml";
        var resourcePathManagement = "IssueMeisterLib.Xmi.IssueMeister.Forms.xml";
                
        // Create the extentManager
        var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
        var issueMeisterTypes = 
            extentManager.LoadNonPersistentExtentFromResources(assemblyType, resourcePathTypes, WorkspaceNames.WorkspaceTypes, DmInternTypesDomainsDatenmeister);
        var issueMeisterManagement = 
            extentManager.LoadNonPersistentExtentFromResources(assemblyType, resourcePathManagement, WorkspaceNames.WorkspaceManagement, DmInternManagementDomainsDatenmeister);

        var extentSetting =
            new ExtentType(ExtentTypeName);
        extentSetting.rootElementMetaClasses.Add(
            issueMeisterTypes.element("#IssueMeister.Issue")
            ?? throw new InvalidOperationException("IssueMeister.Issue was not found. "));
        _extentSettings.extentTypeSettings.Add(extentSetting);
        
        MofUriExtent.Migration.AddConverter("dm:///_internal/types/internal#IssueMeister.Issue", DmInternTypesDomainsDatenmeister + "#IssueMeister.Issue");

        return Task.CompletedTask;
    }
}