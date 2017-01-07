using System;
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
            return ProviderObject.GetProperty(property);
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                ProviderObject.SetProperty(property, value);
            }
            else if (DotNetHelper.IsOfMofObject(value))
            {
                var asMofObject = (MofObject) value ;

                if (asMofObject.Extent == null)
                {
                    var result = ObjectCopier.Copy(new MofFactory(Extent), asMofObject);
                    ProviderObject.SetProperty(property, result);
                }
                else
                {
                    throw new NotImplementedException($"Type of {value.GetType()} currently not supported.");
                }
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
    }
}