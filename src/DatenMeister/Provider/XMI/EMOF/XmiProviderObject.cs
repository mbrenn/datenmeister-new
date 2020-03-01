using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DatenMeister.Provider.XMI.Standards;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.XMI.EMOF
{
    /// <inheritdoc />
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmiProviderObject : IProviderObject, IProviderObjectSupportsListMovements
    {
        /// <summary>
        /// Checks whether the given property name is valid. This conversion is used to store the data into the xml
        /// </summary>
        /// <param name="property"></param>
        public static string NormalizePropertyName(string property)
        {
            if (property == "href")
            {
                return "_href";
            }

            return XmlConvert.EncodeLocalName(property);
        }

        /// <summary>
        /// Denormalizes the property names, so they can be stored into the xml.
        /// </summary>
        /// <param name="property">Property being used</param>
        /// <returns>The value is being sent to the provider</returns>
        public static string DenormalizePropertyName(string property)
        {
            if (property == "_href")
            {
                return "href";
            }

            return XmlConvert.DecodeName(property);
        }


        public static readonly XName TypeAttribute = Namespaces.Xmi + "type";

        /// <summary>
        /// Converts a property name to a property string used for references
        /// </summary>
        /// <param name="propertyName">The name of the property to be converted</param>
        /// <returns>The proeprty with added-ref</returns>
        private static string ConvertPropertyToReference(string propertyName) =>
            NormalizePropertyName(propertyName) + "-ref";

        /// <summary>
        /// Gets the Xml Node
        /// </summary>
        public XElement XmlNode { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets the id of the XmlElement
        /// </summary>
        public string? Id
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
                return propertyAsDateTime.ToString(CultureInfo.InvariantCulture);
            }

            if (value.GetType().IsEnum)
            {
                return value.ToString();
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            throw new InvalidOperationException(
                $"Only some types as properties are supported at the moment. Type is: {value.GetType()}");
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
                if (valueAsXmlObject.XmlNode.Parent != null)
                {
                    valueAsXmlObject = new XmiProviderObject(
                        new XElement(valueAsXmlObject.XmlNode),
                        (XmiProvider) Provider);
                }

                valueAsXmlObject.XmlNode.Name = property;
                
                // Creates an id, of the node does not have currently an ID
                if (XmiId.Get(valueAsXmlObject.XmlNode) == null)
                {
                    XmiId.Set(valueAsXmlObject.XmlNode, XmiId.CreateNew());
                }
                

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
                return new XElement(property, DotNetHelper.AsString(value));
            }

            // A uri reference creates an href element
            if (value is UriReference uriReference)
            {
                return new XElement(
                    property,
                    new XAttribute("href", uriReference.Uri));
            }

            throw new InvalidOperationException("Value is not an XmlObject or an IElement: " + (value ?? "'null'"));
        }

        /// <inheritdoc />
        public string? MetaclassUri
        {
            get => XmlNode.Attribute(TypeAttribute)?.Value;
            set => XmlNode.SetAttributeValue(TypeAttribute, value);
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            var normalizedPropertyName = NormalizePropertyName(property);

            var propertyAsString = ReturnObjectAsString(normalizedPropertyName);
            var propertyAsReference = ConvertPropertyToReference(property);

            return XmlNode.Attribute(propertyAsString) != null
                   || XmlNode.Attribute(propertyAsReference) != null
                   || XmlNode.Elements(propertyAsString).Any();
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            // Check, if there are subelements as the given value
            if (XmlNode.Elements(normalizePropertyName).Any())
            {
                var list = new List<object>();
                foreach (var element in XmlNode.Elements(normalizePropertyName))
                {
                    var hrefAttribute = element.Attribute("href");
                    if (hrefAttribute != null)
                    {
                        // Element is an href, so create a uri reference....
                        list.Add(new UriReference(hrefAttribute.Value));
                    }
                    else
                    {
                        if (!element.HasAttributes && !element.HasElements)
                        {
                            list.Add(element.Value);
                        }
                        else
                        {
                            list.Add(new XmiProviderObject(element, (XmiProvider) Provider));
                        }
                    }
                }

                return list;
            }

            // Check, if there is the attribute, otherwise null
            var attribute = XmlNode.Attribute(normalizePropertyName);
            if (attribute != null)
            {
                return attribute.Value;
            }

            var uriAttribute = XmlNode.Attribute(ConvertPropertyToReference(property));
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

                var attributeName = attribute.Name.ToString();
                if (attributeName.EndsWith("-ref"))
                {
                    attributeName = attributeName.Substring(0, attributeName.Length - "-ref".Length);
                }

                result.Add(attributeName);
            }

            foreach (var element in XmlNode.Elements().Distinct())
            {
                result.Add(element.Name.ToString());
            }

            return result.Distinct().Select(DenormalizePropertyName);
        }

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            XmlNode.Attributes(normalizePropertyName).FirstOrDefault()?.Remove();
            XmlNode.Attributes(ConvertPropertyToReference(property)).FirstOrDefault()?.Remove();
            
            foreach (var x in XmlNode.Elements(normalizePropertyName).ToList())
            {
                x.Remove();
            }

            return true;
        }

        /// <inheritdoc />
        public void SetProperty(string property, object? value)
        {
            var normalizePropertyName = NormalizePropertyName(property);
            DeleteProperty(property);
            
            if (value == null)
            {
                return;
            }

            if (value is UriReference uriReference)
            {
                XmlNode.SetAttributeValue(ConvertPropertyToReference(property), uriReference.Uri);
            }
            else if (value is XmiProviderObject elementAsXml)
            {
                // Includes the XmiProvider to the node. Will be added to the node
                elementAsXml.XmlNode.Name = normalizePropertyName;
                XmlNode.Add(elementAsXml.XmlNode);
            }
            else
            {
                // Sets the property of the node...
                DeleteProperty(property);
                var xmlTextValue = ReturnObjectAsString(value);
                if (!string.IsNullOrEmpty(xmlTextValue))
                {
                    XmlNode.SetAttributeValue(normalizePropertyName, xmlTextValue);
                }
            }
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            XmlNode.Attribute(normalizePropertyName)?.Remove();
            XmlNode.Elements(normalizePropertyName).Remove();
        }

        /// <summary>
        /// Gets the size of all elements of a value, if that is an enumeration
        /// </summary>
        /// <param name="property">Property to be queried</param>
        /// <returns>The size of the list</returns>
        private int GetSizeOfList(string property) =>
            XmlNode.Elements(property).Count();

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index = -1)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            if (index == GetSizeOfList(property) || index == -1)
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(normalizePropertyName, value);
                XmlNode.Add(valueAsXmlObject);
            }
            else
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(normalizePropertyName, value);
                var addedBefore = XmlNode.Elements(normalizePropertyName).ElementAt(index);
                addedBefore.AddBeforeSelf(valueAsXmlObject);
            }

            return true;
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            if (value is XmiProviderObject valueAsXmlElement)
            {
                foreach (var subElement in
                    XmlNode.Elements(normalizePropertyName)
                        .Where(subElement => XmiId.Get(subElement) == XmiId.Get(valueAsXmlElement.XmlNode)))
                {
                    subElement.Remove();
                    return true;
                }
            }
            else
            {
                var valueAsString = ReturnObjectAsString(value);
                foreach (var subElement in
                    XmlNode.Elements(normalizePropertyName)
                        .Where(subElement => subElement.Value.Equals(valueAsString)))
                {
                    subElement.Remove();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finds a certain list into the property list
        /// </summary>
        /// <param name="property">Property, which is selected</param>
        /// <param name="value">Value, which is required</param>
        /// <returns>The found element</returns>
        private XElement? FindInPropertyList(string property, object value)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            if (value is XmiProviderObject valueAsXmlElement)
            {
                foreach (var subElement in
                    XmlNode.Elements(normalizePropertyName)
                        .Where(subElement => XmiId.Get(subElement) == XmiId.Get(valueAsXmlElement.XmlNode)))
                {
                    return subElement;
                }
            }
            else
            {
                var valueAsString = ReturnObjectAsString(value);
                foreach (var subElement in
                    XmlNode.Elements(normalizePropertyName)
                        .Where(subElement => subElement.Value.Equals(valueAsString)))
                {
                    subElement.Remove();
                    return subElement;
                }
            }

            return null;
        }

        public bool HasContainer() =>
            XmlNode.Parent != null && XmlNode.Parent != XmlNode.Document?.Root;

        /// <summary>
        /// Gets the container of the object
        /// </summary>
        /// <returns></returns>
        public IProviderObject? GetContainer()
        {
            if (HasContainer())
            {
                return new XmiProviderObject(XmlNode.Parent, (XmiProvider) Provider);
            }

            return null;
        }

        public void SetContainer(IProviderObject value)
        {
            if (!(value is XmiProviderObject providerObject))
            {
                throw new ArgumentException($"{nameof(value)} is not of Type provider Object");
            }

            if (XmlNode.Parent != null)
            {
                XmlNode.Remove();
            }

            providerObject.XmlNode.Add(XmlNode);
        }

        public bool MoveElementUp(string property, object value)
        {
            var found = FindInPropertyList(property, value);
            if (found == null)
            {
                return false;
            }
            
            // Walk backwards until we find an element - ignore text nodes
            var previousNode = found.PreviousNode;
            while (previousNode != null && !(previousNode is XElement))
            {
                previousNode = previousNode.PreviousNode;
            }
            if (previousNode == null)
            {
                return true;
            }
            
            found.Remove();
            previousNode.AddBeforeSelf(found);

            return true;
        }

        public bool MoveElementDown(string property, object value)
        {
            var found = FindInPropertyList(property, value);
            if (found == null)
            {
                return false;
            }
            
            // Walk backwards until we find an element - ignore text nodes
            var nextNode = found.NextNode;
            while (nextNode != null && !(nextNode is XElement))
            {
                nextNode = nextNode.NextNode;
            }
            if (nextNode == null)
            {
                return true;
            }
            
            found.Remove();
            nextNode.AddAfterSelf(found);

            return true;
        }

        /// <summary>
        /// Converts the xmiproviderobject to string
        /// </summary>
        /// <returns>The converted object</returns>
        public override string ToString()
        {
            if (XmlNode == null)
            {
                return base.ToString();
            }

            var attribute = XmlNode.Attribute("id");
            if (attribute != null)
            {
                return attribute.Value;
            }
            
            attribute = XmlNode.Attribute("name");
            if (attribute != null)
            {
                return attribute.Value;
            }

            attribute = XmlNode.Attribute("title");
            if (attribute != null)
            {
                return attribute.Value;
            }
            
            return base.ToString();
        }
    }
}