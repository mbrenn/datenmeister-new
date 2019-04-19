using DatenMeister.Core.Plugins;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.WPF.Modules.ImportExtentManager
{
    public class ImportExtentManagerPlugin : IDatenMeisterPlugin
    {
        public const string PackageName = "ImportExtentManager";

        private readonly ViewLogic _viewLogic;

        private readonly PackageMethods _packageMethods;

        public ImportExtentManagerPlugin(ViewLogic viewLogic, PackageMethods packageMethods)
        {
            _viewLogic = viewLogic;
            _packageMethods = packageMethods;
        }

        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new ImportExtentViewExtensions());
            _packageMethods.ImportByManifest(
                typeof(UmlPlugin),
                "DatenMeister.XmiFiles.Views.ImportExtentManager.xmi",
                PackageName,
                _viewLogic.GetInternalViewExtent(),
                PackageName);
        }
    }
}