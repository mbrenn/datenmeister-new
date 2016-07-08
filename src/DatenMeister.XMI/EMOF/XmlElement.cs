using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.XMI.EMOF
{
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmlElement : IElement, IHasId, IObjectKnowsExtent
    {
        public static readonly XName typeAttribute = Namespaces.Xmi + "type";
        private static readonly XName _attributeNameForId = "id";

        private readonly XElement _node;

        public XElement XmlNode => _node;

        /// <summary>
        /// Stores the owning element of the element. 
        /// </summary>
        private XmlElement _container;

        /// <summary>
        /// Stores the owning extent of the object.
        /// This object or the container should be set
        /// </summary>
        private XmlUriExtent _owningExtent;

        /// <summary>
        /// Gets the id of the XmlElement
        /// </summary>
        public string Id => _node.Attribute(_attributeNameForId).Value;

        public XmlElement(XElement node, XmlElement container = null)
        {
            Debug.Assert(node != null, "node != null");
            _node = node;
            _container = container;

            // Checks, if an id is given. if not. set it. 
            if (node.Attribute(_attributeNameForId) == null)
            {
                node.SetAttributeValue(_attributeNameForId, Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Initializes a new instance of the XmlElement class.
        /// </summary>
        /// <param name="node">Node to be used</param>
        /// <param name="extent">Extent to be set</param>
        public XmlElement(XElement node, XmlUriExtent extent)
            : this(node)
        {
            _owningExtent = extent;
        }

        public override bool Equals(object obj)
        {
            return @equals(obj);
        }

        public override int GetHashCode()
        {
            return _node.GetHashCode();
        }

        public bool equals(object other)
        {
            Debug.Write($"this:{GetHashCode()} other:{other.GetHashCode()}");
            var otherAsXmlObject = other as XmlElement;
            if (otherAsXmlObject == null)
            {
                return false;
            }

            // Simple implementation will look, if all the attributes are same
            if (_node.Attributes().Count() != otherAsXmlObject._node.Attributes().Count())
            {
                return false;
            }

            foreach (var attribute in _node.Attributes())
            {
                var otherAttribute = otherAsXmlObject._node.Attribute(attribute.Name);
                if (otherAttribute == null)
                {
                    return false;
                }

                if (otherAttribute.Value != attribute.Value)
                {
                    return false;
                }
            }

            // Ok, all the attributes are the same, we regard it as the same... until now
            return true;
        }

        public object get(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return _node.Attribute(propertyAsString)?.Value;
        }

        public bool isSet(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return _node.Attribute(propertyAsString) != null;
        }

        public void set(string property, object value)
        {
            var propertyAsString = ReturnObjectAsString(property);
            _node.SetAttributeValue(propertyAsString, ReturnObjectAsString(value));
        }

        public void unset(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            _node.SetAttributeValue(propertyAsString, null);
        }

        private string ReturnObjectAsString(object property)
        {
            if (property is string)
            {
                return property.ToString();
            }

            if (property is DateTime)
            {
                return ((DateTime) property).ToUniversalTime().ToString(CultureInfo.InvariantCulture);
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            throw new InvalidOperationException($"Only strings as properties are supported at the moment. Type is: {property.GetType()}");
        }

        public IElement metaclass => getMetaClass();

        public IElement getMetaClass()
        {
            // Find the metaclass uri.
            var attribute = XmlNode.Attribute(typeAttribute);
            if (attribute == null)
            {
                return null;
            }

            var extent = GetExtent();
            if (extent == null)
            {
                // We have it, now try to find it. 
                // First of all, we need to get a list of all extents in the meta layer
                throw new InvalidOperationException("We have a metaclass but cannot find it due to missing extent");
            }

            return extent.Workspaces?.FindItem(attribute.Value);
        }

        public XmlUriExtent GetExtent()
        {
            return _owningExtent ?? _container?.GetExtent();
        }
             

        public IElement container()
        {
            return null;
        }

        public IEnumerable<IExtent> Extents
        {
            get { yield return _owningExtent; }
        }
    }
}