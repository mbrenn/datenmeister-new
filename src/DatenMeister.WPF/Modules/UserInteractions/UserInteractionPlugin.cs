using DatenMeister.Core;
using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.WPF.Modules.UserInteractions
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class UserInteractionPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public UserInteractionPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterInitialization:
                    _scopeStorage.Add(new UserInteractionState());
                    break;
            }
        }
    }
}