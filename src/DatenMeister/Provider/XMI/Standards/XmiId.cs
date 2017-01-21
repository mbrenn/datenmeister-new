using System.Linq;
using System.Xml.Linq;

namespace DatenMeister.Provider.XMI.Standards
{
    /// <summary>
    /// Implements the standard as given in http://www.w3.org/TR/xml-id/
    /// </summary>
    public static class XmiId
    {
        public static bool IsValid(XDocument document)
        {
            var foundIds = document.Descendants().Attributes(Namespaces.Xmi + "id").Select(x => x.Value).OrderBy(x => x);
            var currentId = string.Empty;

            foreach (var id in foundIds)
            {
                if (id == currentId)
                {
                    return false;
                }

                currentId = id;
            }

            return true;
        }

        /// <summary>
        /// Gets the id of a certain Xml Element
        /// </summary>
        /// <param name="element">Element whose id shall be retrieved</param>
        /// <returns>The retrieved element</returns>
        public static string Get(XElement element)
        {
            var xmlIdAttribute = element.Attribute(Namespaces.Xmi + "id");
            return xmlIdAttribute?.Value;
        }

        /// <summary>
        /// Sets the id of a certain xml element
        /// </summary>
        /// <param name="element">Element which shall retrieve an id</param>
        /// <param name="id">Id of the element to be set</param>
        public static void Set(XElement element, string id)
        {
            element.SetAttributeValue(Namespaces.Xmi + "id", id);
        }

        /// <summary>
        /// Returns a value indicating whether the element has an id
        /// </summary>
        /// <param name="node">Node to be evaluated</param>
        /// <returns>true, if element has an id</returns>
        public static bool HasId(XElement node)
        {
            return node.Attribute(Namespaces.Xmi + "id") != null;
        }
    }
}