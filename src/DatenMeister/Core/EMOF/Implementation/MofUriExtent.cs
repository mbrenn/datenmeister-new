using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the MOF interface for the uriextent
    /// </summary>
    public class MofUriExtent : MofExtent, IUriExtent
    {
        private readonly string _uri;
        private ExtentUrlNavigator<MofElement> _navigator;

        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri, IDotNetTypeLookup typeLookup = null) :
            base(provider, typeLookup)
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

        /// <summary>
        /// Gets the id of the element as defined in the uri. 
        /// The format of the uri is expected to be like protocol://path#id.
        /// The text after the first '#' is returned
        /// </summary>
        /// <param name="uri">Uri to be looked for</param>
        /// <returns>Found uri</returns>
        public static string GetIdOfUri(string uri)
        {
            var pos = uri.IndexOf('#');
            if (pos == -1)
            {
                return uri;
            }

            return uri.Substring(pos + 1);
        }
    }
}