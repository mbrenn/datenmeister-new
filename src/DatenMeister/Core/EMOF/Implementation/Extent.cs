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

        public Extent(IProvider provider)
        {
            Provider = provider;
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public object get(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool useContainment()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IReflectiveSequence elements()
        {
            throw new System.NotImplementedException();
        }
    }
}