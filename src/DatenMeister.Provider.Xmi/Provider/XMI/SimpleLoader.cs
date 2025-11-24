using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.Xmi.Provider.XMI;

/// <summary>
/// Includes a simple XMI loader which is attribute and element driven.
/// By loading an xmi file, the attributes and extents are directly stored into an existing Extent.
/// </summary>
public class SimpleLoader
{
    /// <summary>
    /// Defines a list of actions that will be performed after the loading has finished
    /// </summary>
    private readonly List<Action> _afterLoadActions = new();

    private readonly Dictionary<string, IElement> _idToElement = new();

    /// <summary>
    /// Stores the uri resolver being used to figure out the href instances.
    /// </summary>
    private readonly IUriResolver? _uriResolver;

    /// <summary>
    /// Initializes a new instance of the
    /// </summary>
    /// <param name="uriResolver"></param>
    public SimpleLoader(IUriResolver? uriResolver = null)
    {
        _uriResolver = uriResolver;
    }

    /// <summary>
    /// Loads the xmi from the embedded resources
    /// </summary>
    /// <param name="factory">Factory being used to elements</param>
    /// <param name="extent">Extent being loaded</param>
    /// <param name="resourceName">Path to the resources</param>
    public void LoadFromEmbeddedResource(IFactory factory, IUriExtent extent, string resourceName)
    {
        using var stream = typeof(WorkspaceNames).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException($"Stream for {resourceName} is not found");
        LoadFromStream(factory, extent, stream);
    }

    public void LoadFromFile(IFactory factory, IUriExtent extent, string filePath)
    {
        ArgumentNullException.ThrowIfNull(factory);
        using var stream = new FileStream(filePath, FileMode.Open);
        LoadFromStream(factory, extent, stream);
    }

    /// <summary>
    ///     Loads the file from a stream
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="extent">Extent to which the data is loaded</param>
    /// <param name="stream">Stream to be used for loading</param>^
    public void LoadFromStream(IFactory factory, IUriExtent extent, Stream stream)
    {
        var document = XDocument.Load(stream);
        LoadFromDocument(factory, extent, document);
    }

    /// <summary>
    ///     Loads the document from an XDocument
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="extent">Extent to which the data is loaded</param>
    /// <param name="document">Document to be loaded</param>
    public void LoadFromDocument(IFactory factory, IUriExtent extent, XDocument document)
    {
        _idToElement.Clear();
        _afterLoadActions.Clear();

        // Skip the first element
        foreach (var element in document.Elements().Elements())
        {
            extent.elements().add(LoadElement(factory, element));
        }

        foreach (var action in _afterLoadActions)
        {
            action();
        }
    }

    /// <summary>
    /// Loads the document from a string.
    /// </summary>
    /// <param name="factory">Factory being used to create the instance</param>
    /// <param name="extent">Extent to which the elements will be added</param>
    /// <param name="xmlText">Text to be addede</param>
    public void LoadFromText(IFactory factory, IUriExtent extent, string xmlText)
    {
        var document = XDocument.Parse(xmlText);
        LoadFromDocument(factory, extent, document);
    }

    /// <summary>
    /// Loads the Xml from a text
    /// </summary>
    /// <param name="factory">Factory to be used</param>
    /// <param name="element">Element being used</param>
    public IObject LoadFromXmlNode(IFactory factory, XElement element) =>
        LoadElement(factory, element);

    /// <summary>
    ///     Loads the specific element with a very simple loading algorithm
    /// </summary>
    /// <param name="factory">Factory being used to create instances</param>
    /// <param name="element">Xml Element to be used to load object</param>
    private IObject LoadElement(IFactory factory, XElement element)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(element);

        var resultingElement = factory.create(null);
        foreach (var attribute in element.Attributes())
        {
            resultingElement.set(attribute.Name.ToString(), attribute.Value);
        }

        // Check, if element has id
        var xmiId = XmiId.Get(element);
        if (xmiId != null)
        {
            // In some Xmi files, the Xmi-id is used multiple times (e.g. Uml). We only assign it the first time
            if (!_idToElement.ContainsKey(xmiId))
            {
                _idToElement[xmiId] = resultingElement;

                if (resultingElement is ICanSetId resultSetId)
                {
                    resultSetId.Id = xmiId;
                }
            }
        }

        var dict = new Dictionary<string, IReflectiveCollection>();

        foreach (var subElement in element.Elements())
        {
            // Checks, if the given element is a reference
            var attributeIdRef = subElement.Attribute(Namespaces.Xmi + "idref");
            if (attributeIdRef != null)
            {
                _afterLoadActions.Add(
                    () =>
                    {
                        var referencedElement = _idToElement[attributeIdRef.Value];
                        resultingElement.set(subElement.Name.ToString(), referencedElement);
                    });
                continue;
            }

            var attributeHref = subElement.Attribute("href");
            if (attributeHref != null && _uriResolver != null)
            {
                _afterLoadActions.Add(
                    () =>
                    {
                        var referencedElement = _uriResolver?.Resolve(attributeHref.Value, ResolveType.Default);
                        if (referencedElement == null)
                        {
                            throw new InvalidOperationException("Unknown href:" + attributeHref.Value);
                        }

                        resultingElement.set(subElement.Name.ToString(), referencedElement);
                    });
                continue;
            }

            // Element is not a given element, so perform regular replacement of elements
            var name = subElement.Name.ToString();
            IReflectiveCollection currentList;
            if (dict.ContainsKey(name))
            {
                currentList = dict[name];
            }
            else
            {
                resultingElement.set(name, new List<object>());
                currentList = resultingElement.get<IReflectiveCollection>(name);
                dict[name] = currentList;
            }

            if (subElement.HasElements || subElement.HasAttributes)
            {
                var loadedElement = LoadElement(factory, subElement);

                // Sets the container being used
                // ReSharper disable once SuspiciousTypeConversion.Global
                var asSetContainer = loadedElement as IElementSetContainer;
                asSetContainer?.setContainer(resultingElement);

                // Adds the item to the current list
                currentList.add(loadedElement);
            }
            else
            {
                currentList.add(subElement.Value);
            }
        }

        return resultingElement;
    }
}