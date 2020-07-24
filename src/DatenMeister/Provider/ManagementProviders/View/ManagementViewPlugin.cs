using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.Provider.ManagementProviders.View
{
    [PluginDependency(typeof(UmlPlugin))]
    // ReSharper disable once UnusedMember.Global
    public class ManagementViewPlugin : IDatenMeisterPlugin
    {
        private readonly FormsPlugin _formsPlugin;
        private readonly PackageMethods _packageMethods;

        /// <summary>
        /// Initializes a new instance of the ManagementViewDefinitions class
        /// </summary>
        /// <param name="formsPlugin">View logic being used to find View Extent</param>
        /// <param name="packageMethods">The helper for package methods</param>
        public ManagementViewPlugin(FormsPlugin formsPlugin, PackageMethods packageMethods)
        {
            _formsPlugin = formsPlugin;
            _packageMethods = packageMethods;
        }

        public void Start(PluginLoadingPosition position)
        {
            AddToViewDefinition();
        }

        /// <summary>
        /// Adds the views to the view logic
        /// </summary>
        public void AddToViewDefinition()
        {
            _packageMethods.ImportByManifest(
                typeof(ManagementViewPlugin),
                "DatenMeister.XmiFiles.Forms.WorkspacesAndExtents.xmi",
                ManagementViewDefinitions.PackageName,
                _formsPlugin.GetInternalFormExtent(),
                $"DatenMeister::{ManagementViewDefinitions.PackageName}");
        }
    }
}