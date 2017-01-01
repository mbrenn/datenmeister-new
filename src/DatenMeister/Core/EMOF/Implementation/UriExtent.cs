﻿using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class UriExtent  : Extent, IUriExtent
    {
        private readonly string _uri;

        /// <inheritdoc />
        public UriExtent(IProvider provider, string uri) : base(provider)
        {
            _uri = uri;
        }

        /// <inheritdoc />
        public string contextURI()
        {
            return _uri;
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