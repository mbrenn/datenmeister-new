using DatenMeister.Core;
using DatenMeister.Plugins;

namespace DatenMeister.WPF.Modules.UserInteractions;

[PluginLoading(PluginLoadingPosition.AfterInitialization)]
public class UserInteractionPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterInitialization:
                scopeStorage.Add(new UserInteractionState());
                break;
        }
    }
}