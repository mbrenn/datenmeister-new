using DatenMeister.Core.Runtime;
using DatenMeister.Plugins;

namespace DatenMeister.WPF.Modules.DefaultTypes;

public class DefaultTypesPlugin(DefaultClassifierHints hints) : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new DefaultTypesExtension(hints));
    }
}