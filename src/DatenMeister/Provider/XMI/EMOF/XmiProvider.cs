using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.XMI.Standards;

namespace DatenMeister.Provider.XMI.EMOF
{
    /// <summary>
    /// Defines the provider for xml manipulation
    /// </summary>
    public class XmiProvider : IProvider, IHasUriResolver
    {
        /// <summary>
        /// Defines the name of the element
        /// </summary>
        public string ElementName { get; set; } = DefaultElementNodeName;

        public const string DefaultRootNodeName = "xmi";

        private const string DefaultElementNodeName = "item";

        private readonly XDocument _document;
        private readonly XElement _rootNode;

        public XElement RootNode => _rootNode;

        public XDocument Document => _document;

        private static readonly XNamespace XDatenMeisterNamespace = "http://datenmeister.net/";

        private static readonly XName XMetaXmlNodeName = XDatenMeisterNamespace + "meta";

        /// <summary>
        /// Gets or sets the uri resolver for this provider. Will be used to figure out information
        /// about the meta classes
        /// </summary>
        public IUriResolver UriResolver { get; set; }

        public XmiProvider( /*string rootNodeName = DefaultRootNodeName*/)
        {
            var rootNodeName = DefaultRootNodeName;
            _document = new XDocument();
            _rootNode = new XElement(rootNodeName);
            _document.Add(_rootNode);
            /*_rootNode.SetAttributeValue(_urlPropertyName, uri);*/
        }

        public XmiProvider(XDocument document /*, string rootNodeName = DefaultRootNodeName*/)
        {
            var rootNodeName = document.Elements().First().Name;

            _document = document;
            _rootNode = _document.Element(rootNodeName);

            if (_rootNode == null)
            {
                throw new InvalidOperationException($"The given document does not have a root node called {rootNodeName}.");
            }
        }

        /// <summary>
        /// Gets the meta node. If the meta node does not exist, create the meta node
        /// </summary>
        /// <returns>The returned meta node</returns>
        private XElement GetMetaNode()
        {
            var metaNode = _rootNode.Element(XMetaXmlNodeName);
            if (metaNode == null)
            {
                metaNode = new XElement(XMetaXmlNodeName);
                _rootNode.AddFirst(metaNode);
            }

            return metaNode;
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri)
        {
            var node = new XElement(ElementName);
            if (!string.IsNullOrEmpty(metaClassUri))
            {
                node.Add(new XAttribute(XmiProviderObject.TypeAttribute, metaClassUri));
            }

            return new XmiProviderObject(node, this);
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
            _rootNode.Add(((XmiProviderObject) valueAsObject).XmlNode);
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            var found = FindById(id);
            if (found != null)
            {
                found.Remove();
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            var found = new List<XElement>();

            foreach (var node in _rootNode.Elements())
            {
                if (node.Name.Namespace == XDatenMeisterNamespace)
                {
                    continue;
                }

                found.Add(node);
            }

            foreach (var node in found)
            {
                node.Remove();
            }
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            if (id == null)
            {
                return new XmiProviderObject(GetMetaNode(), this);
            }

            var result = FindById(id);
            return result == null ? null : new XmiProviderObject(result, this);
        }

        /// <summary>
        /// Finds a certain Xml Element by its id
        /// </summary>
        /// <param name="id">Id to be queried</param>
        /// <returns>The found element</returns>
        private XElement FindById(string id)
        {
            return _rootNode.Descendants().FirstOrDefault(x => XmiId.Get(x) == id);
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            foreach (var element in _rootNode.Elements())
            {
                if (element.Name.Namespace == XDatenMeisterNamespace)
                {
                    continue;
                }

                yield return new XmiProviderObject(element, this);
            }
        }

        /// <summary>
        /// Gets the capabilities of the provider
        /// </summary>
        /// <returns></returns>
        public ProviderCapability GetCapabilities() =>
            ProviderCapability.StoreMetaDataInExtent;
    }
}