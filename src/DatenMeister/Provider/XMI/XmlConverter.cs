using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Provider.XMI.Standards;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Provider.XMI
{
    /// <summary>
    /// Converts a given object to an xml object
    /// </summary>
    public class XmlConverter
    {
        private readonly MofExtent _extent;

        /// <summary>
        /// Ignores the ids
        /// </summary>
        public bool SkipIds { get; set; }

        public XmlConverter()
        {
            _extent = new MofExtent(
                new XmiProvider());
        }
        
        /// <summary>
        /// Converts the given element to an xml element
        /// </summary>
        /// <param name="element">Element to be converted</param>
        /// <returns>Converted element to be shown</returns>
        public XElement ConvertToXml(IObject element)
        {
            var copier = new ObjectCopier(new MofFactory(_extent));

            var result = (MofElement) copier.Copy(element);
            var xmlNode = ((XmiProviderObject)result.ProviderObject).XmlNode;
            if (SkipIds)
            {
                foreach (var node in xmlNode.DescendantNodesAndSelf().OfType<XElement>())
                {
                    node.Attribute(XmiId.IdAttributeName)?.Remove();
                }
            }

            return xmlNode;
        }

        /// <summary>
        /// Converts the given element to an xml element
        /// </summary>
        /// <param name="elements">Element to be converted</param>
        /// <returns>Converted element to be shown</returns>
        public XElement ConvertToXml(IEnumerable<object> elements)
        {
            var factory = new MofFactory(_extent);
            var copier = new ObjectCopier(factory);
            var rootItem = (MofObject) factory.create(null);

            var list = 
                elements.Cast<IElement>()
                    .Select(element => copier.Copy(element))
                    .Cast<object>().ToList();

            rootItem.set("items", list);

            var xmlNode = ((XmiProviderObject) rootItem.ProviderObject).XmlNode;

            if (SkipIds)
            {
                foreach (var node in xmlNode.DescendantNodesAndSelf().OfType<XElement>())
                {
                    node.Attribute(XmiId.IdAttributeName)?.Remove();
                }
            }

            return xmlNode;
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
}