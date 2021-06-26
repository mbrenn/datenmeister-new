using DatenMeister.Core;
using DatenMeister.Forms;
using DatenMeister.Plugins;

namespace DatenMeister.Modules.DefaultTypes
{
    public class DefaultTypesPlugin : IDatenMeisterPlugin
    {
        private IScopeStorage _scopeStorage;

        public DefaultTypesPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }
        public void Start(PluginLoadingPosition position)
        {
            var formsPluginState = _scopeStorage.Get<FormsPluginState>();
            formsPluginState.FormModificationPlugins.Add(
                new DefaultTypesFormModificationPlugin());
        }
    }
}