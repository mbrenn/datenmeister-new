using System;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.Xml
{
    [ConfiguredBy(typeof(XmlReferenceSettings))]
    public class XmlReferenceLoader : IProviderLoader
    {
        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            if (!(configuration is XmlReferenceSettings settings))
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