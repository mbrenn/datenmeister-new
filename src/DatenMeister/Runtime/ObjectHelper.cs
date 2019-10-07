using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Runtime
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Gets the given property value as a single element, if the get function returns
        /// a collection or enumeration
        /// </summary>
        /// <param name="value">Object to be queried</param>
        /// <param name="property">Property to be queried</param>
        /// <returns>The given and singlelized element, if there is just one element in the enumeration</returns>
        private static object GetAsSingle(this IObject value, string property)
        {
            var propertyValue = value.get(property);
            if (propertyValue is IEnumerable<object> asObjectList)
            {
                var list = asObjectList.ToList();
                if (list.Count == 1)
                {
                    return list[0];
                }

                return null;
            }

            return propertyValue;
        }

        public static bool IsPropertyOfType<T>(this IObject value, string property)
        {
            if (value.isSet(property))
            {
                if (value.get(property) is T)
                {
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// Gets the typed value of the property.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="value">MOF Object being queried</param>
        /// <param name="property">Property being queried in the <c>value</c></param>
        /// <returns>The content of the element or default(T) if not nknown</returns>
        public static T get<T>(this IObject value, string property)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (typeof(T) == typeof(string))
            {
                return (T)(object)DotNetHelper.AsString(value.GetAsSingle(property));
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)DotNetHelper.AsInteger(value.GetAsSingle(property));
            }

            if (typeof(T) == typeof(double))
            {
                return (T)(object)DotNetHelper.AsDouble(value.GetAsSingle(property));
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)DotNetHelper.AsBoolean(value.GetAsSingle(property));
            }

            if (typeof(T) == typeof(IObject))
            {
                return (T)(value.GetAsSingle(property) as IObject);
            }

            if ( typeof(T) == typeof(IElement))
            {
                return (T)(value.GetAsSingle(property) as IElement);
            }

            if (typeof(T) == typeof(IReflectiveCollection))
            {
                return (T) (object) new MofReflectiveSequence((MofObject) value, property);
            }

            if (typeof(T) == typeof(IReflectiveSequence))
            {
                return (T)(object)new MofReflectiveSequence((MofObject)value, property);
            }

            if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(value.GetAsSingle(property).ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                {
                    return (T)(object) result;
                }

                return (T)(object) DateTime.MinValue;
            }

            if (typeof(T).IsEnum)
            {
                var valueAsElement = value.GetAsSingle(property);
                if (valueAsElement == null)
                {
                    return default(T);
                }

                if (typeof(T) == valueAsElement.GetType())
                {
                    return (T) valueAsElement;
                }

                if (valueAsElement is string propertyValueAsString)
                {
                    return (T) Enum.Parse(typeof(T), propertyValueAsString);
                }

                if (valueAsElement is IElement propertyObject && value is MofObject mofObject)
                {
                    // Get Enumeration Instance
                    var resolvedElement = mofObject.ReferencedExtent.Resolve(propertyObject);
                    if (resolvedElement == null)
                    {
                        return default(T);
                    }

                    var name = NamedElementMethods.GetName(resolvedElement);
                    return (T) Enum.Parse(typeof(T), name);
                }
            }

            throw new InvalidOperationException($"{typeof(T).FullName} is not handled by get");
        }

        public static T? getOrNull<T>(this IObject value, string property) where T : struct
        {
            if (!value.isSet(property))
            {
                return null;
            }

            return get<T>(value, property);
        }

        public static T getOrDefault<T>(this IObject value, string property)
        {
            if (!value.isSet(property))
            {
                return default(T);
            }

            return get<T>(value, property);
        }

        /// <summary>
        /// Gets the value of a property if the property is set.
        /// If the property is no set, then null will be returned
        /// </summary>
        /// <param name="value">Object being queried</param>
        /// <param name="property">Property of the object</param>
        /// <returns>The value of the object or null, if not existing</returns>
        public static object GetOrDefault(this IObject value, string property)
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
        public static object GetFirstOrDefault(this IObject value, string property)
        {
            if (value.isSet(property))
            {
                var result = value.get(property);
                if (DotNetHelper.IsOfEnumeration(result))
                {
                    var resultAsEnumeration = (IEnumerable<object>) result;
                    return resultAsEnumeration.FirstOrDefault();
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Adds a list item to the reflective sequence of the given value.
        /// If the given property is not already a reflective sequence, it will become to one
        /// </summary>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <param name="toBeAdded"></param>
        public static void AddCollectionItem(this IObject value, string property, object toBeAdded)
        {
            var reflection = new MofReflectiveSequence((MofObject) value, property);
            reflection.add(toBeAdded);
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
        /// Sets the properties of the value
        /// </summary>
        /// <param name="value">Object which will receive the values</param>
        /// <param name="properties">Properties to be set</param>
        public static IObject SetProperties(this IObject value, IDictionary<string, object> properties)
        {
            foreach (var pair in properties)
            {
                value.set(pair.Key, pair.Value);
            }

            return value;
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
            var result = value.getOrDefault<IReflectiveCollection>(property);
            result = result ?? CreateReflectiveCollectionObject(value, property);

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
            var result = value.getOrDefault<IReflectiveSequence>(property);
            result = result ?? (IReflectiveSequence) CreateReflectiveCollectionObject(value, property);

            return result;
        }

        /// <summary>
        /// Gets the reflective collection of a property of the mof object
        /// </summary>
        /// <param name="mofObject">Mof Object whose property shall be considered as a reflective collection</param>
        /// <param name="property">Name of the property</param>
        /// <returns>The reflective collection containing the property</returns>
        private static IReflectiveCollection CreateReflectiveCollectionObject(this IObject mofObject, string property)
            => new MofReflectiveSequence((MofObject)mofObject, property);

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
            if (!(value.get(property) is IEnumerable<object> result))
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
        public static IEnumerable<object> ForceAsEnumerable(
            this IObject value,
            string property)
        {
            if (!value.isSet(property))
            {
                // If value is not set, an empty list is returned
                return new object[] { };
            }

            var result = value.get(property);
            if (result is IEnumerable<object> resultAsEnumerable)
            {
                // Ok, directly return it as enumerable
                return resultAsEnumerable;
            }

            return result == null ? new object[]{} : new[] {result};
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
            if (!(value is IObject result))
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
            if (!(value is IElement result))
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
        /// <returns>The found extent</returns>
        public static IExtent GetExtentOf(this IObject value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            // If the given object is already an extent... happy life
            if (value is IExtent asExtent)
            {
                return asExtent;
            }

            // If the object is contained by another object, query the contained objects
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetExtentOf(parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            if (value is IHasExtent objectKnowsExtent)
            {
                return objectKnowsExtent.Extent as IUriExtent;
            }

            return null;
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
            if (!(result is IUriExtent resultAsUriExtent))
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
            // First, verifies if the element has direct access to the uri of the element
            if (element is IKnowsUri asKnowsUri)
            {
                return asKnowsUri.Uri;
            }

            // If not, try to get the information via the extent which owns the object
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

        public static IUriResolver GetUriResolver(this IObject element)
        {
            return (element as MofObject)?.Extent as IUriResolver;
        }

        public static IUriResolver GetUriResolver(this IExtent element)
        {
            return element as IUriResolver;
        }

        /// <summary>
        /// Gets all possible properties of the given element.
        /// If the element has a metaclass (or some other Classifier), the properties
        /// of the metaclass. Otherwise, the set properties will be returned
        /// </summary>
        /// <param name="value">Element, whose properties will be queried</param>
        /// <returns>Enumeration of properties</returns>
        public static IEnumerable<string> GetPropertyNames(IObject value)
        {
            switch (value)
            {
                case IElement element when element.metaclass != null:
                    return ClassifierMethods.GetPropertyNamesOfClassifier(element.metaclass);

                case IObjectAllProperties knowsProperties:
                    return knowsProperties.getPropertiesBeingSet();

                default:
                    return Array.Empty<string>();
            }
        }
    }
}