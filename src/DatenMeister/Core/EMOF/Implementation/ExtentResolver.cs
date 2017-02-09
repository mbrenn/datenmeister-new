using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Supports the resolvement of objects to 
    /// </summary>
    public class ExtentResolver : IUriResolver
    {
        /// <summary>
        /// Stores the extent to which the resolver is allocated
        /// </summary>
        private readonly MofExtent _extent;

        /// <summary>
        /// Stores a list of other extents that shall also be considered as meta extents
        /// </summary>
        private readonly List<IUriExtent> _metaExtents = new List<IUriExtent>();

        /// <summary>
        /// Initializes a new instance of the UriResolver class.
        /// </summary>
        /// <param name="extent">Extent being used as a relative source for information</param>
        public ExtentResolver(MofExtent extent)
        {
            _extent = extent;
        }

        /// <summary>
        /// Adds an extent as a meta extent, so it will also be used to retrieve the element
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        public void AddMetaExtent(IUriExtent extent)
        {
            lock (_metaExtents)
            {
                if (_metaExtents.Any(x => x.contextURI() == extent.contextURI()))
                {
                    // Already in 
                    return;
                }

                _metaExtents.Add(extent);
            }
        }

        /// <inheritdoc />
        public IElement Resolve(string uri)
        {
            var asUriExtent = _extent as MofUriExtent;
            var result = asUriExtent?.element(uri);
            if (result != null)
            {
                return result;
            }

            lock (_metaExtents)
            {
                foreach (var metaExtent in _metaExtents)
                {
                    var element = metaExtent.element(uri);
                    if (element != null)
                    {
                        return element;
                    }
                }
            }

            throw new NotImplementedException($"The given element with uri {uri} is not of type UriExtent");
        }
    }
}