﻿using System;
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
        public Extent _extent;

        /// <summary>
        /// Initializes a new instance of the UriResolver class.
        /// </summary>
        /// <param name="extent">Extent being used as a relative source for information</param>
        public ExtentResolver(Extent extent)
        {
            _extent = extent;
        }

        /// <inheritdoc />
        public IElement Resolve(string uri)
        {
            var asUriExtent = _extent as UriExtent;
            if (asUriExtent != null)
            {
                return asUriExtent.element(uri);
            }

            throw new NotImplementedException("The given extent is not of type UriExtent");
        }
    }
}