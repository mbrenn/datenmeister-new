using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Plugins;
using DatenMeister.Plugins.Helper;

namespace DatenMeister.DataView.Forms;

[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
// ReSharper disable once UnusedType.Global
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    private const string DmTypesUriReference = "dm:///types.forms.dataview.datenmeister/";
    private const string DmManagementUriReference = "dm:///management.forms.dataview.datenmeister/";

    public Task Start(PluginLoadingPosition position)
    {
        // Defines the pluginhelper
        var pluginHelper = new PluginHelper(
            new PluginHelperConfiguration
            {
                PluginType = typeof(Plugin),
                Position = position,
                ScopeStorage = scopeStorage,
                WorkspaceLogic = workspaceLogic
            });
        
        // Adds the extent
        pluginHelper.AddExtentForTypesFromManifest("Xmi.Types.xmi", DmTypesUriReference);
        pluginHelper.AddExtentForManagementFromManifest("Xmi.Forms.xmi", DmManagementUriReference);
        
        // Adds the javascript file
        var jsParameter = new AddJavaScriptFileToWebserverParameter();
        jsParameter.SetByRelativeFileName("Js/DatenMeister.ViewNode.Forms.js");
        jsParameter.ProjectsRelativePathToDevelopmentFile = "../../DatenMeister.DataView.Forms";
        pluginHelper.AddJavaScriptFileToWebServer(jsParameter);
        
        // Adds the action handler
        pluginHelper.AddActionHandler(new ViewNodeRenderActionHandler());
        
        return Task.CompletedTask;
    }
}