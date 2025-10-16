using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;

namespace DatenMeister.Provider.Xmi.Provider.XMI;

/// <summary>
/// Converts a given object to an xml object
/// </summary>
public class XmlConverter
{
    private readonly MofExtent _extent = new(
        new XmiProvider());

    /// <summary>
    /// Ignores the ids
    /// </summary>
    public bool SkipIds { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the xml form shall contain relative paths
    /// </summary>
    public bool RelativePaths { get; set; }

    /// <summary>
    /// Converts the given element to an xml element
    /// </summary>
    /// <param name="element">Element to be converted</param>
    /// <returns>Converted element to be shown</returns>
    public XElement ConvertToXml(IObject element)
    {
        var copyOptions = new CopyOption { CloneAllReferences = false, CopyId = true };
        var extentName = (element as IElement)?.GetUriExtentOf()?.contextURI() ??
                         string.Empty;
        var copier = new ObjectCopier(new MofFactory(_extent));

        var result = (MofElement)copier.Copy(element, copyOptions); // Copies the element
        var xmlNode = ((XmiProviderObject)result.ProviderObject).XmlNode;

        if (SkipIds)
        {
            foreach (var node in xmlNode.DescendantNodesAndSelf().OfType<XElement>())
            {
                node.Attribute(XmiId.IdAttributeName)?.Remove();
            }
        }

        if (RelativePaths && extentName != string.Empty)
        {
            // Go through all nodes and converts the given paths to relative paths
            RemoveAbsolutePathsOfExtent(xmlNode, extentName);
        }

        return xmlNode;
    }

    /// <summary>
    /// Converts the given element to an xml element
    /// </summary>
    /// <param name="elements">Element to be converted</param>
    /// <returns>Converted element to be shown</returns>
    public XElement ConvertToXml(IEnumerable<object?> elements)
    {
        var copyOptions = new CopyOption { CloneAllReferences = false, CopyId = true };
        var factory = new MofFactory(_extent);
        var copier = new ObjectCopier(factory);
        var rootItem = (MofObject)factory.create(null);
        var elementsAsList = elements.ToList();
        var extentName = elementsAsList.OfType<IElement>().FirstOrDefault()?.GetUriExtentOf()?.contextURI() ??
                         string.Empty;

        var list =
            elementsAsList.Cast<IElement>()
                .Select(element => copier.Copy(element, copyOptions))
                .Cast<object>().ToList();

        rootItem.set("items", list);

        var xmlNode = ((XmiProviderObject)rootItem.ProviderObject).XmlNode;

        if (SkipIds)
        {
            foreach (var node in xmlNode.DescendantNodesAndSelf().OfType<XElement>())
            {
                node.Attribute(XmiId.IdAttributeName)?.Remove();
            }
        }

        if (RelativePaths && extentName != string.Empty)
        {
            // Go through all nodes and converts the given paths to relative paths
            RemoveAbsolutePathsOfExtent(xmlNode, extentName);
        }

        return xmlNode;
    }

    private void RemoveAbsolutePathsOfExtent(XElement xmlNode, string extentPath)
    {
        foreach (var node in xmlNode.DescendantNodesAndSelf().OfType<XElement>())
        {
            foreach (var attribute in node.Attributes())
            {
                if ((attribute.Name.ToString() == "href" || attribute.Name.ToString().EndsWith("-ref")) &&
                    attribute.Value.StartsWith(extentPath))
                {
                    attribute.Value = attribute.Value.Substring(extentPath.Length);
                }
            }
        }
    }

    /// <summary>
    /// Converts the given XElement to a MOF object
    /// </summary>
    /// <param name="element">Element to be converted</param>
    /// <param name="factory">Factory being used</param>
    /// <returns>The converted element</returns>
    public IObject ConvertFromXml(XElement element, IFactory factory)
    {
        var loader = new SimpleLoader();
        return loader.LoadFromXmlNode(factory, element);
    }
}