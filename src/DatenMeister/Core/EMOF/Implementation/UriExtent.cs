using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class UriExtent  : Extent, IUriExtent
    {
        /// <inheritdoc />
        public UriExtent(IProvider provider) : base(provider)
        {
        }

        /// <inheritdoc />
        public string contextURI()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public string uri(IElement element)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IElement element(string uri)
        {
            throw new System.NotImplementedException();
        }
    }
}