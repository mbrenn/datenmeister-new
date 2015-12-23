using System;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.XMI.EMOF
{
    public class XmlUriExtent : IUriExtent
    {
        private readonly string _urlPropertyName = "uri";

        private XDocument _document;
        private XElement _rootNode;

        public XmlUriExtent(string uri, string rootNodeName = "items")
        {
            _document = new XDocument();
            _rootNode = new XElement(rootNodeName);
            _document.Add(_rootNode);
            _rootNode.SetAttributeValue(_urlPropertyName, uri);
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return new XmlReflectiveSequence(_rootNode);
        }

        public string contextURI()
        {
            return _rootNode.Attribute("uri").Value;
        }

        public string uri(IElement element)
        {
            var xmlElement = element as XmlElement;
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement), "given element is not an XmlElement");
            }

            var hasId = (IHasId) xmlElement;
            return $"{contextURI()}#{hasId.Id}";
        }

        public IElement element(string uri)
        {
            // Split in #
            var posHash = uri.IndexOf('#');
            if (posHash == -1)
            {
                return null;
            }

            var extentUrl = uri.Substring(0, posHash);
            var id = uri.Substring(posHash + 1);

            if (extentUrl != contextURI())
            {
                // Not in this extent
                return null;
            }

            foreach (var innerElement in elements().Cast<XmlElement>())
            {
                if (((IHasId) innerElement).Id.ToString() == id)
                {
                    return innerElement;
                }
            }

            // Not found
            return null;
        }
    }
}