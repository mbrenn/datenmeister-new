using System;
using System.IO;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.XMI.EMOF;

namespace DatenMeister.XMI.ExtentStorage
{
    [ConfiguredBy(typeof(XmiStorageConfiguration))]
    public class XmiStorage : IExtentStorage
    {
        private readonly IWorkspaceCollection _workspaceCollection;

        public XmiStorage()
        {
            
        }

        public XmiStorage(IWorkspaceCollection workspaceCollection)
        {
            _workspaceCollection = workspaceCollection;
        }

        public IUriExtent LoadExtent(ExtentStorageConfiguration configuration, bool createAlsoEmpty = false)
        {
            var xmiConfiguration = (XmiStorageConfiguration) configuration;

            XDocument xmlDocument;
            if (!File.Exists(xmiConfiguration.Path))
            {
                if (createAlsoEmpty)
                {
                    // We need to create an empty Xmi file... Not the best thing at the moment, but we try it. 
                    xmlDocument = new XDocument(
                        new XElement(XmlUriExtent.DefaultRootNodeName));
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

            var result = new XmlUriExtent(xmlDocument, xmiConfiguration.ExtentUri)
            {
                Workspaces = _workspaceCollection
            };

            return result;
        }

        public void StoreExtent(IUriExtent extent, ExtentStorageConfiguration configuration)
        {
            var xmiConfiguration = configuration as XmiStorageConfiguration;
            if (xmiConfiguration != null)
            {
                var xmlExtent = extent as XmlUriExtent;
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