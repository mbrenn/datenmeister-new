using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Uml.Helper
{
    public class NamedElementMethods
    {
        private readonly IUmlNameResolution _umlNameResolution;

        public NamedElementMethods(IUmlNameResolution umlNameResolution)
        {
            _umlNameResolution = umlNameResolution;
        }

        /// <summary>
        /// Gets the full path to the given element. It traverses through the container values of the
        /// objects and retrieves the partial names by 'name'.
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>Full name of the element</returns>
        public string GetFullName(IElement value)
        {
            var current = value.container();
            var result = _umlNameResolution.GetName(value);
            var depth = 0;

            while (current != null)
            {
                var currentName = _umlNameResolution.GetName(current);
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
        /// Gets the given element by the fullname by traversing through the name attributes
        /// </summary>
        /// <param name="collection">Collection to be queried, could also be an extent reflective collection</param>
        /// <param name="fullName">Fullname to be traversed, each element shall be separated by an ':'</param>
        /// <returns>The found element or null, if not found</returns>
        public IElement GetByFullName(IReflectiveCollection collection, string fullName)
        {
            var elementNames = fullName.Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            var current = (IEnumerable<object>) collection;
            IElement found = null;

            foreach (var elementName in elementNames)
            {
                foreach (var currentValue in current
                    .Select(x => x as IElement)
                    .Where(x => x != null))
                {
                    var name = _umlNameResolution.GetName(currentValue);
                    if (name == elementName)
                    {
                        // We found the element, now abort the search and look in its properties
                        found = currentValue;
                        break;
                    }
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
        private IEnumerable<IElement> GetAllPropertyValues(IElement value)
        {
            var asProperties = value as IObjectAllProperties;
            if (asProperties == null)
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
                    var asElement = innerValue as IElement;
                    if (asElement != null)
                    {
                        // and inner value is an IElement
                        yield return asElement;
                    }
                }
            }
        }
    }
}