using BurnSystems.Logging;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Models;

namespace DatenMeister.Core.Provider.InMemory;

public class InMemoryProviderLoader : IProviderLoader
{
    /// <summary>
    /// Sores the logger
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(InMemoryProviderLoader));

    public IWorkspaceLogic? WorkspaceLogic { get; set; }

    public IScopeStorage? ScopeStorage { get; set; }

    /// <summary>
    /// Just creates the provider for the memory
    /// </summary>
    /// <returns>The new InMemoryProvider</returns>
    public async Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        Logger.Info("InMemoryProvider is created");
            
        // Checks whether the item is a linked list
        IProvider provider;
        if (configuration.getOrDefault<bool>(
                _ExtentLoaderConfigs._InMemoryLoaderConfig.isLinkedList))
        {
            provider = new InMemoryProviderLinkedList();
        }
        else
        {
            provider = new InMemoryProvider();
        }
            
        return await Task.FromResult(new LoadedProviderInfo(provider));
    }

    public Task StoreProvider(IProvider extent, IElement configuration)
    {
        Logger.Info("Storing of in Memory Object is not possible");

        return Task.CompletedTask;
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } =
        new()
        {
            IsPersistant = false,
            AreChangesPersistant = false
        };
}