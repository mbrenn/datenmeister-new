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
        /// Initializes a new instance of the UriResolver class.
        /// </summary>
        /// <param name="extent">Extent being used as a relative source for information</param>
        public ExtentResolver(MofExtent extent)
        {
            _extent = extent;
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

            foreach (var metaExtent in _extent.MetaExtents)
            {
                var element = metaExtent.element(uri);
                if (element != null)
                {
                    return element;
                }
            }


            return null;
        }

        public IElement ResolveById(string id)
        {
            var asUriExtent = _extent as MofUriExtent;
            var uri = asUriExtent?.contextURI() + "#" + id;
            return (_extent as IUriExtent)?.element(uri);
        }
    }
}