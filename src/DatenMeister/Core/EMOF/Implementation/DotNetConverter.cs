using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// A supporting class, that converts a .Net Element to a MOF object or a MOF object to a .Net Object
    /// </summary>
    public class DotNetConverter
    {
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
        private object ConvertToMofIfNotPrimitive(object value, string requestedId = null)
        {
            if (DotNetHelper.IsOfPrimitiveType(value) || DotNetHelper.IsOfEnum(value))
            {
                return value;
            }

            if (value is IObject)
            {
                return value;
            }

            if (value == null)
            {
                return null;
            }

            // Check, if the element already existed 
            if (_visitedElements.Contains(value))
            {
                return null;
            }

            _visitedElements.Add(value);

            // Gets the uri of the lookup up type
            var metaClassUri = _extent?.GetMetaClassUri(value.GetType());
            var metaClass = metaClassUri == null ? null : _extent.Resolve(metaClassUri, ResolveType.OnlyMetaClasses);

            return ConvertToMofObject(value, metaClass, requestedId);
        }

        /// <summary>
        /// Converts the given .Net Object in value to a MofObject
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="metaClass">Metaclass being used to create the element</param>
        /// <param name="requestedId">Id of the element that shall be put</param>
        /// <returns>The converted element as a MofObject</returns>
        private IObject ConvertToMofObject(object value, IElement metaClass = null, string requestedId = null)
        {
            // After having the uri, create the required element
            var createdElement = _factory.create(metaClass);
            if (!string.IsNullOrEmpty(requestedId) && createdElement is ICanSetId canSetId)
            {
                canSetId.Id = requestedId;
            }

            var type = value.GetType();
            foreach (var reflectedProperty in type.GetProperties(
                BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public))
            {
                var innerValue = reflectedProperty.GetValue(value);
                if (DotNetHelper.IsOfEnumeration(innerValue))
                {
                    var list = new List<object>();
                    var enumeration = (IEnumerable)innerValue;
                    foreach (var innerElementValue in enumeration)
                    {
                        list.Add(ConvertToMofIfNotPrimitive(innerElementValue));
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
        public static object ConvertToMofObject(MofUriExtent receiver, object value, string requestedId = null)
        {
            return new DotNetConverter(receiver).ConvertToMofIfNotPrimitive(value, requestedId);
        }

        /// <summary>
        /// Sets the given object into the MofObject. 
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        public static object ConvertToMofObject(IUriExtent receiver, object value, string requestedId = null)
        {
            return ConvertToMofObject((MofUriExtent)receiver, value, requestedId);
        }

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
            IElement metaclass = null,
            string requestedId = null)
        {
            return new DotNetConverter((MofUriExtent)receiver).ConvertToMofObject(value, metaclass, requestedId);
        }

        /// <summary>
        /// Converts the given .Net Object in value to a MofObject. The intermediate InMemory Extent is used. 
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="metaClass">Metaclass being used to create the element</param>
        /// <param name="requestedId">Id of the element that shall be put</param>
        /// <returns>The converted element as a MofObject</returns>
        public static IObject ConvertFromDotNetObject(
            object value,
            IElement metaclass = null,
            string requestedId = null)
        {
            return ConvertFromDotNetObject(InMemoryProvider.TemporaryExtent, value, metaclass, requestedId);
        }

        public static object ConvertToDotNetObject(IElement element, IDotNetTypeLookup lookup)
        {
            throw new InvalidOperationException();
            //lookup.ToType(NamedElementMethods.GetName(element.metaclass()));
        }

        /// <summary>
        /// Converts the given MOF Object into a .Net Object
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned</typeparam>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object</returns>
        public static T ConvertToDotNetObject<T>(IObject value)
        {
            return (T) ConvertToDotNetObject(value, typeof(T));
        }

        /// <summary>
        /// Converts the given MOF Object into a .Net Object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="type">Type of the element to be converted</param>
        /// <returns>The converted object</returns>
        public static object ConvertToDotNetObject(IObject value, Type type)
        {
            var result = Activator.CreateInstance(type);
            foreach (var reflectedProperty in type.GetProperties(
                BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public))
            {
                if (value.isSet(reflectedProperty.Name))
                {
                    var propertyValue = value.get(reflectedProperty.Name);
                    if (reflectedProperty.PropertyType == typeof(string))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsString(propertyValue));
                    }

                    if (reflectedProperty.PropertyType == typeof(int))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsInteger(propertyValue));
                    }

                    if (reflectedProperty.PropertyType == typeof(double))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsDouble(propertyValue));
                    }

                    if (reflectedProperty.PropertyType == typeof(bool))
                    {
                        reflectedProperty.SetValue(result, DotNetHelper.AsBoolean(propertyValue));
                    }
                }
            }

            return result;
        }
    }
}
