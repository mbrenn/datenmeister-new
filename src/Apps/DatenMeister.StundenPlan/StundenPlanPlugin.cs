using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.StundenPlan;

public class StundenPlanPlugin : IDatenMeisterPlugin
{
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;
    
    private const string ExtentTypeName = "StundenPlan";
    
    public const string ViewModeName = "StundenPlan";

    public const string UriStundenPlanForm = "dm:///datenmeister.stundenmeister/";
    public const string UriStundenPlanTypes = "dm:///datenmeister.stundenmeister/";

    public StundenPlanPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
    }
    
    public void Start(PluginLoadingPosition position)
    {
        var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
        extentManager.CreateExtentByResource(
            typeof(StundenPlanPlugin),
            "IssueMeisterLib.Xmi.IssueMeister.Forms.xml",
            UriStundenPlanForm,
            "Management");
        
        extentManager.CreateExtentByResource(
            typeof(StundenPlanPlugin),
            "IssueMeisterLib.Xmi.IssueMeister.Types.xml",
            UriStundenPlanTypes,
            "Types");

        var extentSettings = _scopeStorage.Get<ExtentSettings>();
        var extentSetting =
            new ExtentType(ExtentTypeName);
        
        extentSettings.extentTypeSettings.Add(extentSetting);
    }
}