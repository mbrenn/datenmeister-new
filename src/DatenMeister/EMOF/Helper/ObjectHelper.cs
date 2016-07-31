using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Helper
{
    /// <summary>
    ///     Includes several methods to support the handling of objects
    /// </summary>
    public static class ObjectHelper
    {
        public static Dictionary<object, object> AsDictionary(
            this IObject value,
            IEnumerable<string> properties)
        {
            var result = new Dictionary<object, object>();

            foreach (var property in properties
                .Where(property => value.isSet(property)))
            {
                result[property] = value.get(property);
            }

            return result;
        }

        public static Dictionary<string, string> AsStringDictionary(
            this IObject value,
            IEnumerable<string> properties)
        {
            var result = new Dictionary<string, string>();

            foreach (var property in properties
                .Where(property => value.isSet(property)))
            {
                var propertyValue = value.get(property);
                result[property.ToString()] = propertyValue == null ? "null" : propertyValue.ToString();
            }

            return result;
        }
        /// <summary>
        /// Tries to retrieve the extent as given by the implemented interface
        /// IObjectKnowsExtent. If the interface is not implemented by the root element
        /// of the given element, the method will return a failure
        /// </summary>
        /// <param name="value">Value, which is queried</param>
        /// <returns></returns>
        public static IExtent GetExtentOf(this IObject value)
        {
            // If the object is contained by another object, query the contained objects 
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetExtentOf(parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IObjectKnowsExtent;
            if (objectKnowsExtent != null)
            {
                return objectKnowsExtent.Extents.FirstOrDefault() as IUriExtent;
            }
            else
            {
                throw new ArgumentException($"The following element does not implement the IObjectKnowsExtent interface: {value}");
            }
        }

        public static IUriExtent GetUriExtentOf(this IObject value)
        {
            var result = GetExtentOf(value);
            if (result == null)
            {
                return null;
            }

            // Checks, if the given result is a uriextent
            var resultAsUriExtent = result as IUriExtent;
            if (resultAsUriExtent == null)
            {
                throw new InvalidOperationException($"The returned extent is not an IUriExtent {result}");
            }

            return resultAsUriExtent;
        }

        /// <summary>
        /// Gets the uri of a certain element. 
        /// </summary>
        /// <param name="element">Element whose uri is queried</param>
        /// <returns>Uri of the element</returns>
        public static string GetUri(this IElement element)
        {
            return element.GetUriExtentOf()?.uri(element);
        }

        /// <summary>
        /// Queries the property 'property' of the value and expects a list that can be enumerated. 
        /// After that, the property 'propertyOfChild' is evaluated and checked against the requested value. 
        /// If the value of the propertyOfChild is the same as 'requestValue', it will be returned.
        /// </summary>
        /// <param name="value">Value, whose property list shall be queried</param>
        /// <param name="property">The property that is used to retrieve the list</param>
        /// <param name="propertyOfChild">Each item of the list is enumerated against the list</param>
        /// <param name="requestValue">The value that is looked for</param>
        /// <returns>The list of found elements</returns>
        public static IEnumerable<IObject> GetByPropertyFromCollection(
            this IElement value,
            string property,
            string propertyOfChild,
            object requestValue)
        {
            var valueOfProperty = value.get(property);
            if (valueOfProperty == null)
            {
                // Nothing has been found, so return null
                yield break;
            }

            var asEnumeration = (valueOfProperty as IEnumerable)?.Cast<object>();
            if (asEnumeration == null)
            {
                throw new InvalidOperationException("The value behind the property is not an enumeration");
            }

            foreach (var x in asEnumeration)
            {
                var asElement = x as IObject;
                var valueOfChild = asElement?.get(propertyOfChild);
                if (valueOfChild?.Equals(requestValue) == true)
                {
                    yield return asElement;
                }
            }
        }
    }
}