using DatenMeister.Core.Runtime;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.WPF.Modules.DefaultTypes
{
    public class DefaultTypesPlugin : IDatenMeisterPlugin
    {
        private readonly DefaultClassifierHints _hints;

        public DefaultTypesPlugin(DefaultClassifierHints hints)
        {
            _hints = hints;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new DefaultTypesExtension(_hints));
        }
    }
}