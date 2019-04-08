using DatenMeister.Core.Plugins;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.WPF.Modules.ViewManager
{
    /// <summary>
    /// Defines the plugin for the view manager
    /// </summary>
    public class ViewManagerPlugin : IDatenMeisterPlugin
    {
        private PackageMethods _packageMethods;

        private ViewLogic _viewLogic;

        public const string PackageName = "ViewManager";

        public ViewManagerPlugin(PackageMethods packageMethods, ViewLogic viewLogic)
        {
            _packageMethods = packageMethods;
            _viewLogic = viewLogic;
        }

        /// <inheritdoc />
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new ViewManagerViewExtensionFactory());
            _packageMethods.ImportByManifest(
                typeof(UmlPlugin),
                "DatenMeister.XmiFiles.Views.Viewmanager.xmi",
                PackageName,
                _viewLogic.GetInternalViewExtent(),
                PackageName);
        }
    }
}