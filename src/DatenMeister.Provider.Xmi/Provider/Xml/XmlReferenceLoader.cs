using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.Xmi.Provider.Xml
{
    public class XmlReferenceLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var filePath =
                configuration.getOrDefault<string>(
                    _DatenMeister._ExtentLoaderConfigs._XmlReferenceLoaderConfig.filePath);

            // Now load the stuff
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, ScopeStorage);
            var simpleLoader = new XmlToExtentConverter(configuration);
            simpleLoader.Convert(XDocument.Load(filePath), extent);

            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // No storing at the moment
        }

        public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new ProviderLoaderCapabilities()
        {
            IsPersistant = true,
            AreChangesPersistant = false
        };
    }
}