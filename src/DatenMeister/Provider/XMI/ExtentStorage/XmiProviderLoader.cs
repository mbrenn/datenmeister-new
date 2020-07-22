using System;
using System.IO;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    [ConfiguredBy(typeof(XmiStorageLoaderConfig))]
    public class XmiProviderLoader : IProviderLoader
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(XmiProviderLoader));

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            var xmiConfiguration = (XmiStorageLoaderConfig) configuration;

            XDocument xmlDocument;
            if (!File.Exists(xmiConfiguration.filePath) || extentCreationFlags == ExtentCreationFlags.CreateOnly)
            {
                if (extentCreationFlags != ExtentCreationFlags.LoadOnly)
                {
                    xmlDocument = CreateEmptyXmiDocument(xmiConfiguration);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"File not found: {xmiConfiguration.filePath}");
                }
            }
            else
            {
                try
                {
                    xmlDocument = XDocument.Load(xmiConfiguration.filePath);
                }
                catch (Exception exc)
                {
                    Logger.Warn(exc.ToString());
                    xmlDocument = CreateEmptyXmiDocument(xmiConfiguration);
                }
            }

            return new LoadedProviderInfo(new XmiProvider(xmlDocument));
        }

        /// <summary>
        /// Creates an empty Xmi document as given by the configuration
        /// </summary>
        /// <param name="xmiLoaderConfig">Xmi Configuration being used</param>
        /// <returns>Found XDocument</returns>
        private static XDocument CreateEmptyXmiDocument(XmiStorageLoaderConfig xmiLoaderConfig)
        {
            // Creates directory if necessary
            var directoryPath = Path.GetDirectoryName(xmiLoaderConfig.filePath)
                                ?? throw new InvalidOperationException("directoryPath is null");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // We need to create an empty Xmi file... Not the best thing at the moment, but we try it.
            var xmlDocument = new XDocument(
                new XElement(XmiProvider.DefaultRootNodeName));

            // Try to create file, to verify that file access and other activities are given
            xmlDocument.Save(xmiLoaderConfig.filePath);
            return xmlDocument;
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            if (configuration is XmiStorageLoaderConfig xmiConfiguration)
            {
                if (!(extent is XmiProvider xmlExtent))
                    throw new InvalidOperationException("Only XmlUriExtents are supported");

                // Deletes existing file
                if (File.Exists(xmiConfiguration.filePath))
                    File.Delete(xmiConfiguration.filePath);

                // Loads existing file
                using (var fileStream = File.OpenWrite(xmiConfiguration.filePath))
                {
                    xmlExtent.Document.Save(fileStream);
                }
            }
            else
            {
                throw new ArgumentException("Configuration is of an unknown type");
            }
        }
    }
}