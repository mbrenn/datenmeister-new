using DatenMeister.Core.Plugins;

namespace DatenMeister.WPF.Modules.DefaultTypes
{
    public class DefaultTypesPlugin : IDatenMeisterPlugin
    {
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new DefaultTypesExtension());
        }
    }
}