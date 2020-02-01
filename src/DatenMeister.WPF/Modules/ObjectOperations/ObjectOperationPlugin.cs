using DatenMeister.Runtime.Plugins;

namespace DatenMeister.WPF.Modules.ObjectOperations
{
    public class ObjectOperationPlugin : IDatenMeisterPlugin
    {
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new ObjectOperationViewExtension());
        }
    }
}