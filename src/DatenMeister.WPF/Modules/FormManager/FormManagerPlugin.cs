using DatenMeister.Excel.Properties;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.WPF.Modules.FormManager
{
    /// <summary>
    /// Defines the plugin for the view manager
    /// </summary>
    [UsedImplicitly]
    public class FormManagerPlugin : IDatenMeisterPlugin
    {
        private readonly PackageMethods _packageMethods;

        private readonly FormsPlugin _formsPlugin;

        public const string PackageName = "FormManager";

        public FormManagerPlugin(PackageMethods packageMethods, FormsPlugin formsPlugin)
        {
            _packageMethods = packageMethods;
            _formsPlugin = formsPlugin;
        }

        /// <inheritdoc />
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new FormManagerViewExtension());

            _packageMethods.ImportByManifest(
                typeof(UmlPlugin),
                "DatenMeister.XmiFiles.Forms.Formmanager.xmi",
                PackageName,
                _formsPlugin.GetInternalFormExtent(),
                $"DatenMeister::{PackageName}");
        }
    }
}