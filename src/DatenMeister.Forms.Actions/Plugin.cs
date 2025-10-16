using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.Forms.Actions;

internal class Plugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public IScopeStorage ScopeStorage { get; } = scopeStorage;

    public Task Start(PluginLoadingPosition position)
    {
        var actionLogicState = ScopeStorage.Get<ActionLogicState>();
        actionLogicState.AddActionHandler(new NavigateToFieldsForTestActionHandler());
        actionLogicState.AddActionHandler(new CreateFormUponViewActionHandler());

        return Task.CompletedTask;
    }
}