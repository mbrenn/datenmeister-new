using DatenMeister.Core.Plugins;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Uml.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        private readonly PackageMethods _packageMethods;

        public const string PackageName = "Uml";

        public UmlPlugin(ViewLogic viewLogic, PackageMethods packageMethods)
        {
            _viewLogic = viewLogic;
            _packageMethods = packageMethods;
        }

        public void Start(PluginLoadingPosition position)
        {
            AddToViewDefinition();
        }

        /// <summary>
        /// Adds the views to the view logic.
        /// </summary>
        public void AddToViewDefinition()
        {
            _packageMethods.ImportByManifest(
                typeof(UmlPlugin),
                "DatenMeister.XmiFiles.Views.UML.xmi",
                PackageName,
                _viewLogic.GetInternalViewExtent(),
                PackageName);
        }
    }
}