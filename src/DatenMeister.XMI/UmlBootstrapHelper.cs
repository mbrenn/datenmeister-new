using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.XMI
{
    /// <summary>
    /// Contains some methods which help to parse and understand the xmi file
    /// </summary>
    public static class UmlBootstrapHelper
    {
        /// <summary>
        /// Gets all packages of a specific xml node
        /// </summary>
        /// <param name="element"></param>
        public static IEnumerable<XElement> XmiGetPackages(this XElement element)
        {
            var typeName = "uml:Package";
            return XmlGetElementsOfType(element, typeName);
        }

        public static IEnumerable<XElement> XmiGetClass(this XElement element)
        {
            var typeName = "uml:Class";
            return XmlGetElementsOfType(element, typeName);
        }

        /// <summary>
        /// Returns an anumeration of all elements having a specific type
        /// </summary>
        /// <param name="element">Element to be queried</param>
        /// <param name="typeName">Name of the type being queried</param>
        /// <returns>Enumeration of all elements</returns>
        private static IEnumerable<XElement> XmlGetElementsOfType(XElement element, string typeName)
        {
            return element
                .Elements()
                .Where(x => x.Attribute(Namespaces.Xmi + "type")?.Value == typeName);
        }

        /// <summary>
        /// Gets the xmi name of the given xml Node
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string XmiGetName(this XElement element)
        {
            return element.Attribute("name")?.Value;
        }
    }
}
