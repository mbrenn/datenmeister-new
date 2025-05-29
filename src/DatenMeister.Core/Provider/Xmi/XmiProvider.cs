using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;

namespace DatenMeister.Core.Provider.Xmi;

/// <summary>
/// Defines the provider for xml manipulation
/// </summary>
public class XmiProvider : IProvider, IHasUriResolver, IProviderSupportFunctions
{
    /// <summary>
    /// Gets the value whether the same xmiprovider instance shall always be used
    /// for the same XmlNode. 
    /// </summary>
    public const bool ConfigurationUseIsolatedXmiProviderObjects = true;

    public const string DefaultRootNodeName = "xmi";

    private const string DefaultElementNodeName = "item";

    private static readonly XNamespace XDatenMeisterNamespace = "http://datenmeister.net/";

    internal static readonly XName XMetaXmlNodeName = XDatenMeisterNamespace + "meta";

    /// <summary>
    ///     Defines the cache
    /// </summary>
    private readonly Dictionary<string, XElement> _cache = new();

    /// <summary>
    /// Stores the cached provider objects to be sure that the same XmiProvider object will be used
    /// when the same XElement is retrieved
    /// </summary>
    private readonly Dictionary<XElement, XmiProviderObject> _providerObjectCache = new();

    private readonly XElement _rootNode;

    internal readonly object LockObject = new();

    internal readonly Dictionary<string, string> NormalizationCache = new();

    /// <summary>
    /// Defines the interface
    /// </summary>
    private ProviderSupportFunctions? _supportFunctions;

    public XmiProvider( /*string rootNodeName = DefaultRootNodeName*/)
    {
        var rootNodeName = DefaultRootNodeName;
        Document = new XDocument();
        _rootNode = new XElement(rootNodeName);
        Document.Add(_rootNode);
        /*_rootNode.SetAttributeValue(_urlPropertyName, uri);*/

        CreateProviderSupportFunctions();
    }

    public XmiProvider(XDocument document /*, string rootNodeName = DefaultRootNodeName*/)
    {
        var rootNodeName = document.Elements().First().Name;

        Document = document;
        _rootNode = Document.Element(rootNodeName) ??
                    throw new InvalidOperationException("Xml Document inconclusive");

        if (_rootNode == null)
        {
            throw new InvalidOperationException(
                $"The given document does not have a root node called {rootNodeName}.");
        }

        CreateProviderSupportFunctions();
    }

    /// <summary>
    /// Defines the name of the element
    /// </summary>
    public string ElementName { get; set; } = DefaultElementNodeName;

    public XDocument Document { get; }

    /// <summary>
    ///     Gets or sets the uri resolver for this provider. Will be used to figure out information
    ///     about the meta classes
    /// </summary>
    public IUriResolver? UriResolver { get; set; }

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

    /// <inheritdoc />
    public void AddElement(IProviderObject? valueAsObject, int index = -1)
    {
        lock (LockObject)
        {
            if (valueAsObject is XmiProviderObject providerObject)
            {
                var count = _rootNode.Elements(ElementName).Count();
                if (index == 0)
                {
                    // First node could be a meta node, so only the nodes with Element Name are interesting
                    var firstNode = _rootNode.Elements(ElementName).FirstOrDefault();
                    if (firstNode == null)
                    {
                        // If there is absolutely no node, then add the node at the end of the document
                        _rootNode.Add(providerObject.XmlNode);
                    }
                    else
                    {
                        firstNode.AddBeforeSelf(providerObject.XmlNode);
                    }
                }
                else if (index < 0 || index >= count)
                {
                    // Add new node at the end of the document
                    _rootNode.Add(providerObject.XmlNode);
                }
                else
                {
                    // Insert the node at the given index
                    var before = _rootNode.Elements(ElementName).ElementAt(index - 1);
                    before.AddAfterSelf(providerObject.XmlNode);
                }
            }
            else
            {
                // We need to create a new element by copying the values from the provider object to the new element
                var newProviderObject = CreateElement(valueAsObject?.MetaclassUri);
                foreach (var property in valueAsObject?.GetProperties() ?? [])
                {
                    newProviderObject.SetProperty(property, valueAsObject?.GetProperty(property));
                }

                // Recursive call, but now the provider object is having the correct type
                if (newProviderObject is not XmiProviderObject)
                {
                    // Additional, cautious check to avoid unlimited recursion
                    throw new InvalidOperationException("newProviderObject is not XmiProviderObject");
                }

                AddElement(newProviderObject, index);
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
                _cache.Remove(id);
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

            _cache.Clear();
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
    /// Gets the support functions
    /// </summary>
    public ProviderSupportFunctions ProviderSupportFunctions => _supportFunctions
                                                                ?? throw new NotSupportedException(
                                                                    "Should not happen");

    private void CreateProviderSupportFunctions()
    {
        _supportFunctions = new ProviderSupportFunctions
        {
            QueryById = id =>
            {
                var result = FindById(id);
                if (result == null) return null;

                return CreateProviderObject(result);
            }
        };
    }

    /// <summary>
    ///     Gets the meta node. If the meta node does not exist, create the meta node
    /// </summary>
    /// <returns>The returned meta node</returns>
    private XElement GetMetaNode()
    {
        lock (LockObject)
        {
            var metaNode = _rootNode.Element(XMetaXmlNodeName);
            if (metaNode == null)
            {
                metaNode = new XElement(XMetaXmlNodeName);
                _rootNode.AddFirst(metaNode);
            }

            return metaNode;
        }
    }

    /// <summary>
    ///     Creates a new provider object for the xmiprovider
    /// </summary>
    /// <param name="xmlElement">Xml element storing the data</param>
    /// <returns>Created ProviderObject</returns>
    public XmiProviderObject CreateProviderObject(XElement xmlElement)
    {
#pragma warning disable 162
        if (ConfigurationUseIsolatedXmiProviderObjects)
        {
            XmiProviderObject? result;

            lock (_providerObjectCache)
            {
                if (_providerObjectCache.TryGetValue(xmlElement, out result)) return result;
            }

            result = XmiProviderObject.Create(xmlElement, this);

            lock (_providerObjectCache)
            {
                _providerObjectCache[xmlElement] = result;

                return result;
            }
        }

        // ReSharper disable once HeuristicUnreachableCode
        return XmiProviderObject.Create(xmlElement, this);
#pragma warning restore 162
    }

    /// <summary>
    ///     Finds a certain Xml Element by its id
    /// </summary>
    /// <param name="id">Id to be queried</param>
    /// <returns>The found element</returns>
    private XElement? FindById(string id)
    {
        lock (LockObject)
        {
            if (_cache.TryGetValue(id, out var element))
            {
                if (XmiId.Get(element) == id) return element;

                _cache.Remove(id);
            }
        }

        lock (LockObject)
        {
            foreach (var x in _rootNode.Descendants())
            {
                var foundId = XmiId.Get(x);
                if (foundId != null)
                {
                    // Go through each item to build up the cache
                    _cache[foundId] = x;
                }
            }
        }

        return _cache.TryGetValue(id, out var element2) ? element2 : null;
    }

    public static XElement GetMetaNodeFromFile(string filePath)
    {
        var document = XDocument.Load(filePath);
        var provider = new XmiProvider(document);
        return provider.GetMetaNode();
    }

    /// <summary>
    /// Creates an Xmi Provider by using a certain file pat
    /// </summary>
    /// <param name="xmiFilePath">File path to be used</param>
    /// <returns>The created xmi</returns>
    public static XmiProvider CreateByFile(string xmiFilePath)
    {
        var document = XDocument.Load(xmiFilePath);
        return new XmiProvider(document);
    }
}