using System;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.XMI.EMOF
{
    [AssignFactoryForExtentType(typeof(XmlFactory))]
    public class XmlUriExtent : IUriExtent
    {
        public const string DefaultRootNodeName = "xmi";
        public const string DefaultElementNodeName = "item";
        private readonly string _urlPropertyName = "uri";

        private readonly XDocument _document;
        private readonly XElement _rootNode;

        internal XDocument Document => _document;

        public IWorkspaceLogic Workspaces { get; set; }


        private readonly ExtentUrlNavigator<XmlElement> _navigator;

        public XmlUriExtent(IWorkspaceLogic workspaces, string uri, string rootNodeName = DefaultRootNodeName)
        {
            if (workspaces == null) throw new ArgumentNullException(nameof(workspaces));
            Workspaces = workspaces;
            _document = new XDocument();
            _rootNode = new XElement(rootNodeName);
            _document.Add(_rootNode);
            _rootNode.SetAttributeValue(_urlPropertyName, uri);
            _navigator = new ExtentUrlNavigator<XmlElement>(this);
        }

        public XmlUriExtent(IWorkspaceLogic workspaces, XDocument document, string uri, string rootNodeName = DefaultRootNodeName)
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
            _navigator = new ExtentUrlNavigator<XmlElement>(this);
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return new XmlReflectiveSequence(this, _rootNode, DefaultElementNodeName);
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
            return _navigator.uri(element);
        }

        /// <summary>
        /// Gets the element by the uri
        /// </summary>
        /// <param name="uri">Uri of the element to be retrieved</param>
        /// <returns>The found element</returns>
        public IElement element(string uri)
        {
            return _navigator.element(uri);
        }
    }
}