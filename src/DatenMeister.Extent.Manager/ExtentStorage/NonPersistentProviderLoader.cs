using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Provider.Interfaces;

namespace DatenMeister.Extent.Manager.ExtentStorage;

/// <summary>
/// This is just a dummy class which allows to return a non-execution IProviderLoader instance, allowing to
/// remove a lot of nullability checks
/// </summary>
public class NonPersistentProviderLoader : IProviderLoader
{
    public IWorkspaceLogic? WorkspaceLogic { get; set; }
    public IScopeStorage? ScopeStorage { get; set; }
    public Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        throw new InvalidOperationException("I can never load, so I should not get called");
    }

    public Task StoreProvider(IProvider provider, IElement configuration)
    {
        return Task.CompletedTask;
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new()
    {
        AreChangesPersistant = false,
        IsPersistant = false
    };
}