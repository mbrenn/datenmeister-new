using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the MOF interface for the uriextent
    /// </summary>
    public class UriExtent : Extent, IUriExtent
    {
        private readonly string _uri;
        private ExtentUrlNavigator<MofElement> _navigator;

        /// <inheritdoc />
        public UriExtent(IProvider provider, string uri) : base(provider)
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
            /*var asMofElement = (MofElement)element;
            if ((asMofElement.Extent as UriExtent)?.contextURI() != contextURI())
            {
                throw new InvalidOperationException("The given element is not contained in the extent.");
            }

            return contextURI() + "#" + asMofElement.ProviderObject.Id;*/
        }

        /// <inheritdoc />
        public IElement element(string uri)
        {
            return _navigator.element(uri);
            /*foreach (var innerElement in elements().GetAllDescendants()
                .OfType<IElement>())
            {
                if (this.uri(innerElement) == uri)
                {
                    return innerElement;
                }
            }

            return null;*/
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"UriExent: {contextURI()}";
        }
    }
}