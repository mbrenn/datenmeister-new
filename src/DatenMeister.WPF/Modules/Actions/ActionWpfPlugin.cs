using DatenMeister.Core;
using DatenMeister.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Actions;

/// <summary>
/// Defines the plugin for action
/// </summary>
public class ActionWpfPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        scopeStorage
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