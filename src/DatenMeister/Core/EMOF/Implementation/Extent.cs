using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class Extent : IExtent
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
        public Extent(IProvider provider)
        {
            Provider = provider;
            Resolver = new ExtentResolver(this);
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            var otherAsExtent = other as Extent;
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
    }
}