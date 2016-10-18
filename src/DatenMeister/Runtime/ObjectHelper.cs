using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Gets the value of a property if the property is set. 
        /// If the property is no set, then null will be returned
        /// </summary>
        /// <param name="value">Object being queried</param>
        /// <param name="property">Property of the object</param>
        /// <returns>The value of the object or null, if not existing</returns>
        public static object getOrDefault(this IObject value, string property)
        {
            if (value.isSet(property))
            {
                return value.get(property);
            }

            return null;
        }
        /// <summary>
        /// Gets the value of a property if the property is set and is not an enumeration. 
        /// If the property is an enumeration, the first element will be returned
        /// If the property is no set, then null will be returned
        /// </summary>
        /// <param name="value">Object being queried</param>
        /// <param name="property">Property of the object</param>
        /// <returns>The value of the object or null, if not existing</returns>
        public static object getFirstOrDefault(this IObject value, string property)
        {
            if (value.isSet(property))
            {
                var result = value.get(property);
                if (DotNetHelper.IsOfEnumeration(result))
                {
                    var resultAsEnumeration = result as IEnumerable<object>;
                    return resultAsEnumeration.FirstOrDefault();
                }

                return result;
            }

            return null;
        }

        public static Dictionary<object, object> AsDictionary(
            this IObject value,
            IEnumerable<string> properties)
        {
            var result = new Dictionary<object, object>();

            foreach (var property in properties
                .Where(value.isSet))
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
                .Where(value.isSet))
            {
                var propertyValue = value.get(property);
                result[property] = propertyValue == null ? "null" : propertyValue.ToString();
            }

            return result;
        }

        /// <summary>
        /// Gets a certain property value as a reflective collection. 
        /// If the value is not a reflective collection, an exception is thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <param name="property">Property that is access</param>
        /// <returns>The reflective collection or an exception if the property is not
        /// a reflective collection</returns>
        public static IReflectiveCollection GetAsReflectiveCollection(
            this IObject value,
            string property)
        {
            var result = value.get(property) as IReflectiveCollection;
            if (result == null)
            {
                throw new InvalidOperationException("The given result is not a ReflectiveCollection");
            }

            return result;
        }

        /// <summary>
        /// Gets a certain property value as a reflective sequence. 
        /// If the value is not a reflective sequence, an exception is thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <param name="property">Property that is access</param>
        /// <returns>The reflective sequence or an exception if the property is not
        /// a reflective collection</returns>
        public static IReflectiveSequence GetAsReflectiveSequence(
            this IObject value,
            string property)
        {
            var result = value.get(property) as IReflectiveSequence;
            if (result == null)
            {
                throw new InvalidOperationException("The given result is not a ReflectiveSequence");
            }

            return result;
        }

        /// <summary>
        /// Gets a certain property value as a reflective sequence.
        /// If the value is not a reflective sequence, an exception is thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <param name="property">Property that is access</param>
        /// <returns>The reflective sequence or an exception if the property is not
        /// a reflective collection</returns>
        public static IEnumerable<object> GetAsEnumerable(
            this IObject value,
            string property)
        {
            var result = value.get(property) as IEnumerable<object>;
            if (result == null)
            {
                throw new InvalidOperationException("The given result is not a ReflectiveSequence");
            }

            return result;
        }

        /// <summary>
        /// Returns the value as an IObject. 
        /// If the object is not an IObject, an exception is thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The converted object</returns>
        public static IObject AsIObject(
            this object value)
        {
            var result = value as IObject;
            if (result == null)
            {
                throw new InvalidOperationException("The given value is not an IObject");
            }

            return result;
        }

        /// <summary>
        /// Returns the value as an IObject. 
        /// If the object is not an IObject, an exception is thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The converted object</returns>
        public static IElement AsIElement(
            this object value)
        {
            var result = value as IElement;
            if (result == null)
            {
                throw new InvalidOperationException("The given value is not an IElement");
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

            throw new ArgumentException($"The following element does not implement the IObjectKnowsExtent interface: {value}");
        }

        /// <summary>
        /// Gets the extent of the given object as IUriExtent interface object. 
        /// If the uriextent cannot be retrieved due to object incompatibilities, 
        /// an exception will be thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>null or the given extent</returns>
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

            foreach (var obj in GetByPropertyFromCollection(asEnumeration, propertyOfChild, requestValue))
            {
                yield return obj;
            }
        }

        /// <summary>
        /// Gets the enumeration of all object that have a certain property value in one of its children
        /// </summary>
        /// <param name="asEnumeration">The enumeration being queried</param>
        /// <param name="propertyOfChild">Property that is queried</param>
        /// <param name="requestValue">The value, that is used as a validation against the property</param>
        /// <returns>Enumeration of objects</returns>
        public  static IEnumerable<IObject> GetByPropertyFromCollection(
            this IEnumerable<object> asEnumeration, 
            string propertyOfChild, 
            object requestValue)
        {
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

        /// <summary>
        /// Is true
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>True, if value indicates a true statement</returns>
        public static bool IsTrue(object value)
        {
            return value.Equals(true) ||
                   value.Equals(1) ||
                   value.Equals("true") ||
                   value.Equals("TRUE");
        }
    }
}