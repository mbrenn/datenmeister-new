using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.XMI.UmlBootstrap
{
    /// <summary>
    ///     Contains some methods which help to parse and understand the xmi file
    /// </summary>
    public static class Helper
    {
        /// <summary>
        ///     Gets all packages of a specific xml node
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

        public static IEnumerable<XElement> XmiGetProperty(this XElement element)
        {
            var typeName = "uml:Property";
            return XmlGetElementsOfType(element, typeName);
        }

        /// <summary>
        ///     Gets all packages of a specific xml node
        /// </summary>
        /// <param name="element"></param>
        public static IEnumerable<IObject> XmiGetPackages(this IObject element)
        {
            var typeName = "uml:Package";
            return XmlGetElementsOfType(element, typeName);
        }

        public static IEnumerable<IObject> XmiGetClass(this IObject element)
        {
            var typeName = "uml:Class";
            return XmlGetElementsOfType(element, typeName);
        }

        public static IEnumerable<IObject> XmiGetProperty(this IObject element)
        {
            var typeName = "uml:Property";
            return XmlGetElementsOfType(element, typeName);
        }

        /// <summary>
        ///     Returns an anumeration of all elements having a specific type
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
        ///     Goes through the instance of the objects and returns the
        ///     ones which are of a specific type
        /// </summary>
        /// <param name="element">Element to be queried</param>
        /// <param name="typeName">Only the elements of this type will be returned</param>
        /// <returns>Enumeration of objects of the given type</returns>
        private static IEnumerable<IObject> XmlGetElementsOfType(IObject element, string typeName)
        {
            var elementAsExt = (IObjectExt) element;
            if (elementAsExt == null)
            {
                throw new ArgumentNullException("element is not Null");
            }

            var attributeXmi = "{" + Namespaces.Xmi + "}type";

            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var propertyValue = element.get(property);
                var propertyAsEnumerable = propertyValue as IEnumerable;
                foreach (var innerValue in propertyAsEnumerable)
                {
                    var innerValueAsObject = innerValue as IObject;
                    if (innerValueAsObject != null && innerValueAsObject.isSet(attributeXmi))
                    {
                        var type = innerValueAsObject.get(attributeXmi).ToString();
                        if (type == typeName)
                        {
                            yield return innerValueAsObject;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the xmi name of the given xml Node
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string XmiGetName(this XElement element)
        {
            return element.Attribute("name")?.Value;
        }
    }
}