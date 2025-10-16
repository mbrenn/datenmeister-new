using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.Core.TypeIndexAssembly;


public class TypeIndexPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public IScopeStorage ScopeStorage { get; set; } = scopeStorage;

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.BeforeBootstrapping:
                ScopeStorage.Add(new TypeIndexStore());
                break;
        }

        return Task.CompletedTask;
    }
}