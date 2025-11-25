using DatenMeister.Actions;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Plugins;
using DatenMeister.Types;
using DatenMeister.WebServer.Library.PageRegistration;
using System.Reflection;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Reports.Forms;

[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
internal class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        if (position == PluginLoadingPosition.AfterLoadingOfExtents)
        {
            var localTypeSupport = new LocalTypeSupport(workspaceLogic, scopeStorage);
            var formMethods = new FormMethods(workspaceLogic);
            var targetExtent = formMethods.GetInternalFormExtent();
            var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
            
            // First, import the types
            PackageMethods.ImportByStream(
                GetXmiStreamForTypes(), null, localTypeExtent, "DatenMeister.Reports.Forms");
            
            // After that, import the forms
            PackageMethods.ImportByStream(
                GetXmiStreamForForms(), null, targetExtent, "DatenMeister.Reports.Forms");

            // Adds the javascript
            var pluginLogic = new PageRegistrationLogic(scopeStorage.Get<PageRegistrationData>());
            pluginLogic.AddJavaScriptFromResource(
                typeof(Plugin),
                "DatenMeister.Reports.Forms.Js.DatenMeister.Reports.Forms.js",
                "DatenMeister.Reports.Forms.js",
                "../../Datenmeister.Reports.Forms/js/DatenMeister.Reports.Forms.js");
            pluginLogic.AddJavaScriptFromResource(
                typeof(Plugin),
                "DatenMeister.Reports.Forms.Js.DatenMeister.Reports.Types.js",
                "DatenMeister.Reports.Types.js",
                "../../Datenmeister.Reports.Forms/js/DatenMeister.Reports.Types.js");

            pluginLogic.AddCssFileFromResource(
                typeof(Plugin),
                "DatenMeister.Reports.Forms.Css.DatenMeister.Reports.Forms.css",
                "DatenMeister.Reports.Types.css",
                "../../Datenmeister.Reports.Forms/js/DatenMeister.Reports.Types.css");

            // Adds the action handler
            scopeStorage.Get<ActionLogicState>().AddActionHandler(
                new RequestReportAction(workspaceLogic, scopeStorage));
        }

        return Task.CompletedTask;
    }

    public static Stream GetXmiStreamForForms()
    {
        var stream = typeof(Plugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Reports.Forms.Xmi.DatenMeister.Reports.Forms.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }

    public static Stream GetXmiStreamForTypes()
    {
        var stream = typeof(Plugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Reports.Forms.Xmi.DatenMeister.Reports.Types.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }
}