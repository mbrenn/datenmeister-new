using System;
using System.IO;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.XMI.EMOF;

namespace DatenMeister.XMI.ExtentStorage
{
    public class XmiStorage  : IExtentStorage
    {
        public IUriExtent LoadExtent(ExtentStorageConfiguration configuration)
        {
            var xmiConfiguration = (XmiStorageConfiguration) configuration;
            if (!File.Exists(xmiConfiguration.Path))
            {
                throw new InvalidOperationException(
                    $"File not found: {xmiConfiguration.Path}");
            }

            var xmlDocument = XDocument.Load(xmiConfiguration.Path);
            return new XmlUriExtent(xmlDocument, xmiConfiguration.ExtentUri);
        }

        public void StoreExtent(IUriExtent extent, ExtentStorageConfiguration configuration)
        {
            var xmiConfiguration = (XmiStorageConfiguration)configuration;

            var xmlExtent = extent as XmlUriExtent;
            if (xmlExtent == null)
            {
                throw new InvalidOperationException("Only XmlUriExtents are supported");
            }

            using (var fileStream = File.OpenWrite(xmiConfiguration.Path))
            {
                xmlExtent.Document.Save(fileStream);
            }
        }
    }
}