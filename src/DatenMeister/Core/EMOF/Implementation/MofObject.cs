using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the abstraction of the Mof Object.
    /// </summary>
    public class MofObject : IObject, IHasExtent, IObjectAllProperties
    {
        /// <summary>
        /// Stores the extent
        /// </summary>
        private MofExtent _extent;

        /// <summary> 
        /// Gets the extent of the mof object
        /// </summary>
        public MofExtent Extent
        {
            get { return _extent; }
            set { _extent = CreatedByExtent = value; }
        }

        /// <summary>
        /// Stores the extent that is used to create the element. 
        /// This extent is used for type lookup and other referencing things. 
        /// </summary>
        internal MofExtent CreatedByExtent { get; private set; }

        /// <summary>
        /// Gets the extent of the mof object
        /// </summary>
        IExtent IHasExtent.Extent => Extent;

        /// <summary>
        /// Gets the provided object
        /// </summary>
        public IProviderObject ProviderObject { get; }

        /// <summary>
        /// Initializes a new instance of the MofObject class. 
        /// </summary>
        /// <param name="providedObject">The database abstraction of the object</param>
        /// <param name="extent">The extent being used to access the item</param>
        public MofObject(IProviderObject providedObject, MofExtent extent)
        {
            if (providedObject == null) throw new ArgumentNullException(nameof(providedObject));
            if (providedObject.Provider == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("providedObject.Provider");
            }

            ProviderObject = providedObject;
            CreatedByExtent = Extent = extent;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return @equals(obj);
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            var otherAsObject = other as MofObject;
            if (otherAsObject != null)
            {
                return otherAsObject.ProviderObject.Id == ProviderObject.Id;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ProviderObject?.GetHashCode() ?? base.GetHashCode();
        }

        /// <inheritdoc />
        public object get(string property)
        {
            return get(property, false);
        }

        public object get(string property, bool noReferences)
        {
            var result = ProviderObject.GetProperty(property);
            return ConvertToMofObject(this, property, result, noReferences);
        }

        /// <summary>
        /// Converts the object to a mof object that can be added to the MofObject
        /// </summary>
        /// <param name="container">Container to be added</param>
        /// <param name="property">Property to be set</param>
        /// <param name="value">Value to be converted</param>
        /// <param name="noReferences">True, if references shall be resolved</param>
        /// <returns>The converted object</returns>
        internal static object ConvertToMofObject(
            MofObject container, 
            string property, 
            object value,
            bool noReferences = false)
        {
            if (value == null)
            {
                return null;
            }

            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return value;
            }

            var resultAsProviderObject = value as IProviderObject;
            if (resultAsProviderObject != null)
            {
                return new MofElement(resultAsProviderObject, container.Extent, container as MofElement);
            }

            if (value is IEnumerable<object>)
            {
                return new MofReflectiveSequence(container, property);
            }

            var valueAsUriReference = value as UriReference;
            if (valueAsUriReference != null)
            {
                if (noReferences)
                {
                    return valueAsUriReference;
                }
                
                return container.Extent.Resolver.Resolve(valueAsUriReference.Uri);
            }

            throw new NotImplementedException($"Type of {value.GetType()} currently not supported.");
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            if (DotNetHelper.IsOfEnumeration(value))
            {
                var valueAsEnumeration = (IEnumerable<object>) value;
                ProviderObject.EmptyListForProperty(property);
                foreach (var child in valueAsEnumeration)
                {
                    ProviderObject.AddToProperty(property, MofExtent.ConvertForSetting(this, child));
                }
            }
            else
            {
                ProviderObject.SetProperty(property, MofExtent.ConvertForSetting(this, value));
            }
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            return ProviderObject.IsPropertySet(property);
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            ProviderObject.DeleteProperty(property);
        }

        /// <inheritdoc />
        public IEnumerable<string> getPropertiesBeingSet()
        {
            return ProviderObject.GetProperties();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return UmlNameResolution.GetName(this);
        }

        public virtual IObject CreatedBy(MofExtent extent)
        {
            CreatedByExtent = extent;
            return this;
        }
    }
}