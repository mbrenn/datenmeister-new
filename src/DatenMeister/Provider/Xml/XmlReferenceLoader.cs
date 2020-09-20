using System;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.Xml
{
    [ConfiguredBy(typeof(XmlReferenceLoaderConfig))]
    public class XmlReferenceLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var filePath = 
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._XmlReferenceLoaderConfig.filePath);
            
            // Now load the stuff
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider);
            var simpleLoader = new XmlToExtentConverter(configuration);
            simpleLoader.Convert(XDocument.Load(filePath), extent);

            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // No storing at the moment
        }
    }
}