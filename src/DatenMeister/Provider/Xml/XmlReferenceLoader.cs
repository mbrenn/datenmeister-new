using System;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Provider.InMemory;
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

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            if (!(configuration is XmlReferenceLoaderConfig settings))
            {
                throw new InvalidOperationException("Given configuration is not of type ExcelReferenceSettings");
            }

            // Now load the stuff
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider);
            var simpleLoader = new XmlToExtentConverter(settings);
            simpleLoader.Convert(XDocument.Load(settings.filePath), extent);

            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            // No storing at the moment
        }
    }
}