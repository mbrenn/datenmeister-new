using DatenMeister.Core;
using DatenMeister.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Apps.ZipExample;

public class ZipCodeWpfPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        scopeStorage
            .Get<UserInteractionState>()
            .ElementInteractionHandler
            .Add(
                new ZipCodeInteractionHandler());
    }
}