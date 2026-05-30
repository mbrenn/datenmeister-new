using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.Actions.Transformations;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
[PluginLoading(PluginLoadingPosition.AfterInitialization)]
[PluginDependency(typeof(ActionsPlugin))]
public class TransformationPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    /// <summary>
    /// Gets the scope storage
    /// </summary>
    private readonly IScopeStorage _scopeStorage = scopeStorage;

    public Task Start(PluginLoadingPosition position)
    {
        if (position == PluginLoadingPosition.AfterInitialization)
        {
            var found = _scopeStorage.Get<ActionLogicState>();
            found.ActionHandlers.Add(new TransformItemsActionHandler());
        }

        return Task.CompletedTask;
    }
}