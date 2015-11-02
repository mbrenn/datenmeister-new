using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.XMI
{
    /// <summary>
    /// Stores some Xmi, Uml and other namespaces
    /// </summary>
    public static class Namespaces
    {
        /// <summary>
        /// Defines the namespace for XMI
        /// </summary>
        public static readonly XNamespace Xmi = "http://www.omg.org/spec/XMI/20131001";

        /// <summary>
        /// Defines the Uml namespace
        /// </summary>
        public static readonly XNamespace Uml = "http://www.omg.org/spec/UML/20131001";
    }
}
