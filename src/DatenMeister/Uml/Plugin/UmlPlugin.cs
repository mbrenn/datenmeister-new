using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Extents.Configuration;
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
        private readonly FormsPlugin _formsPlugin;

        private readonly PackageMethods _packageMethods;

        public const string PackageName = "Uml";

        /// <summary>
        /// Stores the name of the extent type
        /// </summary>
        public const string ExtentType = "Uml.Classes";

        public UmlPlugin(FormsPlugin formsPlugin, PackageMethods packageMethods, ExtentSettings extentSettings)
        {
            _formsPlugin = formsPlugin;
            _packageMethods = packageMethods;
            extentSettings.extentTypeSettings.Add(
                new ExtentTypeSetting(ExtentType));
            
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
                "DatenMeister.XmiFiles.Forms.UML.xmi",
                PackageName,
                _formsPlugin.GetInternalFormExtent(),
                PackageName);
        }
    }
}