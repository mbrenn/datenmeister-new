using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
{
    [PluginDependency(typeof(UserInteractionPlugin))]
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class ExtentPropertyInteractionPlugin : IDatenMeisterPlugin
    {
        private IScopeStorage _scopeStorage;

        public ExtentPropertyInteractionPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
             _scopeStorage.Get<UserInteractionState>().ElementInteractionHandler.Add(
                 new ExtentPropertyUserInteraction(_scopeStorage));
        }
    }
}