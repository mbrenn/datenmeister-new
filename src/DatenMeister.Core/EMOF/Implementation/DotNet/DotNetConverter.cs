using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation.DotNet
{
    /// <summary>
    /// A supporting class, that converts a .Net Element to a MOF object or a MOF object to a .Net Object
    /// </summary>
    public class DotNetConverter
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DotNetConverter));

        /// <summary>
        /// Stores the MOF Factory being used to create the MOF object. This is dependent upon the extent.
        /// </summary>
        private readonly MofFactory _factory;

        /// <summary>
        /// Stores a list of already visited elements, so a recursion is avoided, when a conversion from .Net Object to Mof Object is executed.
        /// This also assumes that the converter is not re-entrant
        /// </summary>
        private readonly HashSet<object> _visitedElements = new HashSet<object>();

        /// <summary>
        /// Stores the resolver to find metaclasses by .Net Types
        /// </summary>
        private readonly MofUriExtent _extent;

        /// <summary>
        /// Initializes a new instance of the DotNetConverter class
        /// </summary>
        /// <param name="extent">Extent being used as reference to find typeLookup and Resolver</param>
        private DotNetConverter(MofUriExtent extent)
        {
            _factory = new MofFactory(extent);
            _extent = extent;
        }

        /// <summary>
        /// Converts the given object and stores it into the receiver's method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        private object? ConvertToMofIfNotPrimitive(object? value, string? requestedId = null)
        {
            if (DotNetHelper.IsOfPrimitiveType(value) || DotNetHelper.IsOfEnum(value))
            {
                return value;
            }

            switch (value)
            {
                case null:
                    return null;
                case IObject _:
                    return value;
            }

            // Check, if the element already existed
            if (_visitedElements.Contains(value))
            {
                return null;
            }

            _visitedElements.Add(value);

            // Gets the uri of the lookup up type
            var metaClassUri = _extent.GetMetaClassUri(value.GetType());
            var metaClass = metaClassUri == null ? null : _extent.ResolveElement(metaClassUri, ResolveType.OnlyMetaClasses);

            return ConvertToMofObject(value, metaClass, requestedId);
        }

        /// <summary>
        /// Converts the given .Net Object in value to a MofObject
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="metaClass">Metaclass being used to create the element</param>
        /// <param name="requestedId">Id of the element that shall be put</param>
        /// <returns>The converted element as a MofObject</returns>
        private IObject ConvertToMofObject(object value, IElement? metaClass = null, string? requestedId = null)
        {
            // After having the uri, create the required element
            var createdElement = _factory.create(metaClass);
            if (requestedId != null && !string.IsNullOrEmpty(requestedId) && createdElement is ICanSetId canSetId)
            {
                canSetId.Id = requestedId;
            }

            var type = value.GetType();
            foreach (var reflectedProperty in type.GetProperties(
                BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public))
            {
                var innerValue = reflectedProperty.GetValue(value);
                if (DotNetHelper.IsOfEnumeration(innerValue) && innerValue != null)
                {
                    var list = new List<object>();
                    var enumeration = (IEnumerable) innerValue;
                    foreach (var innerElementValue in enumeration)
                    {
                        var convertedValue = ConvertToMofIfNotPrimitive(innerElementValue);
                        if (convertedValue != null)
                        {
                            list.Add(convertedValue);
                        }
                    }

                    createdElement.set(reflectedProperty.Name, list);
                }
                else
                {
                    createdElement.set(reflectedProperty.Name, ConvertToMofIfNotPrimitive(innerValue));
                }
            }

            return createdElement;
        }

        /// <summary>
        /// Sets the given object into the MofObject.
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        public static object? ConvertToMofObject(MofUriExtent receiver, object value, string? requestedId = null)
            => new DotNetConverter(receiver).ConvertToMofIfNotPrimitive(value, requestedId);

        /// <summary>
        /// Sets the given object into the MofObject.
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        public static object? ConvertToMofObject(IUriExtent receiver, object value, string? requestedId = null)
            => ConvertToMofObject((MofUriExtent) receiver, value, requestedId);

        /// <summary>
        /// Converts the given .Net Object in value to a MofObject
        /// </summary>
        /// <param name="receiver">Extent being used to receive the newly created object</param>
        /// <param name="value">Value to be converted</param>
        /// <param name="metaclass">Metaclass being used to create the element</param>
        /// <param name="requestedId">Id of the element that shall be put</param>
        /// <returns>The converted element as a MofObject</returns>
        public static IObject ConvertFromDotNetObject(
            IUriExtent receiver,
            object value,
            IElement? metaclass = null,
            string? requestedId = null)
            => new DotNetConverter((MofUriExtent) receiver).ConvertToMofObject(value, metaclass, requestedId);

        /// <summary>
        /// Converts the given .Net Object in value to a MofObject. The intermediate InMemory Extent is used.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="metaclass">Metaclass being used to create the element</param>
        /// <param name="requestedId">Id of the element that shall be put</param>
        /// <returns>The converted element as a MofObject</returns>
        public static IObject ConvertFromDotNetObject(
            object value,
            IElement? metaclass = null,
            string? requestedId = null)
            => ConvertFromDotNetObject(InMemoryProvider.TemporaryExtent, value, metaclass, requestedId);

        /// <summary>
        /// Converts the MOF element to a .Net element by using the explicit DotNet Type Lookup
        /// </summary>
        /// <param name="element">MOF element to be converted</param>
        /// <param name="lookup">Lookup table to find the .Net type of the MOF element</param>
        /// <returns></returns>
        public static object? ConvertToDotNetObject(IElement element, IDotNetTypeLookup lookup)
        {
            var uri = element.metaclass?.GetUri() ?? throw new InvalidOperationException("Uri is not set");
            var type = lookup.ToType(uri);
            if (type == null)
            {
                throw new InvalidOperationException(
                    $"Unknown metaclass {NamedElementMethods.GetName(element.metaclass)}");
            }

            return ConvertToDotNetObject(element, type);
        }

        /// <summary>
        /// Converts the given element to a real .Net Object by using the associated extents
        /// </summary>
        /// <param name="element">Element to be converted</param>
        /// <returns>The converted .Net Type</returns>
        public static object? ConvertToDotNetObject(IElement element)
        {
            var mofElement = (MofElement) element;
            var metaClassUri = mofElement.metaclass?.GetUri();
            if (metaClassUri == null)
                throw new InvalidOperationException("metaClassUri is null");

            var type = mofElement.ReferencedExtent.ResolveDotNetType(metaClassUri, ResolveType.Default);

            if (type == null)
            {
                throw new InvalidOperationException(
                    $"Unknown metaclass {NamedElementMethods.GetName(element.metaclass)}");
            }

            return ConvertToDotNetObject(element, type);
        }

        /// <summary>
        /// Converts the given MOF Object into a .Net Object
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned</typeparam>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object</returns>
        public static T ConvertToDotNetObject<T>(IObject value)
            => (T) ConvertToDotNetObject(value, typeof(T))!;

        /// <summary>
        /// Converts the given MOF Object into a .Net Object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="type">Type of the element to be converted</param>
        /// <returns>The converted object</returns>
        public static object? ConvertToDotNetObject(IObject value, Type type)
        {
            if (value == null) return null;
            var result = Activator.CreateInstance(type);
            foreach (var reflectedProperty in type.GetProperties(
                BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public))
            {
                if (value.isSet(reflectedProperty.Name))
                {
                    var propertyValue = value.get(reflectedProperty.Name);
                    if (propertyValue == null)
                    {
                        reflectedProperty.SetValue(result, null);
                    }
                    else if (reflectedProperty.PropertyType == typeof(string))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsString(propertyValue));
                    }
                    else if (reflectedProperty.PropertyType == typeof(int))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsInteger(propertyValue));
                    }
                    else if (reflectedProperty.PropertyType == typeof(double))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsDouble(propertyValue));
                    }
                    else if (reflectedProperty.PropertyType == typeof(bool))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsBoolean(propertyValue));
                    }
                    else if (reflectedProperty.PropertyType == typeof(IObject) ||
                             reflectedProperty.PropertyType == typeof(IElement))
                    {
                        reflectedProperty.SetValue(result, propertyValue as IObject);
                    }
                    else if (reflectedProperty.PropertyType.IsEnum)
                    {
                        var propertyValueType = propertyValue.GetType();
                        if (propertyValueType == reflectedProperty.PropertyType)
                        {
                            reflectedProperty.SetValue(result, propertyValue);
                        }
                        else if (propertyValue is string propertyValueAsString)
                        {
                            reflectedProperty.SetValue(result, Enum.Parse(reflectedProperty.PropertyType, propertyValueAsString));
                        }
                        else if (propertyValue is IElement propertyObject && value is MofObject mofObject)
                        {
                            // Get Enumeration Instance
                            var resolvedElement = mofObject.ReferencedExtent.Resolve(propertyObject);
                            if (resolvedElement == null)
                            {
                                reflectedProperty.SetValue(result, null);
                            }
                            else
                            {
                                var name = NamedElementMethods.GetName(resolvedElement);
                                reflectedProperty.SetValue(result, Enum.Parse(reflectedProperty.PropertyType, name));
                            }
                        }
                    }
                    else if (DotNetHelper.IsEnumeration(reflectedProperty.PropertyType))
                    {
                        var asCollection = reflectedProperty.GetValue(result) as IList;
                        if (asCollection == null)
                        {
                            continue;
                        }
                        
                        var listPropertyType = DotNetTypeGenerator.GetAnyElementType(reflectedProperty.PropertyType);
                        foreach (var valueInList in (IEnumerable) propertyValue)
                        {
                            if (valueInList is IObject asObject)
                            {
                                var listItem = ConvertToDotNetObject(asObject, listPropertyType);
                                asCollection.Add(listItem);
                            }
                        }
                        
                    }
                    else
                    {
                        Logger.Error($"Unknown type for Property: {reflectedProperty.Name}: {reflectedProperty.PropertyType}");
                    }
                }
            }

            return result;
        }
    }
}
