﻿using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the factory according to the MOF specification
    /// </summary>
    public class MofFactory : IFactory
    {
        private readonly IProvider _provider;

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(MofExtent extent)
        {
            if (extent == null) throw new ArgumentNullException(nameof(extent));
            _provider = extent.Provider;
            if (_provider == null) throw new ArgumentNullException(nameof(_provider));
        }

        /// <summary>
        /// Initializes a new instance of the provider
        /// </summary>
        /// <param name="provider">Provider object to be set</param>
        public MofFactory(IProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// Initializes a new instance of the MofFactory by retrieving an object
        /// </summary>
        /// <param name="value">Value to be set</param>
        public MofFactory(MofObject value)
        {
            var extent = value.Extent;
            if (extent != null)
            {
                // First, try the correct way via the extent.
                _provider = extent.Provider;
            }
            else
            {
                // If not available, do it via the providerobject
                _provider = value.ProviderObject.Provider;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(IExtent extent) : this ((MofExtent) extent)
        {
        }

        /// <inheritdoc />
        public IElement package
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public IElement create(IElement metaClass)
        {
            var elementAsMetaClass = (MofElement) metaClass;
            var uriMetaClass = (elementAsMetaClass?.Extent as MofUriExtent)?.uri(metaClass);
            return new MofElement(
                _provider.CreateElement(uriMetaClass),
                null);
        }

        /// <inheritdoc />
        public IObject createFromString(IElement dataType, string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string convertToString(IElement dataType, IObject value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an element by the given value and the metaclass, that is set
        /// </summary>
        /// <param name="value">Value to be set</param>
        /// <param name="id">Id, that shall be set</param>
        /// <param name="metaClass">Metaclass that shall be assigned to the factory</param>
        /// <returns>Created element</returns>
        public IElement create(object value, string id = "", IElement metaClass = null)
        {
            throw new InvalidOperationException();
        }
    }
}