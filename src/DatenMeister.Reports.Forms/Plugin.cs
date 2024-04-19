using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Plugins;
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
