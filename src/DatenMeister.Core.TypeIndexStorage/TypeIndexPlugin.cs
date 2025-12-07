using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Plugins;

#pragma warning disable CS0162 // Unreachable code detected

namespace DatenMeister.Core.TypeIndexAssembly;


[PluginLoading(PluginLoadingPosition.BeforeBootstrapping|PluginLoadingPosition.AfterFinalizationOfIntegration|PluginLoadingPosition.AfterShutdownStarted)]
public class TypeIndexPlugin(IScopeStorage scopeStorage, IWorkspaceLogic workspaceLogic) : IDatenMeisterPlugin
{
    public IScopeStorage ScopeStorage { get; set; } = scopeStorage;
    
    public IWorkspaceLogic WorkspaceLogic { get; set; } = workspaceLogic;

    /// <summary>
    /// Gets or sets a configuration flag which may deactivate the full plugin on source-level
    /// </summary>
    public const bool IsActive = true;

    /// <summary>
    /// Gets a configuration flag indicating whether the listening mechanism is active
    /// </summary>
    public const bool IsListeningActive = true;

    private TypeIndexLogic? _logic;

    /// <summary>
    /// Starts the plugin execution based on the specified loading position.
    /// </summary>
    /// <param name="position">
    /// The position in the plugin loading sequence where this method is invoked.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation of starting the plugin.
    /// </returns>
    public Task Start(PluginLoadingPosition position)
    {
        if (!IsActive)
        {
            return Task.CompletedTask;
        }

        switch (position)
        {
            case PluginLoadingPosition.BeforeBootstrapping:
                ScopeStorage.Add(new TypeIndexStore());
                break;

            case PluginLoadingPosition.AfterFinalizationOfIntegration:
                // We have now loaded everything and let's start the indexing
                _logic = new TypeIndexLogic(WorkspaceLogic);
                _ = _logic.CreateIndexFirstTime();
                
                if(IsListeningActive)
                    _ = _logic.StartListening();
                
                break;
            case PluginLoadingPosition.AfterShutdownStarted:
                _logic?.StopListening();

                break;
        }

        return Task.CompletedTask;
    }
}