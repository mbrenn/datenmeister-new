using System;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Attributes;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.XMI.EMOF
{
    [AssignFactoryForExtentType(typeof(XmlFactory))]
    public class XmlUriExtent : IUriExtent
    {
        public const string DefaultRootNodeName = "xmi";
        private readonly string _urlPropertyName = "uri";

        private readonly XDocument _document;
        private readonly XElement _rootNode;

        internal XDocument Document => _document;

        public IWorkspaceCollection Workspaces { get; set; }

        public XmlUriExtent(IWorkspaceCollection workspaces, string uri, string rootNodeName = DefaultRootNodeName)
        {
            if (workspaces == null) throw new ArgumentNullException(nameof(workspaces));
            Workspaces = workspaces;
            _document = new XDocument();
            _rootNode = new XElement(rootNodeName);
            _document.Add(_rootNode);
            _rootNode.SetAttributeValue(_urlPropertyName, uri);
        }

        public XmlUriExtent(IWorkspaceCollection workspaces, XDocument document, string uri, string rootNodeName = DefaultRootNodeName)
        {
            if (workspaces == null) throw new ArgumentNullException(nameof(workspaces));
            Workspaces = workspaces;
            _document = document;
            _rootNode = _document.Element(rootNodeName);

            if (_rootNode == null)
            {
                throw new InvalidOperationException($"The given document does not have a root node called {rootNodeName}.");
            }
            
            _rootNode.SetAttributeValue(_urlPropertyName, uri);
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return new XmlReflectiveSequence(this, _rootNode);
        }

        public string contextURI()
        {
            var attribute = _rootNode.Attribute(_urlPropertyName);
            if (attribute == null)
            {
                throw new InvalidOperationException("Extent does not have a uri");
            }

            return _rootNode.Attribute(_urlPropertyName).Value;
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
                if (((IHasId) innerElement).Id == id)
                {
                    return innerElement;
                }
            }

            // Not found
            return null;
        }
    }
}