using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Uml.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly FormLogic _formLogic;

        private readonly PackageMethods _packageMethods;

        public const string PackageName = "Uml";

        public UmlPlugin(FormLogic formLogic, PackageMethods packageMethods)
        {
            _formLogic = formLogic;
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
                _formLogic.GetInternalFormExtent(),
                PackageName);
        }
    }
}