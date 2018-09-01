using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// A supporting class, that converts a .Net Element to a MOF object.
    /// </summary>
    public class DotNetSetter
    {
        /// <summary>
        /// Stores the MOF Factory
        /// </summary>
        private readonly MofFactory _factory;

        /// <summary>
        /// Stores a list of already visited elements, so a recursion is avoided
        /// </summary>
        private readonly HashSet<object> _visitedElements = new HashSet<object>();

        /// <summary>
        /// Stores the resolver to find metaclasses by .Net Types
        /// </summary>
        private readonly MofUriExtent _extent;

        /// <summary>
        /// Initializes a new instance of the DotNetSetter class
        /// </summary>
        /// <param name="extent">Extent being used as reference to find typeLookup and Resolver</param>
        private DotNetSetter(MofUriExtent extent)
        {
            _factory = new MofFactory(extent);
            _extent = extent;
        }

        /// <summary>
        /// Sets the given object into the MofObject. 
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        public static object Convert(MofUriExtent receiver, object value, string requestedId = null)
        {
            return new DotNetSetter(receiver).Convert(value, requestedId);
        }

        /// <summary>
        /// Sets the given object into the MofObject. 
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        public static object Convert(IUriExtent receiver, object value, string requestedId = null)
        {
            return Convert((MofUriExtent)receiver, value, requestedId);
        }

        /// <summary>
        /// Converts the given object and stores it into the receiver's method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requestedId">Defines the id that shall be set upon the newly created object</param>
        private object Convert(object value, string requestedId = null)
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

            // After having the uri, create the required element
            var createdElement = _factory.create(
                metaClassUri == null ? null : _extent.Resolve(metaClassUri, ResolveType.OnlyMetaClasses));
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
                    var enumeration = (IEnumerable) innerValue;
                    foreach (var innerElementValue in enumeration)
                    {
                        list.Add(Convert(innerElementValue));
                    }

                    createdElement.set(reflectedProperty.Name, list);
                }
                else
                {
                    createdElement.set(reflectedProperty.Name, Convert(innerValue));
                }
            }

            return createdElement;
        }
    }
}
