using DatenMeister.Core;
using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Apps.ZipExample
{
    public class ZipCodeWpfPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public ZipCodeWpfPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            _scopeStorage
                .Get<UserInteractionState>()
                .ElementInteractionHandler
                .Add(
                    new ZipCodeInteractionHandler());
        }
    }
}