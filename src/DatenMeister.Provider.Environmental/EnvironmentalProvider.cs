using System.Collections;
using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.Environmental;

/// <summary>
/// Gets the environmental provider
/// </summary>
public class EnvironmentalProvider : IProviderLoader
{
    public IWorkspaceLogic? WorkspaceLogic { get; set; }
    public IScopeStorage? ScopeStorage { get; set; }

    /// <summary>
    /// Loads a provider with the given configuration and extent creation flags.
    /// </summary>
    /// <param name="configuration">The configuration defining the provider's settings and behavior.</param>
    /// <param name="extentCreationFlags">The flags indicating how the extent should be created or loaded.</param>
    /// <return>Returns information about the loaded provider, including the provider instance and configuration information.</return>
    public async Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        return await Task.Run(() =>
        {
            var provider = new InMemoryProvider();

            var variables = Environment.GetEnvironmentVariables();
            foreach (var pair in variables.OfType<DictionaryEntry>().OrderBy(x => x.Key))
            {
                var value = new InMemoryObject(provider, _CommonTypes.TheOne.OSIntegration.__EnvironmentalVariable.Uri);
                value.SetProperty(_CommonTypes._OSIntegration._EnvironmentalVariable.name, pair.Key);
                value.SetProperty(_CommonTypes._OSIntegration._EnvironmentalVariable.value, pair.Value);
                value.Id = pair.Key.ToString();
                provider.AddElement(value);
            }

            return new LoadedProviderInfo(provider);
        });
    }

    /// <summary>
    /// No storage possible
    /// </summary>
    /// <param name="extent"></param>
    /// <param name="configuration"></param>
    public Task StoreProvider(IProvider extent, IElement configuration)
    {
        return Task.CompletedTask;
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new()
    {
        IsPersistant = false,
        AreChangesPersistant = false
    };
}