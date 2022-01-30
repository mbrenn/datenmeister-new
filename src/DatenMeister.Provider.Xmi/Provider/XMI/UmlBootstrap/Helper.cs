using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.Xmi;

namespace DatenMeister.Provider.XMI.UmlBootstrap
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

        public static IEnumerable<IObject> XmiGetClassOrDerived(this IObject element)
        {
            var typeNames = new[]
            {
                "uml:Class",
                "uml:PrimitiveType"
            };

            return XmlGetElementsOfTypes(element, typeNames);
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
            return XmlGetElementsOfTypes(element, new[] { typeName });
        }

        /// <summary>
        /// Gets all elements of a specific type in the proeprty
        /// </summary>
        /// <param name="element">Element, whose property is queried </param>
        /// <param name="typeNames">Type names to be queried</param>
        /// <returns>Enumeration of all elements</returns>
        private static IEnumerable<IObject> XmlGetElementsOfTypes(IObject element, IEnumerable<string> typeNames)
        {
            var elementAsExt = (IObjectAllProperties)element;
            if (elementAsExt == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var attributeXmi = "{" + Namespaces.Xmi + "}type";
            var typeNamesList = typeNames.ToList();

            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var propertyValue = element.get(property);
                if (!(propertyValue is IEnumerable propertyAsEnumerable))
                    continue;

                foreach (var innerValue in propertyAsEnumerable)
                {
                    var innerValueAsObject = innerValue as IObject;
                    if (innerValueAsObject?.isSet(attributeXmi) == true)
                    {
                        var type = innerValueAsObject.getOrDefault<string>(attributeXmi);
                        if (typeNamesList.Count(x => type == x) > 0)
                        {
                            yield return innerValueAsObject;
                        }
                    }
                }
            }
        }

        public static IEnumerable<IObject> GetSubProperties(IObject element)
        {
            var elementAsExt = (IObjectAllProperties)element;
            if (elementAsExt == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var propertyValue = element.get(property);

                if (DotNetHelper.IsOfEnumeration(propertyValue))
                {
                    var propertyAsEnumerable = propertyValue as IEnumerable
                                               ?? throw new InvalidOperationException("Something obscure happened");

                    foreach (var innerValue in propertyAsEnumerable)
                    {
                        if (innerValue is IObject innerValueAsObject)
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
        public static string? XmiGetName(this XElement element) =>
            element.Attribute("name")?.Value;
    }
}