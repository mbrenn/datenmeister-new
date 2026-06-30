using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Plugins;
using DatenMeister.Plugins.Helper;
using DatenMeister.WebServer.Library.PageRegistration;

namespace DatenMeister.Templates.TypeScriptAndTypesPlugin;

[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
// ReSharper disable once UnusedType.Global
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    private const string AssemblyName = "DatenMeister.Templates.TypeScriptAndTypesPlugin";
    private const string DmTypesUriReference = "dm:///intern.types.template.typescriptandtypesplugin.datenmeister/";

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
        
        // Adds the javascript file
        var jsParameter = new AddJavaScriptFileToWebserverParameter();
        jsParameter.SetByRelativeFileName("Js/Demo.js");
        jsParameter.ProjectsRelativePathToDevelopmentFile = "../../../DatenMeister.Templates.TypeScriptAndTypesPlugin";
        pluginHelper.AddJavaScriptFileToWebServer(jsParameter);
        
        return Task.CompletedTask;
    }
}