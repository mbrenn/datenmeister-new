using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
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
        if (position == PluginLoadingPosition.AfterLoadingOfExtents)
        {
            var assemblyType = typeof(Plugin);

            // Load the types
            var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
            var resourcePathTypes = AssemblyName + ".Xmi.Types.xmi";
            extentManager.LoadNonPersistentExtentFromResources(assemblyType, resourcePathTypes,
                WorkspaceNames.WorkspaceTypes, DmTypesUriReference);

            // Adds the javascript
            var pageRegistrationData = scopeStorage.Get<PageRegistrationData>();
            var pageRegistrationLogic = new PageRegistrationLogic(pageRegistrationData);
            pageRegistrationLogic.AddJavaScriptFromResource(
                typeof(Plugin),
                AssemblyName + ".Js.Demo.js",
                AssemblyName + ".Demo.js",
                "../../" + AssemblyName + "/Js/Demo.js");
        }

        return Task.CompletedTask;
    }
}