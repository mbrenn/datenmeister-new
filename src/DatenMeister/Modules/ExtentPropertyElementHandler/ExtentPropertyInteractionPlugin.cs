using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
{
    public class ExtentPropertyInteractionPlugin : IDatenMeisterPlugin
    {
        private IScopeStorage _scopeStorage;

        public ExtentPropertyInteractionPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
             
        }
    }
}