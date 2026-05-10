using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.Actions;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
[PluginLoading(PluginLoadingPosition.AfterInitialization)]
public class ActionsPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    /// <summary>
    /// Gets the scope storage
    /// </summary>
    private readonly IScopeStorage _scopeStorage = scopeStorage;

    public Task Start(PluginLoadingPosition position)
    {
        if (position == PluginLoadingPosition.AfterInitialization)
        {
            var found = _scopeStorage.TryGet<ActionLogicState>();
            if (found != null)
            {
                throw new InvalidOperationException("ActionLogicState is already present");
            }

            _scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
        }

        return Task.CompletedTask;
    }
}