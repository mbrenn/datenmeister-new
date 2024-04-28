using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Plugins;
using DatenMeister.Types;
using DatenMeister.WebServer.Library.PageRegistration;
using System.Reflection;

namespace DatenMeister.Reports.Forms
{
    [PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
    internal class Plugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic workspaceLogic;
        private readonly IScopeStorage scopeStorage;

        public Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            this.workspaceLogic = workspaceLogic;
            this.scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            if (position == PluginLoadingPosition.AfterLoadingOfExtents)
            {
                var localTypeSupport = new LocalTypeSupport(workspaceLogic, scopeStorage);
                var formMethods = new FormMethods(workspaceLogic, scopeStorage);
                var targetExtent = formMethods.GetInternalFormExtent();
                var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
                PackageMethods.ImportByStream(
                    GetXmiStreamForForms(), null, targetExtent, "DatenMeister.Reports.Forms");
                PackageMethods.ImportByStream(
                    GetXmiStreamForTypes(), null, localTypeExtent, "DatenMeister.Reports.Forms");

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
            }
        }

        public static Stream GetXmiStreamForForms()
        {
            var stream = typeof(Plugin).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Reports.Forms.Xmi.DatenMeister.Reports.Forms.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }

        public static Stream GetXmiStreamForTypes()
        {
            var stream = typeof(Plugin).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Reports.Forms.Xmi.DatenMeister.Reports.Types.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }
    }
}
