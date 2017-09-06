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
        private MofExtent _extent;

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
        
    }
}