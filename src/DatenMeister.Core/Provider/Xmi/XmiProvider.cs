﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;

namespace DatenMeister.Core.Provider.Xmi
{
    /// <summary>
    /// Defines the provider for xml manipulation
    /// </summary>
    public class XmiProvider : IProvider, IHasUriResolver, IProviderSupportFunctions
    {
        /// <summary>
        /// Gets the value whether the same xmiprovider instance shall always be used
        /// for the same XmlNode. 
        /// </summary>
        public const bool ConfigurationUniqueXmiProviderObjects = true;
        
        /// <summary>
        /// Defines the name of the element
        /// </summary>
        public string ElementName { get; set; } = DefaultElementNodeName;

        public const string DefaultRootNodeName = "xmi";

        private const string DefaultElementNodeName = "item";

        private readonly XDocument _document;
        private readonly XElement _rootNode;

        public XDocument Document => _document;

        private static readonly XNamespace XDatenMeisterNamespace = "http://datenmeister.net/";

        private static readonly XName XMetaXmlNodeName = XDatenMeisterNamespace + "meta";

        internal readonly object LockObject = new object(); 
        
        internal readonly Dictionary<string, string> NormalizationCache = new Dictionary<string, string>();
        
        /// <summary>
        /// Stores the cached provider objects to be sure that the same XmiProvider object will be used
        /// when the same XElement is retrieved
        /// </summary>
        private readonly Dictionary<XElement, XmiProviderObject> _providerObjectCache 
            = new Dictionary<XElement, XmiProviderObject>();

        /// <summary>
        /// Gets or sets the uri resolver for this provider. Will be used to figure out information
        /// about the meta classes
        /// </summary>
        public IUriResolver? UriResolver { get; set; }

        public XmiProvider( /*string rootNodeName = DefaultRootNodeName*/)
        {
            var rootNodeName = DefaultRootNodeName;
            _document = new XDocument();
            _rootNode = new XElement(rootNodeName);
            _document.Add(_rootNode);
            /*_rootNode.SetAttributeValue(_urlPropertyName, uri);*/

            CreateProviderSupportFunctions();
        }

        public XmiProvider(XDocument document /*, string rootNodeName = DefaultRootNodeName*/)
        {
            var rootNodeName = document.Elements().First().Name;

            _document = document;
            _rootNode = _document.Element(rootNodeName) ?? throw new InvalidOperationException("Xml Document inconclusive");

            if (_rootNode == null)
            {
                throw new InvalidOperationException($"The given document does not have a root node called {rootNodeName}.");
            }
            
            CreateProviderSupportFunctions();
        }

        private void CreateProviderSupportFunctions()
        {
            _supportFunctions = new ProviderSupportFunctions
            {
                QueryById = (id) =>
                {
                    var result = FindById(id);
                    if (result == null) return null;

                    return CreateProviderObject(result);
                }
            };
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
        public IProviderObject CreateElement(string? metaClassUri)
        {
            var node = new XElement(ElementName);
            if (!string.IsNullOrEmpty(metaClassUri))
            {
                node.Add(new XAttribute(XmiProviderObject.TypeAttribute, metaClassUri));
            }

            return CreateProviderObject(node);
        }

        /// <summary>
        /// Creates a new provider object for the xmiprovider
        /// </summary>
        /// <param name="xmlElement">Xml element storing the data</param>
        /// <returns>Created ProviderObject</returns>
        public XmiProviderObject CreateProviderObject(XElement xmlElement)
        {
#pragma warning disable 162
            if (ConfigurationUniqueXmiProviderObjects)
            {
                lock (_providerObjectCache)
                {
                    if (_providerObjectCache.TryGetValue(xmlElement, out var result))
                    {
                        return result;
                    }

                    result = XmiProviderObject.Create(xmlElement, this);
                    _providerObjectCache[xmlElement] = result;

                    return result;
                }
            }
            else
            {
                return XmiProviderObject.Create(xmlElement, this);
                
            }
#pragma warning restore 162
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject? valueAsObject, int index = -1)
        {
            lock (LockObject)
            {
                if (valueAsObject is XmiProviderObject providerObject)
                {
                    _rootNode.Add(providerObject.XmlNode);
                }
            }
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            lock (LockObject)
            {
                var found = FindById(id);
                if (found != null)
                {
                    found.Remove();
                    return true;
                }

                return false;
            }
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            lock (LockObject)
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
        }

        /// <inheritdoc />
        public IProviderObject? Get(string? id)
        {
            if (id == null)
            {
                return CreateProviderObject(GetMetaNode());
            }
            
            lock (LockObject)
            {
                var result = FindById(id);
                return result == null ? null : CreateProviderObject(result);
            }
        }

        /// <summary>
        /// Defines the cache
        /// </summary>
        private readonly Dictionary<string, XElement> _cache = new Dictionary<string, XElement>();
        
        /// <summary>
        /// Finds a certain Xml Element by its id
        /// </summary>
        /// <param name="id">Id to be queried</param>
        /// <returns>The found element</returns>
        private XElement? FindById(string id)
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(id, out var element))
                {
                    if (XmiId.Get(element) == id)
                    {
                        return element;
                    }

                    _cache.Remove(id);
                }

                lock (LockObject)
                {
                    foreach (var x in _rootNode.Descendants())
                    {
                        var foundId = XmiId.Get(x);
                        if (foundId != null)
                            _cache[foundId] = x;
                    }
                }

                return _cache.TryGetValue(id, out var element2) ? element2 : null;
            }
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            List<XElement> rootObjects;
            lock (LockObject)
            {
                rootObjects = _rootNode.Elements().ToList();
            }

            foreach (var element in rootObjects)
            {
                if (element.Name.Namespace == XDatenMeisterNamespace)
                {
                    continue;
                }

                yield return CreateProviderObject(element);
            }
        }

        /// <summary>
        /// Gets the capabilities of the provider
        /// </summary>
        /// <returns></returns>
        public ProviderCapability GetCapabilities() =>
            ProviderCapabilities.StoreMetaDataInExtent;

        /// <summary>
        /// Defines the interface
        /// </summary>
        private ProviderSupportFunctions? _supportFunctions;

        /// <summary>
        /// Gets the support functions
        /// </summary>
        public ProviderSupportFunctions ProviderSupportFunctions => _supportFunctions
                                                                    ?? throw new NotSupportedException(
                                                                        "Should not happen");

        public static XElement GetMetaNodeFromFile(string filePath)
        {
            var document = XDocument.Load(filePath);
            var provider = new XmiProvider(document);
            return provider.GetMetaNode();
        }
    }
}