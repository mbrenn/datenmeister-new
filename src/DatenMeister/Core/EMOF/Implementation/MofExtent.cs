using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class MofExtent : IExtent
    {
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
        public MofExtent(IProvider provider)
        {
            Provider = provider;
            Resolver = new ExtentResolver(this);
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
        public static object ConvertForSetting(MofExtent extent, object value)
        {
            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return value;
            }

            if (DotNetHelper.IsOfMofObject(value))
            {
                var asMofObject = (MofObject)value;

                if (asMofObject.Extent == null)
                {
                    var result = (MofElement)ObjectCopier.Copy(new MofFactory(extent), asMofObject);
                    /*if (this is IElement)
                    {
                        result.SetContainer((IElement) this);
                    }*/

                    return result.ProviderObject;
                }
                else
                {
                    // It is a reference
                    var reference = new UriReference
                    {
                        Uri = ((MofUriExtent)asMofObject.Extent).uri(asMofObject as IElement)
                    };

                    return reference;
                }
            }

            // Then, we have a simple dotnet type, that we try to convert. Let's hope, that it works
            return ConvertForSetting(extent, DotNetSetter.Convert(extent, value));
        }
        /// <summary>
        /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject. 
        /// An arbitrary object shall be stored into the database
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object or an exception if the object cannot be converted</returns>
        public static object ConvertForSetting(MofObject mofObject, object value)
        {
            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return value;
            }

            if (DotNetHelper.IsOfMofObject(value))
            {
                var asMofObject = (MofObject)value;

                if (asMofObject.Extent == null)
                {
                    var result = (MofElement)ObjectCopier.Copy(new MofFactory(mofObject), asMofObject);
                    /*if (this is IElement)
                    {
                        result.SetContainer((IElement) this);
                    }*/

                    return result.ProviderObject;
                }
                else
                {
                    // It is a reference
                    var reference = new UriReference
                    {
                        Uri = ((MofUriExtent)asMofObject.Extent).uri(asMofObject as IElement)
                    };

                    return reference;
                }
            }

            // Then, we have a simple dotnet type, that we try to convert. Let's hope, that it works
            return ConvertForSetting(mofObject, DotNetSetter.Convert(mofObject, value));
        }
    }
}