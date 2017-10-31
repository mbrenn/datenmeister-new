﻿using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    [ConfiguredBy(typeof(XmiStorageConfiguration))]
    public class XmiStorage : IExtentLoader
    {
        public IProvider LoadExtent(ExtentLoaderConfig configuration, bool createAlsoEmpty = false)
        {
            var xmiConfiguration = (XmiStorageConfiguration) configuration;

            XDocument xmlDocument;
            if (!File.Exists(xmiConfiguration.Path))
            {
                if (createAlsoEmpty)
                {
                    xmlDocument = CreateEmptyXmiDocument(xmiConfiguration);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"File not found: {xmiConfiguration.Path}");
                }
            }
            else
            {
                try
                {
                    xmlDocument = XDocument.Load(xmiConfiguration.Path);
                }
                catch(Exception exc)
                {
                    Debug.WriteLine(exc.ToString());
                    xmlDocument = CreateEmptyXmiDocument(xmiConfiguration);
                }
            }

            return new XmiProvider(xmlDocument);
        }

        /// <summary>
        /// Creates an empty Xmi document as given by the configuration
        /// </summary>
        /// <param name="xmiConfiguration">Xmi Configuration being used</param>
        /// <returns>Found XDocument</returns>
        private static XDocument CreateEmptyXmiDocument(XmiStorageConfiguration xmiConfiguration)
        {
            // Creates directory if necessary
            var directoryPath = Path.GetDirectoryName(xmiConfiguration.Path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // We need to create an empty Xmi file... Not the best thing at the moment, but we try it. 
            var xmlDocument = new XDocument(
                new XElement(XmiProvider.DefaultRootNodeName));

            // Try to create file, to verify that file access and other activities are given
            File.WriteAllText(xmiConfiguration.Path, string.Empty);
            return xmlDocument;
        }

        public void StoreExtent(IProvider extent, ExtentLoaderConfig configuration)
        {
            if (configuration is XmiStorageConfiguration xmiConfiguration)
            {
                if (!(extent is XmiProvider xmlExtent))
                {
                    throw new InvalidOperationException("Only XmlUriExtents are supported");
                }

                // Deletes existing file
                if (File.Exists(xmiConfiguration.Path))
                {
                    File.Delete(xmiConfiguration.Path);
                }

                // Loads existing file
                using (var fileStream = File.OpenWrite(xmiConfiguration.Path))
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