﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load the extent storage out of a file.
    /// In addition, it will also use the ExtentManager class to load the actual data
    /// of the extents
    /// </summary>
    public class ExtentConfigurationLoader
    {
        /// <summary>
        /// Gets the information about the loaded extents, 
        /// and filepath where to look after
        /// </summary>
        private ExtentStorageData ExtentStorageData { get; }

        /// <summary>
        /// Gets the extent manager being used to actual load an extent
        /// </summary>
        private IExtentManager ExtentManager { get; }

        public ExtentConfigurationLoader(
            ExtentStorageData extentStorageData,
            IExtentManager extentManager)
        {
            ExtentManager = extentManager;
            ExtentStorageData = extentStorageData;
        }

        /// <summary>
        /// Adds a type for the serialization of the configuration file since the 
        /// ExtentLoaderConfig instances might be derived
        /// </summary>
        /// <param name="type"></param>
        public void AddAdditionalType(Type type)
        {
            ExtentStorageData.AdditionalTypes.Add(type);
        }

        /// <summary>
        /// Gets the additional types for the xml parsing. 
        /// This method is called by the base class to support the loading if unknown extent types
        /// </summary>
        /// <returns>Array of additional types</returns>
        private Type[] GetAdditionalTypes()
        {
            return ExtentStorageData.AdditionalTypes.ToArray();
        }

        /// <summary>
        /// Loads all extents
        /// </summary>
        public void LoadAllExtents()
        {
            List<Tuple<ExtentLoaderConfig, XElement>> loaded = null;
            try
            {
                loaded = LoadConfiguration(ExtentStorageData.FilePath);
                
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception during loading of Extents: " + exc.Message);
            }

            if (loaded == null)
            {
                return;
            }

            foreach (var info in loaded)
            {
                try
                {
                    var extent = ExtentManager.LoadExtent(info.Item1, false);
                    if (info.Item2 != null)
                    {
                        ((MofExtent) extent).MetaXmiElement = info.Item2;
                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine($"Loading extent of {info.Item1.ExtentUri} failed: {exc.Message}");
                }
            }
        }

        /// <summary>
        /// Loads the configuration of the extents and returns the configuation
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Tuple<ExtentLoaderConfig, XElement>> LoadConfiguration(string path)
        {
            var loaded = new List<Tuple<ExtentLoaderConfig, XElement>>();
            var document = XDocument.Load(path);
            foreach (var xmlExtent in document.Elements("extents").Elements("extent"))
            {
                var xmlConfig = xmlExtent.Element("config");
                var configType = xmlConfig.Attribute("configType").Value;

                // Gets the type of the configuration in the white list to avoid any unwanted security issue
                var found = GetAdditionalTypes().FirstOrDefault(x => x.FullName == configType);
                if (found == null)
                {
                    throw new InvalidOperationException("Unknown configtype: " + configType);
                }

                xmlConfig.Name = found.Name; // We need to rename the element, so XmlSerializer can work with it
                var serializer = new XmlSerializer(found);
                var config = serializer.Deserialize(xmlConfig.CreateReader());


                var xmlMeta = xmlExtent.Element("metadata");

                loaded.Add(new Tuple<ExtentLoaderConfig, XElement>((ExtentLoaderConfig) config, xmlMeta));
            }

            return loaded;
        }
        
        /// <summary>
        /// Stores the configuration of the extents into the given file
        /// </summary>
        /// <param name="path">Path to be used to loaded the extent configuration</param>
        public void StoreConfiguration(string path)
        {
            var document = new XDocument();
            var rootNode = new XElement("extents");
            document.Add(rootNode);

            foreach (var extent in ExtentStorageData.LoadedExtents)
            {
                var xmlExtent = new XElement("extent");

                // Stores the configuration
                var xmlData = SerializeToXElement(extent.Configuration);
                xmlData.Name = "config";
                // Stores the .Net datatype to allow restore of the right element
                xmlData.Add(new XAttribute("configType", extent.Configuration.GetType().FullName));
                xmlExtent.Add(xmlData);

                // Stores the metadata
                var xmlMetaData = new XElement(((MofExtent) extent.Extent).MetaXmiElement)
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

        /// <summary>
        /// Stores all extents and the catalogue of the extents
        /// </summary>
        public void StoreAllExtents()
        {
            // Stores the extents themselves into the different database
            ExtentManager.StoreAll();
            
            //Save(ExtentStorageData.FilePath, toBeStored);
            StoreConfiguration(ExtentStorageData.FilePath);
        }
    }
}