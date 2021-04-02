using System.Xml.Linq;

namespace DatenMeister.Core.Provider.Xmi
{
    /// <summary>
    ///     Stores some Xmi, Uml and other namespaces
    /// </summary>
    public static class Namespaces
    {
        /// <summary>
        ///     Defines the namespace for XMI
        /// </summary>
        public static readonly XNamespace Xmi = "http://www.omg.org/spec/XMI/20131001";

        /// <summary>
        ///     Defines the Uml namespace
        /// </summary>
        public static readonly XNamespace Uml = "http://www.omg.org/spec/UML/20131001";

        /// <summary>
        ///     Defines the Uml namespace
        /// </summary>
        public static readonly XNamespace XmlNamespace = "http://www.w3.org/2000/xmlns/";
    }
}