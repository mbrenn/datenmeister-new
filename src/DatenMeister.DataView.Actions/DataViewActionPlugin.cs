using DatenMeister.Actions;
using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.DataView.Actions;

[PluginLoading(PluginLoadingPosition.AfterInitialization)]
[PluginDependency(typeof(ActionsPlugin))]
public class DataViewActionPlugin (IScopeStorage scopeStorage): IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        if (position == PluginLoadingPosition.AfterInitialization)
        {
            var actionLogicState = scopeStorage.Get<ActionLogicState>();
            actionLogicState.AddActionHandler(new FreezeViewResultInMemoryHandler());
            actionLogicState.AddActionHandler(new FreezeViewResultInExtentHandler());
        }
        
        return Task.CompletedTask;
    }
}