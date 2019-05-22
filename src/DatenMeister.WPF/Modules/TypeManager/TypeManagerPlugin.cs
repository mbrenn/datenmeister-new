using DatenMeister.Core.Plugins;
using DatenMeister.Excel.Annotations;

namespace DatenMeister.WPF.Modules.TypeManager
{
    [UsedImplicitly]
    public class TypeManagerPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Starts the plugin
        /// </summary>
        /// <param name="position">Position of the plugin</param>
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new TypeManagerViewExtension());
        }
    }
}