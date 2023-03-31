using System.Text;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.WebServer.Library.PageRegistration;

namespace DatenMeister.StundenPlan;

public class StundenPlanPlugin : IDatenMeisterPlugin
{
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;
    
    private const string ExtentTypeName = "StundenPlan";
    
    public const string ViewModeName = "StundenPlan";

    public const string UriStundenPlanForm = "dm:///forms.stundenplan.datenmeister/";
    public const string UriStundenPlanTypes = "dm:///types.stundenplan.datenmeister/";

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
            "DatenMeister.StundenPlan.xmi.StundenPlan.Forms.xml",
            UriStundenPlanForm,
            "Management");
        
        extentManager.CreateExtentByResource(
            typeof(StundenPlanPlugin),
            "DatenMeister.StundenPlan.xmi.StundenPlan.Types.xml",
            UriStundenPlanTypes,
            "Types");

        var extentSettings = _scopeStorage.Get<ExtentSettings>();
        var extentSetting =
            new ExtentType(ExtentTypeName);
        
        extentSettings.extentTypeSettings.Add(extentSetting);


        var pluginLogic = new PageRegistrationLogic(_scopeStorage.Get<PageRegistrationData>());
        pluginLogic.AddUrl(
            "/love_you", 
            "text/plain",
            () => new MemoryStream(
                Encoding.UTF8.GetBytes("I love me")));
    }
}