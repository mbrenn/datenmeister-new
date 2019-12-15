using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.Provider.ManagementProviders
{
    [PluginDependency(typeof(UmlPlugin))]
    // ReSharper disable once UnusedMember.Global
    public class ManagementViewPlugin : IDatenMeisterPlugin
    {
        private readonly ViewLogic _viewLogic;
        private readonly PackageMethods _packageMethods;

        /// <summary>
        /// Initializes a new instance of the ManagementViewDefinitions class
        /// </summary>
        /// <param name="viewLogic">View logic being used to find View Extent</param>
        /// <param name="packageMethods">The helper for package methods</param>
        public ManagementViewPlugin(ViewLogic viewLogic, PackageMethods packageMethods)
        {
            _viewLogic = viewLogic;
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
                "DatenMeister.XmiFiles.Views.WorkspacesAndExtents.xmi",
                ManagementViewDefinitions.PackageName,
                _viewLogic.GetInternalViewExtent(),
                ManagementViewDefinitions.PackageName);
        }
    }
}