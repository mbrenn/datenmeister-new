using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Uml.Helper
{
    /// <summary>
    /// Defines some helper methods for NamedElements
    /// </summary>
    public class NamedElementMethods
    {
        private const int MaxDepth = 1000;

        /// <summary>
        /// Gets the full path to the given element. It traverses through the container values of the
        /// objects and retrieves the partial names by 'name'.
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>Full name of the element</returns>
        public static string GetFullName(IObject value)
        {
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(nameof(value));
                case IElement valueAsElement:
                    var current = valueAsElement.container();
                    var result = GetName(value);
                    var depth = 0;

                    while (current != null)
                    {
                        var currentName = GetName(current);
                        result = $"{currentName}::{result}";
                        current = current.container();
                        depth++;

                        if (depth > MaxDepth)
                        {
                            throw new InvalidOperationException(
                                $"The full name of the element {value} could not be retrieved due to an infinite loop. (Threshold is 1000)");
                        }
                    }

                    return result;
            }

            return GetName(value);
        }

        /// <summary>
        /// Gets an element out of the workspace by a fullname
        /// </summary>
        /// <param name="workspace">Workspace to be queried</param>
        /// <param name="fullName">Name of the element</param>
        /// <returns>Found element or null</returns>
        public static IElement GetByFullName(IWorkspace workspace, string fullName)
        {
            return workspace.extent
                .Select(extent => GetByFullName(extent.elements(), fullName))
                .FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Gets an element out of the workspace by a fullname
        /// </summary>
        /// <param name="extent">Extent to be queried</param>
        /// <param name="fullName">Name of the element</param>
        /// <returns>Found element or null</returns>
        public static IElement GetByFullName(IUriExtent extent, string fullName) =>
            GetByFullName(extent.elements(), fullName);

        /// <summary>
        /// Gets the given element by the fullname by traversing through the name attributes
        /// </summary>
        /// <param name="collection">Collection to be queried, could also be an extent reflective collection</param>
        /// <param name="fullName">Fullname to be traversed, each element shall be separated by an ':'</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement GetByFullName(IReflectiveCollection collection, string fullName)
        {
            var elementNames = fullName
                .Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            var current = (IEnumerable<object>) collection;
            IElement found = null;

            foreach (var elementName in elementNames)
            {
                found = null;
                foreach (var currentValue in current
                    .Select(x => x as IElement)
                    .Where(x => x != null))
                {
                    var name = GetName(currentValue);
                    if (name == elementName)
                    {
                        // We found the element, now abort the search and look in its properties
                        found = currentValue;
                        break;
                    }
                }

                if (found == null)
                {
                    return null;
                }

                // Ok, get all list properties as one big enumeration
                current = GetAllPropertyValues(found);
            }

            return found;
        }

        /// <summary>
        /// Creates one huge enumeration containing all the property values
        /// </summary>
        /// <param name="value">The value being queried</param>
        /// <returns>An enumeration</returns>
        public static IEnumerable<IElement> GetAllPropertyValues(IElement value)
        {
            if (!(value is IObjectAllProperties asProperties))
            {
                throw new ArgumentException("Value is not of type 'IObjectAllProperties'");
            }

            foreach (var property in asProperties.getPropertiesBeingSet())
            {
                if (!value.isSet(property))
                {
                    continue;
                }

                // Property exists
                var propertyValue = value.get(property);
                if (!DotNetHelper.IsOfEnumeration(propertyValue))
                {
                    continue;
                }

                // and is an enumeration
                var asEnumeration = (IEnumerable) propertyValue;
                foreach (var innerValue in asEnumeration)
                {
                    if (innerValue is IElement asElement)
                    {
                        // and inner value is an IElement
                        yield return asElement;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element">Element whose name is requested</param>
        /// <returns>The found name or null, if not found</returns>
        public static string GetName(IObject element)
        {
            if (element == null)
            {
                return "null";
            }

            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            if (element.isSet(_UML._CommonStructure._NamedElement.name))
            {
                return element.get(_UML._CommonStructure._NamedElement.name).ToString();
            }

            switch (element)
            {
                case IHasId elementAsHasId:
                    return elementAsHasId.Id;
                case MofObjectShadow shadowedObject:
                    return shadowedObject.Uri;
                case MofObject _:
                    return "MofObject";
                default:
                    return element.ToString();
            }
        }

        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetName(object element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return
                !(element is IObject asObject)
                    ? element.ToString()
                    : GetName(asObject);
        }
    }
}