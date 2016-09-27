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

        public static string Get(XElement element)
        {
            var xmlIdAttribute = element.Attribute(Namespaces.Xmi + "id");
            return xmlIdAttribute?.Value;
        }
    }
}