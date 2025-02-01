using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Plugins;

namespace DatenMeister.Forms.Actions
{
    internal class Plugin : IDatenMeisterPlugin
    {
        public Plugin(IScopeStorage scopeStorage)
        {
            ScopeStorage = scopeStorage;
        }

        public IScopeStorage ScopeStorage { get; }

        public Task Start(PluginLoadingPosition position)
        {
            ScopeStorage.Get<ActionLogicState>().AddActionHandler(new NavigateToFieldsForTestActionHandler());

            return Task.CompletedTask;
        }
    }
}
