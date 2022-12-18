using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using DatenMeister.Core.Helper;

// ReSharper disable UnreachableCode

namespace DatenMeister.Core.Provider.Xmi
{
    /// <summary>
    /// Abstracts the IObject from EMOF
    /// </summary>
    public class XmiProviderObject : IProviderObject, IProviderObjectSupportsListMovements
    {
        /// <summary>
        /// Sets the name of the metaclass prefix
        /// </summary>
        public const string NodeMetaClassPrefix = "dm:///_xmi/node/";

        /// <summary>
        /// Stores the configuration whether a cache shall be used for the normalization function for the
        /// xmiProviders
        /// </summary>
        private const bool ConfigurationUseNormalizationCache = true;

        /// <summary>
        /// Gets or sets a value whether a cache shall be used for the property values.
        /// This value may only be set, of the UniqueXmiProvider Objects are set in XmiProvider
        /// </summary>
        private const bool ConfigurationUsePropertyCache = true;

        public static readonly XName TypeAttribute = Namespaces.Xmi + "type";

        /// <summary>
        /// Defines a cache to store the property values upon the xml
        /// </summary>
        private readonly Dictionary<string, object> _propertyCache = new();

        private readonly XmiProvider _xmiProvider;

        /// <summary>
        /// Initializes a new instance of the XmlElement class.
        /// </summary>
        /// <param name="node">Node to be used</param>
        /// <param name="provider">Provider to be set</param>
        public XmiProviderObject(XElement node, XmiProvider provider)
        {
            XmlNode = node ?? throw new ArgumentNullException(nameof(node));
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _xmiProvider = provider;

            lock (_xmiProvider.LockObject)
            {
                // Checks, if an id is given. if not. set it.
                if (!XmiId.HasId(node))
                {
                    XmiId.Set(node, XmiId.CreateNew());
                }
            }
        }

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
            get
            {
                lock (_xmiProvider.LockObject)
                {
                    return XmiId.Get(XmlNode);
                }
            }
            set
            {
                lock (_xmiProvider.LockObject)
                {
                    ClearPropertyProviderCache();
                    if (value == null || string.IsNullOrEmpty(value))
                    {
                        XmiId.Remove(XmlNode);
                    }
                    else
                    {
                        XmiId.Set(XmlNode, value);
                    }
                }
            }
        }

        /// <inheritdoc />
        public IProvider Provider { get; }

        /// <inheritdoc />
        public string? MetaclassUri
        {
            get
            {
                lock (_xmiProvider.LockObject)
                {
                    return GetMetaClassUri();
                }
            }

            set
            {
                lock (_xmiProvider.LockObject)
                {
                    XmlNode.SetAttributeValue(TypeAttribute, value);
                }
            }
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            lock (_xmiProvider.LockObject)
            {
#pragma warning disable 162
                if (ConfigurationUsePropertyCache)
                {
                    if (_propertyCache.ContainsKey(property))
                    {
                        return true;
                    }
                }
#pragma warning restore 162

                var normalizedPropertyName = NormalizePropertyName(property);

                var propertyAsString = ReturnObjectAsString(normalizedPropertyName) ?? string.Empty;
                var propertyAsReference = ConvertPropertyToReference(property);

                return XmlNode.Attribute(propertyAsString) != null
                       || XmlNode.Attribute(propertyAsReference) != null
                       || XmlNode.Elements(propertyAsString).Any();
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
        public object? GetProperty(string property, ObjectType objectType)
        {
            var value = GetPropertyInternal(property, objectType);
            return ObjectTypeConverter.Convert(value, objectType);
        }
        
        private object? GetPropertyInternal(string property, ObjectType objectType)
        {
            lock (_xmiProvider.LockObject)
            {
                var propertyCacheName = property + objectType;
                if (ConfigurationUsePropertyCache
                    && _propertyCache.TryGetValue(propertyCacheName, out var value))
                {
                    return value;
                }

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
                                // Element is a string, so add it
                                list.Add(element.Value);
                            }
                            else
                            {
                                list.Add(_xmiProvider.CreateProviderObject(element));
                            }
                        }
                    }

                    if (ConfigurationUsePropertyCache)
                        _propertyCache[propertyCacheName] = list;

                    return list;
                }

                // Check, if there is the attribute, otherwise null
                var attribute = XmlNode.Attribute(normalizePropertyName);
                if (attribute != null)
                {
                    if (objectType == ObjectType.Element)
                    {
                        // User requests an element, so return a Uri reference
                        var reference = new UriReference($"#{attribute.Value}");

                        if (ConfigurationUsePropertyCache)
                            _propertyCache[propertyCacheName] = reference;
                        return reference;
                    }

                    // User requests normal types

                    if (ConfigurationUsePropertyCache)
                        _propertyCache[propertyCacheName] = attribute.Value;
                    return attribute.Value;
                }

                var uriAttribute = XmlNode.Attribute(ConvertPropertyToReference(property));
                if (uriAttribute != null)
                {
                    var reference = new UriReference(uriAttribute.Value);

                    if (ConfigurationUsePropertyCache)
                        _propertyCache[propertyCacheName] = reference;
                    return reference;
                }

                if (objectType == ObjectType.ReflectiveSequence)
                {
                    // For unknown objects, return an empty enumeration which will then be converted to an Reflective Sequence
                    var empty = new List<object>();
                    _propertyCache[propertyCacheName] = empty;
                    return empty;
                }
                
                return null;
            }
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            lock (_xmiProvider.LockObject)
            {
                var result = new List<string>();
                foreach (var attribute in XmlNode.Attributes())
                {
                    // Skip the Xml-Namespace attributes
                    var xmlNamespace = attribute.Name.Namespace;
                    if (xmlNamespace == Namespaces.Xmi 
                        || xmlNamespace == Namespaces.XmlNamespace
                        || attribute.Name.LocalName == "xmlns")
                    {
                        continue;
                    }

                    // Handle the -ref attributes
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
        }

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

                var normalizePropertyName = NormalizePropertyName(property);

                XmlNode.Attributes(normalizePropertyName).FirstOrDefault()?.Remove();
                XmlNode.Attributes(ConvertPropertyToReference(property)).FirstOrDefault()?.Remove();

                foreach (var x in XmlNode.Elements(normalizePropertyName).ToList())
                {
                    x.Remove();
                }

                return true;
            }
        }

        /// <inheritdoc />
        public void SetProperty(string property, object? value)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

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
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

                var normalizePropertyName = NormalizePropertyName(property);

                XmlNode.Attribute(normalizePropertyName)?.Remove();
                XmlNode.Elements(normalizePropertyName).Remove();
            }
        }


        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index = -1)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

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
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

                var normalizePropertyName = NormalizePropertyName(property);

                if (value is XmiProviderObject valueAsXmlElement)
                {
                    // If the providers are the same, then use 
                    if (valueAsXmlElement.Provider == Provider)
                        foreach (var subElement in
                                 XmlNode.Elements(normalizePropertyName)
                                     .Where(subElement =>
                                         XmiId.Get(subElement) == XmiId.Get(valueAsXmlElement.XmlNode)))
                        {
                            subElement.Remove();
                            return true;
                        }

                    // Now try to go through the references, if there is no direct object with id included
                    foreach (var subElement in
                             XmlNode.Elements(normalizePropertyName)
                                 .Where(subElement =>
                                 {
                                     var href = subElement.Attribute("href")?.Value;
                                     if (href == null) return false;

                                     var posHash = href.IndexOf('#');
                                     if (posHash != -1) href = href[(posHash + 1)..];

                                     return href == XmiId.Get(valueAsXmlElement.XmlNode);
                                 }))
                    {
                        subElement.Remove();
                        return true;
                    }
                }
                else if (value is UriReference uriReference)
                {
                    foreach (var subElement in
                             XmlNode.Elements(normalizePropertyName)
                                 .Where(subElement =>
                                     subElement.Attribute("href")?.Value.Equals(uriReference.Uri) == true))
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
        }

        public bool HasContainer()
        {
            lock (_xmiProvider.LockObject)
            {
                return XmlNode.Parent != null && XmlNode.Parent != XmlNode.Document?.Root;
            }
        }

        /// <summary>
        /// Gets the container of the object
        /// </summary>
        /// <returns></returns>
        public IProviderObject? GetContainer()
        {
            if (HasContainer())
            {
                lock (_xmiProvider.LockObject)
                {
                    return XmlNode.Parent != null
                        ? _xmiProvider.CreateProviderObject(XmlNode.Parent)
                        : null;
                }
            }

            return null;
        }

        public void SetContainer(IProviderObject? value)
        {
            lock (_xmiProvider.LockObject)
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
        }

        public bool MoveElementUp(string property, object value)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

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
        }

        public bool MoveElementDown(string property, object value)
        {
            lock (_xmiProvider.LockObject)
            {
                ClearPropertyProviderCache();

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
        }

#pragma warning disable 162
        // ReSharper disable HeuristicUnreachableCode
        /// <summary>
        ///     Checks whether the given property name is valid. This conversion is used to store the data into the xml
        /// </summary>
        /// <param name="property"></param>
        private string NormalizePropertyName(string property)
        {
            if (ConfigurationUseNormalizationCache)
            {
                if (_xmiProvider.NormalizationCache.TryGetValue(property, out var value)) return value;

                value = property == "href" ? "_href"
                    : property == "xmlns" ? "_xmlns"
                    : XmlConvert.EncodeLocalName(property) ?? throw new InvalidOperationException("Should not happen");

                _xmiProvider.NormalizationCache[property] = value;
                return value;
            }
            else
            {
                var value = property == "href" ? "_href"
                    : property == "xmlns" ? "_xmlns"
                    : XmlConvert.EncodeLocalName(property) ?? throw new InvalidOperationException("Should not happen");

                return value;
            }
        }
        // ReSharper restore HeuristicUnreachableCode
#pragma warning restore 162

        /// <summary>
        ///     Denormalizes the property names, so they can be stored into the xml.
        /// </summary>
        /// <param name="property">Property being used</param>
        /// <returns>The value is being sent to the provider</returns>
        public static string DenormalizePropertyName(string property)
        {
            if (property == "_href") return "href";
            if (property == "_xmlns") return "href";

            return XmlConvert.DecodeName(property)
                   ?? throw new InvalidOperationException("Should not happen");
        }

        /// <summary>
        ///     Converts a property name to a property string used for references
        /// </summary>
        /// <param name="propertyName">The name of the property to be converted</param>
        /// <returns>The proeprty with added-ref</returns>
        private string ConvertPropertyToReference(string propertyName)
        {
            return NormalizePropertyName(propertyName) + "-ref";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearPropertyProviderCache()
        {
#pragma warning disable 162
            if (ConfigurationUsePropertyCache) _propertyCache.Clear();
#pragma warning restore 162
        }

        /// <summary>
        ///     Initializes a new instance of the XmlElement class.
        /// </summary>
        /// <param name="node">Node to be used</param>
        /// <param name="provider">Provider to be set</param>
        internal static XmiProviderObject Create(XElement node, XmiProvider provider)
        {
            return new XmiProviderObject(node, provider);
        }

        /// <summary>
        ///     Converts the object to a string value
        /// </summary>
        /// <param name="value">Value to be converted to a string</param>
        /// <returns>Converted the value to text</returns>
        private static string? ReturnObjectAsString(object value)
        {
            return DotNetHelper.AsString(value);
        }

        /// <summary>
        ///     Converts the given value to an xml element.
        ///     If the value is already an XmlElement, it will be reused, otherwise a new XmlNode will
        ///     be created
        /// </summary>
        /// <param name="property">Property to be set</param>
        /// <param name="value">Value to be converted</param>
        /// <returns>The XmlNode reflecting the given element</returns>
        private XElement ConvertValueAsXmlObject(string property, object value)
        {
            lock (_xmiProvider.LockObject)
            {
                if (value is XmiProviderObject valueAsXmlObject)
                {
                    if (valueAsXmlObject.XmlNode.Parent != null)
                        valueAsXmlObject = _xmiProvider.CreateProviderObject(
                            new XElement(valueAsXmlObject.XmlNode));

                    valueAsXmlObject.XmlNode.Name = property;

                    // Creates an id, of the node does not have currently an ID
                    if (XmiId.Get(valueAsXmlObject.XmlNode) == null)
                        XmiId.Set(valueAsXmlObject.XmlNode, XmiId.CreateNew());

                    return valueAsXmlObject.XmlNode;
                }

                /*var valueAsElement = value as IElement;
                if (valueAsElement != null)
                {
                    var copier = new ObjectCopier(new XmlFactory { Owner = _extent, ElementName = _propertyName });
                    return ((XmlElement) copier.Copy(valueAsElement)).XmlNode;
                }*/

                if (DotNetHelper.IsOfPrimitiveType(value)) return new XElement(property, DotNetHelper.AsString(value));

                // A uri reference creates an href element
                if (value is UriReference uriReference)
                    return new XElement(
                        property,
                        new XAttribute("href", uriReference.Uri));
            }

            throw new InvalidOperationException("Value is not an XmlObject or an IElement: " + (value ?? "'null'"));
        }

        /// <summary>
        ///     Gets the uri of the metaclass.
        ///     If there is no metaclass defined, then a pseudo-metaclass will be created.
        /// </summary>
        /// <returns>The string describing the metaclass or null if not found</returns>
        private string? GetMetaClassUri()
        {
            var result = XmlNode.Attribute(TypeAttribute)?.Value;
            if (result == null)
            {
                // Ok, we don't have a type, so return a pseudo-type dependent on the name of the xml
                // node or null, if it is the default element node
                var element = XmlNode.Name;
                return
                    ((XmiProvider) Provider).ElementName == element
                        ? null
                        : $"{NodeMetaClassPrefix}{XmlNode.Name}";
            }

            // Checks, if there is a colon within the metaclass
            var posColon = result.IndexOf(':');
            if (posColon == -1) return result;

            // If there is a colon, get the value before the colon and use this to figure out the real namespace.
            // Like: xmi:type="uml:Package": uml is the namespace. 
            var xmlNamespace = result.Substring(0, posColon);
            var type = result.Substring(posColon + 1);

            // The first part before the colon is the namespace
            var navigator = XmlNode.CreateNavigator();
            navigator.MoveToFollowing(XPathNodeType.Element);

            // Resolves the namespace
            var foundNamespace = navigator.GetNamespace(xmlNamespace);
            if (foundNamespace != null && !string.IsNullOrEmpty(foundNamespace))
                // We have found something, let's combine the url
                return $"{foundNamespace}#{type}";

            return result;
        }

        /// <summary>
        ///     Gets the size of all elements of a value, if that is an enumeration
        /// </summary>
        /// <param name="property">Property to be queried</param>
        /// <returns>The size of the list</returns>
        private int GetSizeOfList(string property)
        {
            lock (_xmiProvider.LockObject)
            {
                return XmlNode.Elements(property).Count();
            }
        }

        /// <summary>
        ///     Finds a certain list into the property list
        /// </summary>
        /// <param name="property">Property, which is selected</param>
        /// <param name="value">Value, which is required</param>
        /// <returns>The found element</returns>
        private XElement? FindInPropertyList(string property, object value)
        {
            var normalizePropertyName = NormalizePropertyName(property);

            if (value is XmiProviderObject valueAsXmlElement)
            {
                var xmiId = XmiId.Get(valueAsXmlElement.XmlNode);
                if (xmiId == null) return null;

                foreach (var subElement in
                         XmlNode.Elements(normalizePropertyName)
                             .Where(subElement =>
                                 XmiId.Get(subElement) == xmiId
                                 || XmiId.GetHref(subElement) == xmiId))
                    return subElement;
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

        /// <summary>
        /// Converts the xmiproviderobject to string
        /// </summary>
        /// <returns>The converted object</returns>
        public override string? ToString()
        {
            lock (_xmiProvider.LockObject)
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
}