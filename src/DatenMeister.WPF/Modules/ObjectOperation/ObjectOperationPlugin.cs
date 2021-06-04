using DatenMeister.Plugins;

namespace DatenMeister.WPF.Modules.ObjectOperation
{
    public class ObjectOperationPlugin : IDatenMeisterPlugin
    {
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new ObjectOperationViewExtension());
        }
    }
}