using System;
using System.IO;
using System.Xml.Linq;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    [ConfiguredBy(typeof(XmiStorageConfiguration))]
    public class XmiStorage : IExtentLoader
    {
        private readonly IWorkspaceLogic _workspaceCollection;
        
        public XmiStorage(IWorkspaceLogic workspaceCollection)
        {
            _workspaceCollection = workspaceCollection ?? throw new ArgumentNullException(nameof(workspaceCollection));
        }

        public IProvider LoadExtent(ExtentLoaderConfig configuration, bool createAlsoEmpty = false)
        {
            var xmiConfiguration = (XmiStorageConfiguration) configuration;

            XDocument xmlDocument;
            if (!File.Exists(xmiConfiguration.Path))
            {
                if (createAlsoEmpty)
                {
                    // We need to create an empty Xmi file... Not the best thing at the moment, but we try it. 
                    xmlDocument = new XDocument(
                        new XElement(XmiProvider.DefaultRootNodeName));
                }
                else
                {
                    throw new InvalidOperationException(
                        $"File not found: {xmiConfiguration.Path}");
                }
            }
            else
            {
                xmlDocument = XDocument.Load(xmiConfiguration.Path);
            }

            var result = new XmiProvider(xmlDocument);

            return result;
        }

        public void StoreExtent(IProvider extent, ExtentLoaderConfig configuration)
        {
            var xmiConfiguration = configuration as XmiStorageConfiguration;
            if (xmiConfiguration != null)
            {
                var xmlExtent = extent as XmiProvider;
                if (xmlExtent == null)
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