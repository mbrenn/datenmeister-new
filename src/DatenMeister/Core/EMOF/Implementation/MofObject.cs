using System;
using System.Collections.Generic;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the abstraction of the Mof Object.
    /// </summary>
    public class MofObject : IObject, IHasExtent
    {
        /// <summary>
        /// Gets the extent of the mof object
        /// </summary>
        public Extent Extent { get; set; }

        /// <summary>
        /// Gets the extent of the mof object
        /// </summary>
        IExtent IHasExtent.Extent => Extent;

        /// <summary>
        /// Gets the provided object
        /// </summary>
        internal IProviderObject ProviderObject { get; }

        /// <summary>
        /// Initializes a new instance of the MofObject class. 
        /// </summary>
        /// <param name="providedObject">The database abstraction of the object</param>
        /// <param name="extent">The extent being used to access the item</param>
        public MofObject(IProviderObject providedObject, Extent extent)
        {
            ProviderObject = providedObject;
            Extent = extent;
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
        public object get(string property)
        {
            var result = ProviderObject.GetProperty(property);
            return ConvertToMofObject(property, result);
        }

        private object ConvertToMofObject(string property, object result)
        {
            if (DotNetHelper.IsOfMofObject(result))
            {
                return result;
            }

            var resultAsProviderObject = result as IProviderObject;
            if (resultAsProviderObject != null)
            {
                return new MofObject(resultAsProviderObject, Extent);
            }

            if (result is IEnumerable<object>)
            {
                return new MofReflectiveSequence(this, property, (IEnumerable<object>) result);
            }

            throw new NotImplementedException($"Type of {result.GetType()} currently not supported.");
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
                    ProviderObject.AddToProperty(property, ConvertForSetting(child));
                }
            }
            else
            {
                ProviderObject.SetProperty(property, ConvertForSetting(value));
            }
        }

        /// <summary>
        /// Converts the object to be set by the data provider
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object or an exception if the object cannot be converted</returns>
        public object ConvertForSetting(object value)
        {
            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return value;
            }

            if (DotNetHelper.IsOfMofObject(value))
            {
                var asMofObject = (MofObject) value;

                if (asMofObject.Extent == null)
                {
                    var result = ObjectCopier.Copy(new MofFactory(Extent), asMofObject);
                    return result;
                }
            }

            throw new NotImplementedException($"Type of {value.GetType()} currently not supported.");
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
    }
}