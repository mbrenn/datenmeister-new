using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.XMI.EMOF
{
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmlElement : IElement, IHasId, IHasExtent, IObjectAllProperties
    {
        public static readonly XName TypeAttribute = Namespaces.Xmi + "type";
        private static readonly XName AttributeNameForId = "id";

        private readonly XElement _node;

        public XElement XmlNode => _node;

        /// <summary>
        /// Stores the owning element of the element. 
        /// </summary>
        private readonly XmlElement _container;

        /// <summary>
        /// Stores the owning extent of the object.
        /// This object or the container should be set
        /// </summary>
        private readonly XmlUriExtent _owningExtent;

        /// <summary>
        /// Gets the id of the XmlElement
        /// </summary>
        public string Id => _node.Attribute(AttributeNameForId).Value;

        public XmlElement(XElement node, XmlElement container = null)
        {
            Debug.Assert(node != null, "node != null");
            _node = node;
            _container = container;

            // Checks, if an id is given. if not. set it. 
            if (node.Attribute(AttributeNameForId) == null)
            {
                node.SetAttributeValue(AttributeNameForId, Guid.NewGuid().ToString());
            }
        }

        public IExtent Extent => _owningExtent;

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
            return equals(obj);
        }

        public override int GetHashCode()
        {
            return _node.GetHashCode();
        }

        /// <summary>
        /// Gets all the properties that are set within the xml node
        /// </summary>
        /// <returns>Enumeration of objects</returns>
        public IEnumerable<string> getPropertiesBeingSet()
        {
            foreach (var attribute in _node.Attributes())
            {
                var xmlNamespace = attribute.Name.Namespace;
                if (xmlNamespace == Namespaces.Xmi ||
                    xmlNamespace == Namespaces.XmlNamespace)
                {
                    continue;
                }

                yield return attribute.Name.ToString();
            }

            foreach (var element in _node.Elements().Distinct())
            {
                yield return element.Name.ToString();
            }
        }

        public bool equals(object other)
        {
            Debug.Write($"this:{GetHashCode()} other:{other.GetHashCode()}");
            var otherAsXmlObject = other as XmlElement;

            // Simple implementation will look, if all the attributes are same
            if (_node.Attributes().Count() != otherAsXmlObject?._node.Attributes().Count())
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
            // Check, if there are subelements as the given property
            if (_node.Elements(propertyAsString).Any())
            {
                var xmiReflection = new XmlReflectiveSequence(_owningExtent, _node, property);
                return xmiReflection;
            }

            // Check, if there is the attribute, otherwise null
            return _node.Attribute(propertyAsString)?.Value;
        }

        public bool isSet(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return _node.Attribute(propertyAsString) != null || _node.Elements(propertyAsString).Any(); 
        }

        public void set(string property, object value)
        {
            if (value == null)
            {
                unset(property);
                return;
            }

            var propertyAsString = ReturnObjectAsString(property);

            if (DotNetHelper.IsOfMofObject(value))
            {
                // Remove attribute and remove element
                UnsetProperty(propertyAsString);
                var valueAsObject = value as IObject;

                var factory = new XmlFactory {Owner = _owningExtent, ElementName = propertyAsString};
                var objectCopier =
                    new ObjectCopier(factory);
                var targetElement = (XmlElement) objectCopier.Copy(valueAsObject);

                _node.Add(targetElement._node);
            }
            else if (DotNetHelper.IsOfEnumeration(value))
            {
                // Remove attribute and remove element
                UnsetProperty(propertyAsString);

                // Now create the elements
                var objectCopier =
                    new ObjectCopier(new XmlFactory { Owner = _owningExtent, ElementName = propertyAsString });

                var valueAsEnumeration = (IEnumerable) value;
                foreach (var innerValue in valueAsEnumeration)
                {
                    var innerValueAsIElement = innerValue as IObject;
                    if (innerValueAsIElement != null)
                    {
                        var copiedElement = (XmlElement) objectCopier.Copy(innerValueAsIElement);
                        _node.Add(copiedElement._node);
                    }
                    else
                    {
                        // We add them as real elements.
                        var element = new XElement(propertyAsString)
                        {
                            Value = ReturnObjectAsString(innerValue)
                        };

                        _node.Add(element);
                    }
                }
            }
            else
            {
                _node.SetAttributeValue(propertyAsString, ReturnObjectAsString(value));
            }
        }

        private void UnsetProperty(string propertyAsString)
        {
            _node.Attributes(propertyAsString).FirstOrDefault()?.Remove();
            foreach (var x in _node.Elements(propertyAsString).ToList())
            {
                x.Remove();
            }
        }

        public void unset(string property)
        {
            UnsetProperty(property);
        }

        private string ReturnObjectAsString(object property)
        {
            if (property is string)
            {
                return property.ToString();
            }

            if (DotNetHelper.IsOfBoolean(property))
            {
                return property.ToString();
            }

            if (DotNetHelper.IsOfNumber(property))
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
            var attribute = XmlNode.Attribute(TypeAttribute);
            if (attribute == null)
            {
                return null;
            }

            var extent = GetExtent();
            if (extent?.Workspaces == null)
            {
                // We have it, now try to find it. 
                // First of all, we need to get a list of all extents in the meta layer
                throw new InvalidOperationException("We have a metaclass but cannot find it due to missing extent");
            }

            return extent.Workspaces.FindItem(attribute.Value);
        }

        private XmlUriExtent GetExtent()
        {
            return _owningExtent ?? _container?.GetExtent();
        }
             

        public IElement container()
        {
            return null;
        }
    }
}