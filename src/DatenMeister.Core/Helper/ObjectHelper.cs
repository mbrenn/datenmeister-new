using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Uml.Helper;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Core.Helper
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Gets the given property value as a single element, if the get function returns
        /// a collection or enumeration
        /// </summary>
        /// <param name="value">Object to be queried</param>
        /// <param name="property">Property to be queried</param>
        /// <param name="noReferences">Flag, if no recursion shall occur</param>
        /// <param name="objectType">Defines the object type</param>
        /// <returns>The given and singlelized element, if there is just one element in the enumeration</returns>
        private static object? GetAsSingle(
            this IObject value, 
            string property,
            bool noReferences = false,
            ObjectType objectType = ObjectType.None)
        {
            object? propertyValue;
            if (value is MofObject valueAsMofObject)
            {
                propertyValue = valueAsMofObject.get(property, noReferences, objectType);
            }
            else
            {
                propertyValue = value.get(property);
            }

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

        #nullable disable
        
        /// <summary>
        /// Gets the typed value of the property.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="value">MOF Object being queried</param>
        /// <param name="property">Property being queried in the <c>value</c></param>
        /// <param name="noReferences">true, if references shall not be </param>
        /// <returns>The content of the element or default(T) if not known</returns>
        public static T get<T>(this IObject value, string property, bool noReferences = false)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (typeof(T) == typeof(object) && value is MofExtent)
            {
                // ReSharper disable HeuristicUnreachableCode
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (!(value is IHasMofExtentMetaObject metaObject))
                    throw new NotImplementedException("Unfortunately not supported: " + value.GetType());
                // ReSharper restore HeuristicUnreachableCode

                return (T) metaObject.GetMetaObject().get(property, noReferences, ObjectType.None)!;
            }

            if (typeof(T) == typeof(object) && value is MofObject mofObject2)
            {
                return (T) mofObject2.get(property, noReferences, ObjectType.None)!;
            }

            if (typeof(T) == typeof(string))
            {
                return (T) (object) DotNetHelper.AsString(value.GetAsSingle(property, noReferences, ObjectType.String)!)!;
            }

            if (typeof(T) == typeof(int))
            {
                return (T) (object) DotNetHelper.AsInteger(value.GetAsSingle(property, noReferences, ObjectType.Integer)!)!;
            }

            if (typeof(T) == typeof(int?))
            {
                return (T) (object) DotNetHelper.AsInteger(value.GetAsSingle(property, noReferences, ObjectType.Integer)!)!;
            }

            if (typeof(T) == typeof(double))
            {
                return (T) (object) DotNetHelper.AsDouble(value.GetAsSingle(property, noReferences, ObjectType.Double)!)!;
            }

            if (typeof(T) == typeof(bool))
            {
                return ((T) (object) DotNetHelper.AsBoolean(value.GetAsSingle(property, noReferences, ObjectType.Boolean)))!;
            }

            if (typeof(T) == typeof(char))
            {
                var asString = DotNetHelper.AsString(value.GetAsSingle(property, noReferences, ObjectType.String)!)!;
                if (string.IsNullOrEmpty(asString))
                {
                    return (T) (object) ' ';
                }

                return (T) (object) asString[0];
            }

            if (typeof(T) == typeof(IObject))
            {
                var asSingle = (value.GetAsSingle(property, noReferences, ObjectType.Element) as IObject)!;
                return ((T) asSingle)!;
            }

            if (typeof(T) == typeof(IElement))
            {
                var asSingle = (value.GetAsSingle(property, noReferences, ObjectType.Element) as IElement)!;
                return asSingle is MofObjectShadow ? default : (T) asSingle;
            }

            if (typeof(T) == typeof(IReflectiveCollection))
            {
                if (!(value is IHasMofExtentMetaObject metaObject))
                    throw new NotImplementedException("Unfortunately not supported: " + value.GetType());
                
                return (T) (object) new MofReflectiveSequence(metaObject.GetMetaObject(), property)
                {
                    NoReferences = noReferences
                };
            }

            if (typeof(T) == typeof(IReflectiveSequence))
            {
                if (!(value is IHasMofExtentMetaObject metaObject))
                    throw new NotImplementedException("Unfortunately not supported: " + value.GetType());
                
                return (T) (object) new MofReflectiveSequence(metaObject.GetMetaObject(), property)
                {
                    NoReferences = noReferences
                };
            }

            if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(
                    value.GetAsSingle(property, noReferences, ObjectType.DateTime)?.ToString() 
                        ?? DateTime.MinValue.ToString(CultureInfo.InvariantCulture),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var result))
                {
                    return (T) (object) result;
                }

                return (T) (object) DateTime.MinValue;
            }

            if (typeof(T) == typeof(object))
            {
                return ((T) value.GetAsSingle(property, noReferences))!;
            }

            if (typeof(T).IsEnum)
            {
                var valueAsElement = value.GetAsSingle(property, noReferences, ObjectType.Enum);
                if (valueAsElement == null)
                {
                    return default;
                }

                if (typeof(T) == valueAsElement.GetType())
                {
                    return (T) valueAsElement;
                }

                if (valueAsElement is string propertyValueAsString)
                {
                    try
                    {
                        return (T) Enum.Parse(typeof(T), propertyValueAsString);
                    }
                    catch
                    {
                        return default;
                    }
                }

                if (valueAsElement is IElement propertyObject && value is MofObject mofObject)
                {
                    // Get Enumeration Instance
                    var resolvedElement = mofObject.ReferencedExtent.Resolve(propertyObject);
                    if (resolvedElement == null)
                    {
                        return default;
                    }

                    var name = NamedElementMethods.GetName(resolvedElement);
                    return (T) Enum.Parse(typeof(T), name);
                }
            }

            throw new InvalidOperationException($"{typeof(T).FullName} is not handled by get");
        }
        
        #nullable enable

        public static T? getOrNull<T>(this IObject value, string property, bool noReferences = false) where T : struct
        {
            if (!value.isSet(property))
            {
                return null;
            }

            return get<T>(value, property, noReferences);
        }

        #nullable disable
        
        public static T getOrDefault<T>(this IObject value, string property, bool noReferences = false)
        {
            if (!value.isSet(property))
            {
                return default;
            }

            return get<T>(value, property, noReferences);
        }
        
        #nullable enable

        /// <summary>
        /// Gets the value of a property if the property is set and is not an enumeration.
        /// If the property is an enumeration, the first element will be returned
        /// If the property is no set, then null will be returned
        /// </summary>
        /// <param name="value">Object being queried</param>
        /// <param name="property">Property of the object</param>
        /// <returns>The value of the object or null, if not existing</returns>
        public static object? getFirstOrDefault(this IObject value, string property)
        {
            if (value.isSet(property))
            {
                var result = value.get(property);
                if (DotNetHelper.IsOfEnumeration(result))
                {
                    var resultAsEnumeration = (IEnumerable<object>) result!;
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

        /// <summary>
        /// Removes a list item to the reflective sequence from the given value.
        /// If the given property is not already a reflective sequence, it will become to one
        /// </summary>
        /// <param name="value">Value to be updated</param>
        /// <param name="property">Property whose Reflective Collection shall be modified</param>
        /// <param name="toBeRemoved">Element to be removed</param>
        /// <returns>true, if removal has been successful</returns>
        public static bool RemoveCollectionItem(this IObject value, string property, object toBeRemoved)
        {
            var reflection = new MofReflectiveSequence((MofObject) value, property);
            return reflection.remove(toBeRemoved);
        }
       
        public static Dictionary<object, object?> AsDictionary(
            this IObject value,
            IEnumerable<string> properties)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var result = new Dictionary<object, object?>();

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
                result[property] = propertyValue?.ToString() ?? "null";
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
        /// Sets the properties of the value
        /// </summary>
        /// <param name="value">Object which will receive the values</param>
        /// <param name="properties">Properties to be set</param>
        public static IElement SetProperties(this IElement value, IDictionary<string, object> properties)
        {
            SetProperties((IObject) value, properties);
            return value;
        }

        /// <summary>
        /// Sets the properties of the value
        /// </summary>
        /// <param name="element">Object which will receive the values</param>
        /// <param name="key">key of the property to be set</param>
        /// <param name="value">Value to be set</param>
        public static IElement SetProperty(this IElement element, string key, object value)
        {
            element.set(key, value);
            return element;
        }

        /// <summary>
        /// Sets the id and returns the element itself to allow chaining
        /// </summary>
        /// <param name="element">Element to be updated</param>
        /// <param name="id">Id to be set </param>
        /// <returns>The element itself</returns>
        public static IObject SetId(this IObject element, string id)
        {
            if (element is ICanSetId elementAsHasId)
            {
                elementAsHasId.Id = id;
            }

            return element;
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

            return result == null ? new object[] { } : new[] {result};
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
        /// Gets the extent of the given element
        /// </summary>
        /// <param name="hasExtent">The interface being able to request the extent</param>
        /// <returns>The found extent</returns>
        public static IExtent? GetExtentOf(this IHasExtent hasExtent) => hasExtent.Extent;

        /// <summary>
        /// Tries to retrieve the extent as given by the implemented interface
        /// IObjectKnowsExtent. If the interface is not implemented by the root element
        /// of the given element, the method will return a failure
        /// </summary>
        /// <param name="value">Value, which is queried</param>
        /// <returns>The found extent</returns>
        public static IExtent? GetExtentOf(this IObject value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            // If the given object is already an extent... happy life
            if (value is IExtent asExtent)
            {
                return asExtent;
            }

            // If the object knows the extent to which it belongs to, it will return it
            if (value is IHasExtent objectKnowsExtent)
            {
                return objectKnowsExtent.Extent as IUriExtent;
            }

            // If the object is contained by another object, query the contained objects
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetExtentOf(parent);
            }

            return null;
        }

        /// <summary>
        /// Gets the metaclass without triggering a trace logging message in case the element behind is not
        /// found
        /// </summary>
        /// <param name="element">Element to be queried</param>
        /// <returns>The element to be retrieved</returns>
        public static IElement? getMetaClassWithoutTracing(this IElement element)
        {
            return element is MofElement asObject ? asObject.getMetaClass(false) : element.metaclass;
        }

        /// <summary>
        /// Gets the extent of the given object as IUriExtent interface object.
        /// If the uriextent cannot be retrieved due to object incompatibilities,
        /// an exception will be thrown
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>null or the given extent</returns>
        public static IUriExtent? GetUriExtentOf(this IObject value)
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

        public static string? GetUri(this IObject objectElement)
        {
            if (objectElement is IUriExtent uriExtent)
            {
                return uriExtent.contextURI();
            }

            if (objectElement is IElement asElement)
            {
                return asElement.GetUri();
            }

            return null;
        }

        /// <summary>
        /// Gets the uri of a certain element.
        /// </summary>
        /// <param name="element">Element whose uri is queried</param>
        /// <returns>Uri of the element</returns>
        public static string? GetUri(this IElement element)
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
            var valueOfProperty = value.getOrDefault<IReflectiveCollection>(property);
            if (valueOfProperty == null)
            {
                // Nothing has been found, so return null
                yield break;
            }

            var asEnumeration = valueOfProperty.Cast<object>();
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
        public static IEnumerable<IObject> GetByPropertyFromCollection(
            this IEnumerable<object> asEnumeration,
            string propertyOfChild,
            object requestValue)
        {
            foreach (var x in asEnumeration)
            {
                var asElement = x as IObject;
                var valueOfChild = asElement?.getOrDefault<object>(propertyOfChild);
                if (valueOfChild != null && valueOfChild.Equals(requestValue) && asElement != null)
                {
                    yield return asElement;
                }
            }
        }

        public static IUriResolver? GetUriResolver(this IObject element) =>
            (element as MofObject)?.Extent as IUriResolver;

        public static IUriResolver GetUriResolver(this IExtent element) =>
            (element as IUriResolver) ?? throw new InvalidOperationException("element is not of type IUriResolver");

        /// <summary>
        /// Gets all possible properties of the given element.
        /// If the element has a metaclass (or some other Classifier), the properties
        /// of the metaclass. Otherwise, the set properties will be returned
        /// </summary>
        /// <param name="value">Element, whose properties will be queried</param>
        /// <returns>Enumeration of properties</returns>
        public static IEnumerable<string> GetPropertyNames(IObject value)
        {
            var list = new List<string>();

            if (value is IElement element)
            {
                var metaClass = element.getMetaClassWithoutTracing();
                if (metaClass != null)
                {
                    list.AddRange(ClassifierMethods.GetPropertyNamesOfClassifier(metaClass));
                }
            }

            if (value is IObjectAllProperties knowsProperties)
            {
                list.AddRange( knowsProperties.getPropertiesBeingSet());
            }

            return list.Distinct();
        }
        
        /// <summary>
        /// Deletes an object from the database.
        /// If the object is a root element, then it will be directly removed from the extent.
        /// If the object is a child item of an existing item, then it will be removed
        /// the parent item
        /// </summary>
        /// <param name="value">Element to be added</param>
        /// <returns>Value whether a deletion was done</returns>
        public static bool DeleteObject(IObject? value)
        {
            if (value == null)
            {
                return false;                
            }
            
            // First, checks container item.... 
            var container = (value as IElement)?.container();
            var ofProperties = (container as IObjectAllProperties)?.getPropertiesBeingSet();
            if (ofProperties != null && container != null)
            {
                foreach (var property in ofProperties)
                {
                    var propertyValue = container.getOrDefault<object>(property);
                    if (propertyValue is IObject propertyValueAsObject)
                    {
                        if (value.@equals(propertyValueAsObject))
                        {
                            // We found it, so we will set it as zero. 
                            // This means that the reference is now lost
                            container.set(property, null);
                            return true;
                        }
                    }

                    if (propertyValue is IReflectiveCollection collection)
                    {
                        // Check, if the reflection collection can delete the given item. 
                        // Here, we rely on the return value itself
                        var success = collection.remove(value);
                        if (success)
                        {
                            return true;
                        }
                    }
                }
            }

            // Second, check whether the element contains to an extent
            var extent = (value as IHasExtent)?.Extent
                         ?? ((value as IElement)?.container() as IExtent);
            if (extent is not null)
            {
                // Removes the element from the extent 
                return extent.elements().remove(value);
            }
            
            return false;
        }
    }
}