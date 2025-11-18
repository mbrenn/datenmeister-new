using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Plugins;

#pragma warning disable CS0162 // Unreachable code detected

namespace DatenMeister.Core.TypeIndexAssembly;


[PluginLoading(PluginLoadingPosition.BeforeBootstrapping|PluginLoadingPosition.AfterLoadingOfExtents)]
public class TypeIndexPlugin(IScopeStorage scopeStorage, IWorkspaceLogic workspaceLogic) : IDatenMeisterPlugin
{
    public IScopeStorage ScopeStorage { get; set; } = scopeStorage;
    
    public IWorkspaceLogic WorkspaceLogic { get; set; } = workspaceLogic;

    /// <summary>
    /// Gets or sets a configuration flag which may deactivate the full plugin on source-level
    /// </summary>
    public const bool IsActive = true;

    public const bool IsListeningActive = true;

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

            case PluginLoadingPosition.AfterLoadingOfExtents:
                // We have now loaded everything and let's start the indexing
                var logic = new TypeIndexLogic(WorkspaceLogic);
                _ = logic.CreateIndexFirstTime();
                
                if(IsListeningActive)
                    _ = logic.StartListening();
                
                break;
        }

        return Task.CompletedTask;
    }
}