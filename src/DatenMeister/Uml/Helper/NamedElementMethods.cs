using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Provider;
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
        /// <param name="separator">Separator</param>
        /// <returns>Full name of the element</returns>
        public static string GetFullName(IObject value, string separator = "::")
        {
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(nameof(value));
                case MofObjectShadow shadow:
                    return shadow.Uri;
                case IElement valueAsElement:
                    var current = valueAsElement.container();
                    var result = GetName(value);
                    var depth = 0;

                    while (current != null)
                    {
                        var currentName = GetName(current);
                        result = $"{currentName}{separator}{result}";
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
        
        public static string GetFullNameWithoutElementId(IObject value, string separator = "::")
        {
            var realSeparator = string.Empty;
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(nameof(value));
                case IElement valueAsElement:
                    var current = valueAsElement.container();
                    var result = string.Empty;
                    var depth = 0;

                    while (current != null)
                    {
                        var currentName = GetName(current);
                        result = $"{currentName}{realSeparator}{result}";

                        realSeparator = separator;

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
        public static IElement? GetByFullName(IWorkspace workspace, string fullName)
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
        public static IElement? GetByFullName(IUriExtent extent, string fullName) =>
            GetByFullName(extent.elements(), fullName);

        /// <summary>
        /// Gets the given element by the fullname by traversing through the name attributes
        /// </summary>
        /// <param name="collection">Collection to be queried, could also be an extent reflective collection</param>
        /// <param name="fullName">Fullname to be traversed, each element shall be separated by an ':'</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement? GetByFullName(IReflectiveCollection collection, string fullName)
        {
            var elementNames = fullName
                .Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            var current = (IEnumerable<object>) collection;
            IElement? found = null;

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
                current = GetAllPropertyValues(found, true);
            }

            return found;
        }

        /// <summary>
        /// Creates one huge enumeration containing all the property values
        /// </summary>
        /// <param name="value">The value being queried</param>
        /// <param name="noReferences">true, if the method call shall not resolve the references</param>
        /// <returns>An enumeration</returns>
        public static IEnumerable<IElement> GetAllPropertyValues(IElement value, bool noReferences = false)
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
                object? propertyValue;
                if (value is MofObject valueConverted)
                {
                    // If no references is set and the given object supports the query via 'noReferences' 
                    propertyValue = valueConverted.get(property, noReferences, ObjectType.ReflectiveSequence);
                }
                else
                {
                    propertyValue = value.get(property);
                }

                if (!DotNetHelper.IsOfEnumeration(propertyValue) || propertyValue == null)
                {
                    continue;
                }

                // and is an enumeration
                var asEnumeration = (IEnumerable) propertyValue;
                foreach (var enumerationElement in CollectionHelper.EnumerateWithNoResolving(asEnumeration, true)
                    .OfType<IElement>())
                {
                    yield return enumerationElement;
                }
            }
        }

        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element">Element whose name is requested</param>
        /// <param name="noReferences">Indicates whether the elements shall be dereferenced</param>
        /// <returns>The found name or null, if not found</returns>
        public static string GetName(IObject? element, bool noReferences = false)
        {
            if (element == null)
            {
                return "null";
            }

            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            if (element.isSet(_UML._CommonStructure._NamedElement.name))
            {
                return element.getOrDefault<string>(_UML._CommonStructure._NamedElement.name, noReferences);
            }

            switch (element)
            {
                case IHasId elementAsHasId:
                    return elementAsHasId.Id ?? string.Empty;
                case IElement asIElement when asIElement.metaclass != null:
                {
                    var name = GetName(asIElement.metaclass, noReferences);
                    if (name != null && !string.IsNullOrEmpty(name))
                    {
                        return $"({name})";
                    }

                    break;
                }
            }

            return element switch
            {
                MofObjectShadow shadowedObject => shadowedObject.Uri,
                MofObject _ => "MofObject",
                _ => element.ToString()
            };
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