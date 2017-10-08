using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static class NamedElementMethods
    {
        /// <summary>
        /// Gets the full path to the given element. It traverses through the container values of the
        /// objects and retrieves the partial names by 'name'.
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>Full name of the element</returns>
        public static string GetFullName(IElement value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var current = value.container();
            var result = UmlNameResolution.GetName(value);
            var depth = 0;

            while (current != null)
            {
                var currentName = UmlNameResolution.GetName(current);
                result = $"{currentName}::{result}";
                current = current.container();
                depth++;

                if (depth > 1000)
                {
                    throw new InvalidOperationException(
                        $"The full name of the element {value} could not be retrieved due to an infinite loop. (Threshold is 1000)");
                }
            }

            return result;
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
        public static IElement GetByFullName(IUriExtent extent, string fullName)
        {
            return GetByFullName(extent.elements(), fullName);
        }

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
                    var name = UmlNameResolution.GetName(currentValue);
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
        private static IEnumerable<IElement> GetAllPropertyValues(IElement value)
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
        /// Gets or create the package structure and returns the last created element.
        /// The elements will be interspaced by '::'
        /// </summary>
        /// <param name="rootElements">Elements which contain the root elements</param>
        /// <param name="factory">Factory being used to create the subitems</param>
        /// <param name="packagePath">Path of the package to be created</param>
        /// <param name="nameProperty">The name property which contain the name for the element</param>
        /// <param name="childProperty">The child property which contain the subelements</param>
        /// <param name="metaClassPackage"></param>
        public static IElement GetOrCreatePackageStructure(
            IReflectiveSequence rootElements, 
            IFactory factory,
            string packagePath, 
            string nameProperty,
            string childProperty, 
            string metaClassPackage)
        {

            var elementNames = packagePath
                .Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            IElement found = null;

            foreach (var elementName in elementNames)
            {
                // Looks for the element with the given name
                IElement childElement = null;
                foreach (var innerElement in rootElements.OfType<IElement>())
                {
                    if (innerElement.isSet(nameProperty))
                    {
                        continue;
                    }

                    if (innerElement.get(nameProperty).ToString() == elementName)
                    {
                        childElement = innerElement;
                    }
                }

                // Creates the child element
                if (childElement == null)
                {
                    childElement = factory.create(null);
                    childElement.set(nameProperty, elementName);
                    rootElements.add(childElement);
                }

                // Sets and finds the child property
                IReflectiveSequence children = null;
                if (childElement.isSet(childProperty))
                {
                    children = childElement.get(childProperty) as IReflectiveSequence;
                }

                if (children == null)
                {
                    childElement.set(childProperty, new List<object>());
                    children = childElement.get(childProperty) as IReflectiveSequence;
                }

                rootElements = children;
                found = childElement;

            }

            return found;
        }
    }
}