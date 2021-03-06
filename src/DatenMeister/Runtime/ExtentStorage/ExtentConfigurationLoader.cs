﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load the extent storage out of a file.
    /// In addition, it will also use the ExtentManager class to load the actual data
    /// of the extents
    /// </summary>
    public partial class ExtentConfigurationLoader
    {
        /// <summary>
        /// Stores the mapper instance being used to find the allowed types
        /// </summary>
        private readonly ConfigurationToExtentStorageMapper _mapper;

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ExtentConfigurationLoader));

        /// <summary>
        /// Gets the information about the loaded extents,
        /// and filepath where to look after
        /// </summary>
        private ExtentStorageData ExtentStorageData { get; }

        /// <summary>
        /// Gets the extent manager being used to actual load an extent
        /// </summary>
        private ExtentManager ExtentManager { get; }

        public ExtentConfigurationLoader(
            IScopeStorage scopeStorage,
            ExtentManager extentManager,
            ConfigurationToExtentStorageMapper mapper)
        {
            _mapper = mapper;
            ExtentManager = extentManager;
            ExtentStorageData = scopeStorage.Get<ExtentStorageData>();
        }

        /// <summary>
        /// Loads the configuration of the extents and returns the configuration
        /// </summary>
        /// <returns>A touple of the extentloader config element
        /// and the xml node to the metaclass</returns>
        public List<Tuple<ExtentLoaderConfig, XElement>> GetConfigurationFromFile()
        {
            var path = ExtentStorageData.FilePath;
            var loaded = new List<Tuple<ExtentLoaderConfig, XElement>>();
            if (!File.Exists(path))
            {
                Logger.Info($"File for Extent not found: {path}");
            }
            else
            {
                var document = XDocument.Load(path);

                foreach (var xmlExtent in document.Elements("extents").Elements("extent"))
                {
                    var xmlConfig = xmlExtent.Element("config") ??
                                    throw new InvalidOperationException("extents::extent::config Xml node not found");
                    var configType = xmlConfig.Attribute("configType")?.Value ??
                                     throw new InvalidOperationException("configType not found");
                    configType = Migration.TranslateLegacyConfigurationType(configType);

                    // Gets the type of the configuration in the white list to avoid any unwanted security issue
                    var found = _mapper.ConfigurationTypes.FirstOrDefault(x =>
                        x.FullName?.ToLower() == configType.ToLower());
                    if (found == null)
                    {
                        Logger.Fatal($"Unknown Configuration Type: {configType}");
                        ExtentStorageData.FailedLoading = true;
                        throw new InvalidOperationException($"Unknown Configuration Type: {configType}");
                    }

                    xmlConfig.Name = found.Name; // We need to rename the element, so XmlSerializer can work with it
                    var serializer = new XmlSerializer(found);
                    var config = serializer.Deserialize(xmlConfig.CreateReader());

                    var xmlMeta = xmlExtent.Element("metadata");

                    loaded.Add(new Tuple<ExtentLoaderConfig, XElement>((ExtentLoaderConfig) config, xmlMeta));
                }
            }

            return loaded;
        }

        /// <summary>
        /// Stores the configuration of the extents into the given file
        /// </summary>
        public void StoreConfiguration()
        {
            // Skip saving, if loading has failed
            if (ExtentStorageData.FailedLoading)
            {
                Logger.Warn(
                    "No extents are stored due to the failure during loading. This prevents unwanted data loss due to a missing extent.");
                return;
            }

            var path = ExtentStorageData.FilePath;
            var document = new XDocument();
            var rootNode = new XElement("extents");
            document.Add(rootNode);

            foreach (var extent in ExtentStorageData.LoadedExtents)
            {
                var xmlExtent = new XElement("extent");

                // Stores the configuration
                var xmlData = SerializeToXElement(extent.Configuration ??
                                                  throw new InvalidOperationException("Configuration is not set"));
                
                xmlData.Name = "config";
                // Stores the .Net datatype to allow restore of the right element
                var fullName = extent.Configuration?.GetType().FullName;
                if (fullName == null) continue;
                
                xmlData.Add(new XAttribute("configType",fullName));
                xmlExtent.Add(xmlData);

                // Stores the metadata
                var xmlMetaData = new XElement(((MofExtent) extent.Extent).LocalMetaElementXmlNode)
                {
                    Name = "metadata"
                };

                xmlExtent.Add(xmlMetaData);
                rootNode.Add(xmlExtent);
            }

            document.Save(path);
        }

        /// <summary>
        /// Helper class to convert the given element into an Xml Element... Unfortunately, there is no direct way to create an Xml Element without using the XDocument
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static XElement SerializeToXElement(object o)
        {
            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                var serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(writer, o);
            }

            return doc.Root;
        }
    }
}