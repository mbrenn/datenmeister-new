using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.XMI.EMOF
{
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmlElement : IElement, IHasId
    {
        private static readonly XName _attributeNameForId = "id";

        private readonly XElement _node;

        public XElement XmlNode
        {
            get { return _node; }
        }

        public XmlElement(XElement node)
        {
            Debug.Assert(node != null, "node != null");
            _node = node;

            // Checks, if an id is given. if not. set it. 
            if (node.Attribute(_attributeNameForId) == null)
            {
                node.SetAttributeValue(_attributeNameForId, Guid.NewGuid().ToString());
            }
        }

        public string Id
        {
            get { return _node.Attribute(_attributeNameForId).Value; }
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

        public object get(object property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return _node.Attribute(propertyAsString)?.Value;
        }

        public bool isSet(object property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return _node.Attribute(propertyAsString) != null;
        }

        public void set(object property, object value)
        {
            var propertyAsString = ReturnObjectAsString(property);
            _node.SetAttributeValue(propertyAsString, ReturnObjectAsString(value));
        }

        public void unset(object property)
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
            else
            {
                throw new InvalidOperationException("Only strings as properties are supported at the moment");
            }
        }

        public IElement metaclass => null;

        public IElement getMetaClass()
        {
            return null;
        }

        public IElement container()
        {
            return null;
        }
    }
}