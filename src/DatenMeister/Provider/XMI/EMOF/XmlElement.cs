using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.Standards;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.XMI.EMOF
{
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmlElement : IProviderObject
    {
        public static readonly XName TypeAttribute = Namespaces.Xmi + "type";

        private readonly XElement _node;

        public XElement XmlNode => _node;

        /// <summary>
        /// Gets the id of the XmlElement
        /// </summary>
        public string Id
        {
            get { return XmiId.Get(_node); }
            set { XmiId.Set(_node, value); }
        }

        /// <summary>
        /// Initializes a new instance of the XmlElement class.
        /// </summary>
        /// <param name="node">Node to be used</param>
        /// <param name="extent">Extent to be set</param>
        public XmlElement(XElement node, XmlUriExtent extent)
        {
            Debug.Assert(node != null, "node != null");
            _node = node;
            Provider = extent;

            // Checks, if an id is given. if not. set it. 
            if (!XmiId.HasId(node))
            {
                XmiId.Set(node, Guid.NewGuid().ToString());
            }
        }

        public override int GetHashCode()
        {
            return _node.GetHashCode();
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

            throw new InvalidOperationException(
                $"Only strings as properties are supported at the moment. Type is: {property.GetType()}");
        }

        private XElement ConvertValueAsXmlObject(string property, object value)
        {
            var valueAsXmlObject = value as XmlElement;
            if (valueAsXmlObject != null)
            {
                valueAsXmlObject.XmlNode.Name = property;
                return valueAsXmlObject.XmlNode;
            }

            /*var valueAsElement = value as IElement;
            if (valueAsElement != null)
            {
                var copier = new ObjectCopier(new XmlFactory { Owner = _extent, ElementName = _propertyName });
                return ((XmlElement) copier.Copy(valueAsElement)).XmlNode;
            }*/

            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return new XElement(property, value.ToString());
            }

            throw new InvalidOperationException("Value is not an XmlObject or an IElement: " + value);
        }

        /// <inheritdoc />
        public IProvider Provider { get; }

        /// <inheritdoc />
        public string MetaclassUri {
            get
            {
                return _node.Attribute(TypeAttribute)?.Value;
            }
            set
            {
                _node.SetAttributeValue(TypeAttribute, value);
            }
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return _node.Attribute(propertyAsString) != null || _node.Elements(propertyAsString).Any();
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            // Check, if there are subelements as the given property
            if (_node.Elements(propertyAsString).Any())
            {
                var list = new List<object>();
                foreach (var element in _node.Elements(propertyAsString))
                {
                    list.Add(new XmlElement(element, (XmlUriExtent) Provider));
                }

                return list;
            }

            // Check, if there is the attribute, otherwise null
            var attribute = _node.Attribute(propertyAsString);
            if (attribute != null)
            {
                return attribute.Value;
            }

            // For unknown objects, return an empty enumeration which will then be converted to an Reflective Sequence
            return new List<object>();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            foreach (var attribute in _node.Attributes())
            {
                var xmlNamespace = attribute.Name.Namespace;
                if (xmlNamespace == Namespaces.Xmi || xmlNamespace == Namespaces.XmlNamespace)
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

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            _node.Attributes(property).FirstOrDefault()?.Remove();
            foreach (var x in _node.Elements(property).ToList())
            {
                x.Remove();
            }

            return true;
        }

        /// <inheritdoc />
        public void SetProperty(string property, object value)
        {
            if (value == null)
            {
                DeleteProperty(property);
                return;
            }

            var propertyAsString = ReturnObjectAsString(property);

            var elementAsXml = value as XmlElement;
            if (elementAsXml != null)
            {
                elementAsXml.XmlNode.Name = property;
                _node.Add(elementAsXml._node);
            }
            else
            {
                DeleteProperty(property);
                _node.SetAttributeValue(propertyAsString, ReturnObjectAsString(value));
            }
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            _node.Attribute(property)?.Remove();
            _node.Elements(property)?.Remove();
        }

        /// <summary>
        /// Gets the size of all elements of a property, if that is an enumeration
        /// </summary>
        /// <param name="property">Property to be queried</param>
        /// <returns>The size of the list</returns>
        private int GetSizeOfList(string property)
        {
            return _node.Elements(property).Count();
        }

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index = -1)
        {
            if (index == GetSizeOfList(property) || index == -1)
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(property, value);
                _node.Add(valueAsXmlObject);
            }
            else
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(property, value);
                var addedBefore = _node.Elements(property).ElementAt(index);
                addedBefore.AddBeforeSelf(valueAsXmlObject);
            }

            return true;
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            if (value is XmlElement)
            {
                var valueAsXmlElement = value as XmlElement;

                foreach (var subElement in _node.Elements(property))
                {
                    if (XmiId.Get(subElement) == XmiId.Get(valueAsXmlElement.XmlNode))
                    {
                        subElement.Remove();
                        return true;
                    }
                }
            }
            else
            {
                var valueAsString = ReturnObjectAsString(value);
                foreach (var subElement in _node.Elements(property))
                {
                    if (subElement.Value.Equals(valueAsString))
                    {
                        subElement.Remove();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}