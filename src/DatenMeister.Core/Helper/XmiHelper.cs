using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;

namespace DatenMeister.Core.Helper;

public static class XmiHelper
{
    public static string ConvertToXmiFromObject(IObject value)
    {
        var provider = new XmiProvider();
        var extent = new MofUriExtent(provider, null);

        var copiedResult = ObjectCopier.Copy(new MofFactory(extent), value, CopyOptions.CopyId);
        var providerObject = (copiedResult as MofObject)?.ProviderObject;
        var xml = (providerObject as XmiProviderObject)?.XmlNode;

        if (xml == null)
        {
            throw new InvalidOperationException("xml not found");
        }

        return xml.ToString();
    }

    /// <summary>
    /// Converts the reflective collection to an xml text
    /// </summary>
    /// <param name="collection">Collection to be reparsed</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown, if procedure failed</exception>
    public static string ConvertToXmiFromCollection(IEnumerable<object?> collection)
    {
        var provider = new XmiProvider();
        var extent = new MofUriExtent(provider, null);

        foreach (var item in collection.OfType<IObject>())
        {
            var copiedResult = ObjectCopier.Copy(new MofFactory(extent), item, CopyOptions.CopyId);
            extent.elements().add(copiedResult);
        }

        var xml = provider.Document.Root;

        if (xml == null)
        {
            throw new InvalidOperationException("xml not found");
        }

        return xml.ToString();
    }

    /// <summary>
    /// Converts the given object back from a string-based xmi to a Mof Object
    /// </summary>
    /// <param name="xmi">Xmi to be converted</param>
    /// <param name="slimEvaluation">true, if the slim evaluation shall be activated</param>
    /// <returns>The converted element</returns>
    public static IObject ConvertItemFromXmi(string xmi, bool slimEvaluation = true)
    {
        var provider = new XmiProvider();
        var providerObject = new XmiProviderObject(XElement.Parse(xmi), provider);

        var extent = new MofUriExtent(provider, null);
        extent.LocalSlimUmlEvaluation = slimEvaluation;
        return new MofElement(providerObject, extent);
    }

    /// <summary>
    /// Converts the given object back from a string-based xmi to a Mof Object
    /// </summary>
    /// <param name="xmi">Xmi to be converted</param>
    /// <param name="slimEvaluation">true, if the slim evaluation shall be activated</param>
    /// <returns>The converted element</returns>
    public static IReflectiveCollection ConvertCollectionFromXmi(string xmi, bool slimEvaluation = true)
    {
        var provider = new XmiProvider();
        var extent = new MofUriExtent(provider, null);
        extent.LocalSlimUmlEvaluation = slimEvaluation;
        var collection = new TemporaryReflectiveCollection();

        var document = XDocument.Parse(xmi) ?? throw new InvalidOperationException("document");
        var rootNode = document.Element(XmiProvider.DefaultRootNodeName);
        var itemNode = rootNode!.Elements("item");
        foreach (XElement element in itemNode)
        {
            collection.add(new MofElement(new XmiProviderObject(element, provider), extent));
        }

        return collection;
    }
}