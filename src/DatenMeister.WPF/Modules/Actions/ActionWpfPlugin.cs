using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Actions
{
    /// <summary>
    /// Defines the plugin for action
    /// </summary>
    public class ActionWpfPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public ActionWpfPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            _scopeStorage
                .Get<UserInteractionState>()
                .ElementInteractionHandler
                .AddRange(
                    new BaseElementInteractionHandler[]
                    {
                        new ActionSetInteractionHandler(),
                        new ActionInteractionHandler()
                    });
        }
    }
}