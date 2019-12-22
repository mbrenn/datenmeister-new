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

        private readonly FormLogic _formLogic;

        public const string PackageName = "FormManager";

        public FormManagerPlugin(PackageMethods packageMethods, FormLogic formLogic)
        {
            _packageMethods = packageMethods;
            _formLogic = formLogic;
        }

        /// <inheritdoc />
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new FormManagerViewExtension());

            _packageMethods.ImportByManifest(
                typeof(UmlPlugin),
                "DatenMeister.XmiFiles.Views.Formmanager.xmi",
                PackageName,
                _formLogic.GetInternalFormExtent(),
                PackageName);
        }
    }
}