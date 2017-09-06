using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Provider.XMI.Standards;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.XMI.EMOF
{
    /// <inheritdoc />
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmiProviderObject : IProviderObject
    {
        public static readonly XName TypeAttribute = Namespaces.Xmi + "type";

        /// <summary>
        /// Converts a property name to a property string used for references
        /// </summary>
        /// <param name="propertyName">The name of the property to be converted</param>
        /// <returns>The proeprty with added-ref</returns>
        private static string ConvertPropertyToReference(string propertyName)
        {
            return propertyName + "-ref";
        }

        /// <summary>
        /// Gets the Xml Node
        /// </summary>
        public XElement XmlNode { get; }

        /// <inheritdoc />
        /// <summary>
        /// Gets the id of the XmlElement
        /// </summary>
        public string Id
        {
            get => XmiId.Get(XmlNode);
            set => XmiId.Set(XmlNode, value);
        }

        /// <inheritdoc />
        public IProvider Provider { get; }

        /// <summary>
        /// Initializes a new instance of the XmlElement class.
        /// </summary>
        /// <param name="node">Node to be used</param>
        /// <param name="provider">Provider to be set</param>
        public XmiProviderObject(XElement node, XmiProvider provider)
        {
            XmlNode = node ?? throw new ArgumentNullException(nameof(node));
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));

            // Checks, if an id is given. if not. set it. 
            if (!XmiId.HasId(node))
            {
                XmiId.Set(node, XmiId.CreateNew());
            }
        }

        public override int GetHashCode()
        {
            return XmlNode.GetHashCode();
        }

        /// <summary>
        /// Converts the object to a string value
        /// </summary>
        /// <param name="value">Value to be converted to a string</param>
        /// <returns>Converted the value to text</returns>
        private string ReturnObjectAsString(object value)
        {
            if (value is string)
            {
                return value.ToString();
            }

            if (DotNetHelper.IsOfBoolean(value))
            {
                return value.ToString();
            }

            if (value is double valueAsDouble)
            {
                return valueAsDouble.ToString(CultureInfo.InvariantCulture);
            }

            if (DotNetHelper.IsOfNumber(value))
            {
                return value.ToString();
            }

            if (value is DateTime propertyAsDateTime)
            {
                return propertyAsDateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            throw new InvalidOperationException(
                $"Only strings as properties are supported at the moment. Type is: {value.GetType()}");
        }

        /// <summary>
        /// Converts the given value to an xml element.
        /// If the value is already an XmlElement, it will be reused, otherwise a new XmlNode will
        /// be created
        /// </summary>
        /// <param name="property">Property to be set</param>
        /// <param name="value">Value to be converted</param>
        /// <returns>The XmlNode reflecting the given element</returns>
        private XElement ConvertValueAsXmlObject(string property, object value)
        {
            if (value is XmiProviderObject valueAsXmlObject)
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
                return new XElement(property, DotNetHelper.ToString(value));
            }

            throw new InvalidOperationException("Value is not an XmlObject or an IElement: " + value);
        }

        /// <inheritdoc />
        public string MetaclassUri {
            get => XmlNode.Attribute(TypeAttribute)?.Value;
            set => XmlNode.SetAttributeValue(TypeAttribute, value);
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            return XmlNode.Attribute(propertyAsString) != null || XmlNode.Elements(propertyAsString).Any();
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            var propertyAsString = ReturnObjectAsString(property);
            // Check, if there are subelements as the given value
            if (XmlNode.Elements(propertyAsString).Any())
            {
                var list = new List<object>();
                foreach (var element in XmlNode.Elements(propertyAsString))
                {
                    list.Add(new XmiProviderObject(element, (XmiProvider) Provider));
                }

                return list;
            }

            // Check, if there is the attribute, otherwise null
            var attribute = XmlNode.Attribute(propertyAsString);
            if (attribute != null)
            {
                return attribute.Value;
            }

            var uriAttribute = XmlNode.Attribute(ConvertPropertyToReference(propertyAsString));
            if (uriAttribute != null)
            {
                return new UriReference(uriAttribute.Value);
            }

            // For unknown objects, return an empty enumeration which will then be converted to an Reflective Sequence
            return new List<object>();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            var result = new List<string>();
            foreach (var attribute in XmlNode.Attributes())
            {
                var xmlNamespace = attribute.Name.Namespace;
                if (xmlNamespace == Namespaces.Xmi || xmlNamespace == Namespaces.XmlNamespace)
                {
                    continue;
                }

                result.Add(attribute.Name.ToString());
            }

            foreach (var element in XmlNode.Elements().Distinct())
            {
                result.Add(element.Name.ToString());
            }

            return result.Distinct();
        }

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            XmlNode.Attributes(property).FirstOrDefault()?.Remove();
            foreach (var x in XmlNode.Elements(property).ToList())
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

            if (value is UriReference uriReference)
            {
                XmlNode.SetAttributeValue(ConvertPropertyToReference(propertyAsString), uriReference.Uri);
            }
            else if (value is XmiProviderObject elementAsXml)
            {
                // Includes the XmiProvider to the node. Will be added to the node
                elementAsXml.XmlNode.Name = property;
                XmlNode.Add(elementAsXml.XmlNode);
            }
            else
            {
                // Sets the property of the node...
                DeleteProperty(property);
                var xmlTextValue = ReturnObjectAsString(value);
                if (!string.IsNullOrEmpty(xmlTextValue))
                {
                    XmlNode.SetAttributeValue(propertyAsString, xmlTextValue);
                }
            }
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            XmlNode.Attribute(property)?.Remove();
            XmlNode.Elements(property).Remove();
        }

        /// <summary>
        /// Gets the size of all elements of a value, if that is an enumeration
        /// </summary>
        /// <param name="property">Property to be queried</param>
        /// <returns>The size of the list</returns>
        private int GetSizeOfList(string property)
        {
            return XmlNode.Elements(property).Count();
        }

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index = -1)
        {
            if (index == GetSizeOfList(property) || index == -1)
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(property, value);
                XmlNode.Add(valueAsXmlObject);
            }
            else
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(property, value);
                var addedBefore = XmlNode.Elements(property).ElementAt(index);
                addedBefore.AddBeforeSelf(valueAsXmlObject);
            }

            return true;
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            if (value is XmiProviderObject valueAsXmlElement)
            {
                foreach (var subElement in XmlNode.Elements(property))
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
                foreach (var subElement in XmlNode.Elements(property))
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