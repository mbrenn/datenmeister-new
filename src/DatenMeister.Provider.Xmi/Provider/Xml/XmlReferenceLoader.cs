using System.Diagnostics;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;

namespace DatenMeister.Provider.Xmi.Provider.Xml;

public class XmlReferenceLoader : IProviderLoader, IProviderLoaderSupportsReload
{
    public IWorkspaceLogic? WorkspaceLogic { get; set; }
    public IScopeStorage? ScopeStorage { get; set; }

    public async Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        var filePath =
            configuration.getOrDefault<string>(
                _ExtentLoaderConfigs._XmlReferenceLoaderConfig.filePath);

        // Now load the stuff
        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, ScopeStorage);
        var simpleLoader = new XmlToExtentConverter(configuration);

        using var reader = new StreamReader(filePath);
        simpleLoader.Convert(await XDocument.LoadAsync(reader, LoadOptions.PreserveWhitespace, CancellationToken.None), extent);

        return new LoadedProviderInfo(provider);
    }

    public Task StoreProvider(IProvider extent, IElement configuration)
    {
        // No storing at the moment
        return Task.CompletedTask;
    }

    public async Task Reload(IProvider provider, IElement configuration)
    {
        var filePath =
            configuration.getOrDefault<string>(
                _ExtentLoaderConfigs._XmlReferenceLoaderConfig.filePath);

        // Now load the stuff
        var tempProvider = new InMemoryProvider();
        var tempExtent = new MofUriExtent(tempProvider, ScopeStorage);
        var simpleLoader = new XmlToExtentConverter(configuration);

        using var reader = new StreamReader(filePath);
        simpleLoader.Convert(await XDocument.LoadAsync(reader, LoadOptions.PreserveWhitespace, CancellationToken.None), tempExtent);

        var providerAsInMemory = provider as InMemoryProvider;
        Debug.Assert(providerAsInMemory != null, "The given provider is not of type InMemoryProvider");

        try
        {
            providerAsInMemory.Lock();
            providerAsInMemory.DeleteAllElements();
            foreach (var n in tempExtent.elements())
            {
                var asElement = n as MofElement;
                if (asElement != null)
                {
                    providerAsInMemory.AddElement(
                        asElement.ProviderObject);
                }
            }
        }
        finally
        {
            providerAsInMemory.Unlock();
        }
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new()
    {
        IsPersistant = true,
        AreChangesPersistant = false
    };
}