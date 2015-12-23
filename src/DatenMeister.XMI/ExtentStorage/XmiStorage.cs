using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.XMI.EMOF;

namespace DatenMeister.XMI.ExtentStorage
{
    public class XmiStorage  : IExtentStorage<XmiStorageConfiguration>
    {
        public IUriExtent LoadExtent(XmiStorageConfiguration configuration)
        {
            if (!File.Exists(configuration.Path))
            {
                throw new InvalidOperationException(
                    $"File not found: {configuration.Path}");
            }

            var xmlDocument = XDocument.Load(configuration.Path);
            return new XmlUriExtent(xmlDocument, configuration.ExtentUri);
        }

        public void StoreExtent(IUriExtent extent, XmiStorageConfiguration configuration)
        {
            var xmlExtent = extent as XmlUriExtent;
            if (xmlExtent == null)
            {
                throw new InvalidOperationException("Only XmlUriExtents are supported");
            }

            using (var fileStream = File.OpenWrite(configuration.Path))
            {
                xmlExtent.Document.Save(fileStream);
            }
        }
    }
}