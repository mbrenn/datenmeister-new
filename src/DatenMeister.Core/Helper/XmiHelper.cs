using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;

namespace DatenMeister.Core.Helper
{
    public class XmiHelper
    {
        public static string ConvertToXmi(IObject value)
        {
            var provider = new XmiProvider();
            var extent = new MofUriExtent(provider);

            var copiedResult = ObjectCopier.Copy(new MofFactory(extent), value);
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
        public static string ConvertToXmi(IReflectiveCollection collection)
        {
            var provider = new XmiProvider();
            var extent = new MofUriExtent(provider);

            foreach (var item in collection.OfType<IObject>())
            {
                var copiedResult = ObjectCopier.Copy(new MofFactory(extent), item);
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
        /// <returns>The converted element</returns>
        public static IObject ConvertItemFromXmi(string xmi)
        {
            var provider = new XmiProvider();
            var providerObject = new XmiProviderObject(XElement.Parse(xmi), provider);
            
            var extent = new MofUriExtent(provider);
            return new MofObject(providerObject, extent);
        }

        /// <summary>
        /// Converts the given object back from a string-based xmi to a Mof Object
        /// </summary>
        /// <param name="xmi">Xmi to be converted</param>
        /// <returns>The converted element</returns>
        public static IReflectiveCollection ConvertCollectionFromXmi(string xmi)
        {
            var provider = new XmiProvider();
            var extent = new MofUriExtent(provider);
            var collection = new TemporaryReflectiveCollection();

            var document = XDocument.Parse(xmi) ?? throw new InvalidOperationException("document");
            var rootNode = document.Element(XmiProvider.DefaultRootNodeName);
            var itemNode = rootNode!.Elements("item");
            foreach (XElement element in itemNode)
            {
                collection.add(new MofObject(new XmiProviderObject(element, provider), extent));
            }

            return collection;
        }
    }
}