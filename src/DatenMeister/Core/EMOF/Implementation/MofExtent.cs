using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class MofExtent : IExtent
    {
        /// <summary>
        /// This type lookup can be used to convert the instances of the .Net types to real MOF meta classes. 
        /// It is only used, if the data 
        /// </summary>
        public IDotNetTypeLookup TypeLookup { get; }

        /// <summary>
        /// Gets or sets the provider for the given extent
        /// </summary>
        public IProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the resolver being used to resolve the uri
        /// </summary>
        public IUriResolver Resolver { get; set; }

        /// <summary>
        /// Initializes a new instance of the Extent 
        /// </summary>
        /// <param name="provider">Provider being used for the extent</param>
        /// <param name="typeLookup">Type lookup being used</param>
        public MofExtent(IProvider provider, IDotNetTypeLookup typeLookup = null)
        {
            Provider = provider;
            Resolver = new ExtentResolver(this);
            TypeLookup = typeLookup;
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            var otherAsExtent = other as MofExtent;
            if (otherAsExtent != null)
            {
                return Equals(otherAsExtent);
            }

            return false;
        }

        /// <inheritdoc />
        public object get(string property)
        {
            return Provider.Get(null).GetProperty(property);
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            Provider.Get(null).SetProperty(property, value);
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            return Provider.Get(null).IsPropertySet(property);
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            Provider.Get(null).DeleteProperty(property);
        }

        /// <inheritdoc />
        public bool useContainment()
        {
            return false;
        }

        /// <inheritdoc />
        public IReflectiveSequence elements()
        {
            return new ExtentReflectiveSequence(this);
        }

        /// <summary>
        /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject. 
        /// An arbitrary object shall be stored into the database
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object or an exception if the object cannot be converted</returns>
        public object ConvertForSetting(object value)
        {
            return ConvertForSetting(value, this, null);
        }

        /// <summary>
        /// Converts the given value to an element that can be used be for the provider object
        /// </summary>
        /// <param name="value">Value to be set</param>
        /// <param name="extent">Extent being used to create the factory or TypeLookup</param>
        /// <param name="container">Container which will host the newly created object</param>
        /// <returns>The converted object being ready for Provider</returns>
        public static object ConvertForSetting(object value, MofExtent extent, MofObject container)
        {
            if (value == null)
            {
                return null;
            }

            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return value;
            }

            if (DotNetHelper.IsOfMofObject(value))
            {
                var asMofObject = (MofObject) value;

                if (asMofObject.Extent == null)
                {
                    var result = (MofElement) ObjectCopier.Copy(new MofFactory(extent), asMofObject);
                    var containerAsElement = container as IElement;
                    if (containerAsElement != null)
                    {
                        result.SetContainer(containerAsElement);
                    }

                    return result.ProviderObject;
                }
                else
                {
                    // It is a reference
                    var reference = new UriReference
                    {
                        Uri = ((MofUriExtent) asMofObject.Extent).uri(asMofObject as IElement)
                    };

                    return reference;
                }
            }

            if (DotNetHelper.IsOfEnumeration(value))
            {
                return ((IEnumerable) value)
                    .Cast<object>()
                    .Select(innerValue => ConvertForSetting(innerValue, extent, container)).ToList();
            }

            // Then, we have a simple dotnet type, that we try to convert. Let's hope, that it works
            if (extent == null)
            {

                throw new InvalidOperationException(
                    "This element was not created by a factory. So a setting by .Net Object is not possible");
            }

            return ConvertForSetting(DotNetSetter.Convert(extent, value), extent, container);
        }

        /// <summary>
        /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject. 
        /// An arbitrary object shall be stored into the database
        /// </summary>
        /// <param name="mofObject">The Mofobject for which the element will be created</param>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object or an exception if the object cannot be converted</returns>
        public static object ConvertForSetting(MofObject mofObject, object value)
        {
            return ConvertForSetting(value, mofObject.CreatedByExtent, mofObject);

        }
    }
}