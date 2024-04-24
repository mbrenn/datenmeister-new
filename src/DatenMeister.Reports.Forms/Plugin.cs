using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Plugins;
using DatenMeister.WebServer.Library.PageRegistration;
using System.Reflection;

namespace DatenMeister.Reports.Forms
{
    [PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
    internal class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
    {
        public void Start(PluginLoadingPosition position)
        {
            if (position == PluginLoadingPosition.AfterLoadingOfExtents)
            {
                var formMethods = new FormMethods(workspaceLogic, scopeStorage);
                var targetExtent = formMethods.GetInternalFormExtent();
                PackageMethods.ImportByStream(
                    GetXmiStream(), null, targetExtent, "DatenMeister.Reports.Forms");

                // Adds the javascript
                var pluginLogic = new PageRegistrationLogic(scopeStorage.Get<PageRegistrationData>());
                pluginLogic.AddJavaScriptFromResource(
                    typeof(Plugin),
                    "DatenMeister.Reports.Forms.Js.DatenMeister.Reports.Forms.js",
                    "DatenMeister.Reports.Forms.js",
                    "../../Datenmeister.Reports.Forms/js/DatenMeister.Reports.Forms.js");
            }
        }

        public static Stream GetXmiStream()
        {
            var stream = typeof(Plugin).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Reports.Forms.Xmi.DatenMeister.Reports.Forms.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }
    }
}
