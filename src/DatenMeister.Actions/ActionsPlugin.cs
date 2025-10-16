using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.Actions;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public class ActionsPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    /// <summary>
    /// Gets the scope storage
    /// </summary>
    private readonly IScopeStorage _scopeStorage = scopeStorage;

    public Task Start(PluginLoadingPosition position)
    {
        _scopeStorage.Add(ActionLogicState.GetDefaultLogicState());

        return Task.CompletedTask;
    }
}