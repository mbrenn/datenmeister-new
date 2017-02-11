﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Provider.XMI.Standards;

namespace DatenMeister.Provider.XMI.EMOF
{
    /// <summary>
    /// Defines the provider for xml manipulation
    /// </summary>
    public class XmlUriExtent : IProvider
    {
        /// <summary>
        /// Defines the name of the element
        /// </summary>
        public string ElementName { get; set; } = DefaultElementNodeName;

        public const string DefaultRootNodeName = "xmi";
        public const string DefaultElementNodeName = "item";
        private readonly string _urlPropertyName = "uri";

        private readonly XDocument _document;
        private readonly XElement _rootNode;

        internal XDocument Document => _document;
        
        public XmlUriExtent(/*string rootNodeName = DefaultRootNodeName*/)
        {
            var rootNodeName = DefaultRootNodeName;
            _document = new XDocument();
            _rootNode = new XElement(rootNodeName);
            _document.Add(_rootNode);
            /*_rootNode.SetAttributeValue(_urlPropertyName, uri);*/
        }

        public XmlUriExtent(XDocument document/*, string rootNodeName = DefaultRootNodeName*/)
        {
            var rootNodeName = DefaultRootNodeName;
            _document = document;
            _rootNode = _document.Element(rootNodeName);

            if (_rootNode == null)
            {
                throw new InvalidOperationException($"The given document does not have a root node called {rootNodeName}.");
            }
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri)
        {
            var node = new XElement(ElementName);
            if (!string.IsNullOrEmpty(metaClassUri))
            {
                node.Add(new XAttribute(XmlElement.TypeAttribute, metaClassUri));
            }

            return new XmlElement(node, this);
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
            _rootNode.Add(((XmlElement) valueAsObject).XmlNode);
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
            _rootNode.RemoveAll();
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            if (id == null)
            {
                return new XmlElement(_rootNode, this);
            }

            var result = FindById(id);
            return result == null ? null : new XmlElement(result, this);
        }

        /// <summary>
        /// Finds a certain Xml Element by its id
        /// </summary>
        /// <param name="id">Id to be queried</param>
        /// <returns>The found element</returns>
        public XElement FindById(string id)
        {
            return _rootNode.Descendants().FirstOrDefault(x => XmiId.Get(x) == id);
        }
        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            foreach (var element in _rootNode.Elements())
            {
                yield return new XmlElement(element, this);
            }
        }
    }
}