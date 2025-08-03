using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Plugins;

namespace DatenMeister.Forms.Actions;

internal class Plugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public IScopeStorage ScopeStorage { get; } = scopeStorage;

    public Task Start(PluginLoadingPosition position)
    {
        ScopeStorage.Get<ActionLogicState>().AddActionHandler(new NavigateToFieldsForTestActionHandler());

        return Task.CompletedTask;
    }
}