using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Provider.XMI.EMOF;

namespace DatenMeister.Modules.Xml
{
    /// <summary>
    /// Defines a class which is capable to import an xml to
    /// a container
    /// </summary>
    public class ByXmlImporter
    {
        /// <summary>
        /// Imports the xmltext into the default composite property
        /// of the given object
        /// </summary>
        /// <param name="container">Container to be added</param>
        /// <param name="xmlText">Xml text containing the elements to be imported</param>
        public static void ImportByXml(IObject container, string xmlText)
        {
            var document = XDocument.Parse(xmlText);
            var provider = new XmiProvider(document);
            var createdObject = new MofUriExtent(provider);

            // Gets the reflective-collection for the element
            var reflectiveCollection = DefaultClassifierHints.GetDefaultReflectiveCollection(container);
            /*var extentCopier = new ExtentCopier(new MofFactory(container));
            extentCopier.Copy(createdObject.elements(), reflectiveCollection);*/

            foreach (var xmlElement in document.Elements())
            {
                var innerElement = new XmiProviderObject(xmlElement, provider);
                var innerMofElement = new MofElement(innerElement, createdObject);

                reflectiveCollection.add(innerMofElement);
            }
        }
    }
}