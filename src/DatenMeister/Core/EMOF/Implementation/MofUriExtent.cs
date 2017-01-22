﻿using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the MOF interface for the uriextent
    /// </summary>
    public class MofUriExtent : Extent, IUriExtent
    {
        private readonly string _uri;
        private ExtentUrlNavigator<MofElement> _navigator;

        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri) : base(provider)
        {
            _uri = uri;
            _navigator = new ExtentUrlNavigator<MofElement>(this);
        }

        /// <inheritdoc />
        public string contextURI()
        {
            return _uri;
        }

        /// <inheritdoc />
        public string uri(IElement element)
        {
            return _navigator.uri(element);
        }

        /// <inheritdoc />
        public IElement element(string uri)
        {
            return _navigator.element(uri);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"UriExent: {contextURI()}";
        }
    }
}