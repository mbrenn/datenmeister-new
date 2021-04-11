using DatenMeister.Excel.Properties;
using DatenMeister.Plugins;

namespace DatenMeister.WPF.Modules.FormManager
{
    /// <summary>
    /// Defines the plugin for the view manager
    /// </summary>
    [UsedImplicitly]
    public class FormManagerPlugin : IDatenMeisterPlugin
    {

        /// <inheritdoc />
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new FormManagerViewExtension());
        }
    }
}