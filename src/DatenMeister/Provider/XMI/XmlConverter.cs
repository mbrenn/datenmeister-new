using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Provider.XMI
{
    /// <summary>
    /// Converts a given object to an xml object
    /// </summary>
    public class XmlConverter
    {
        private readonly MofExtent _extent;

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
        public XElement ConvertToXml(IElement element)
        {
            var copier = new ObjectCopier(new MofFactory(_extent));
            var result = (MofElement) copier.Copy(element);


            return ((XmiProviderObject) result.ProviderObject).XmlNode;
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

            return ((XmiProviderObject) rootItem.ProviderObject).XmlNode;
        }
    }
}