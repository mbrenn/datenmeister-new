using System;
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
        public MofFactory(Extent extent)
        {
            if (extent == null) throw new ArgumentNullException(nameof(extent));
            _provider = extent.Provider;
            if (_provider == null) throw new ArgumentNullException(nameof(_provider));
        }

        /// <summary>
        /// Initializes a new instance of the provider
        /// </summary>
        /// <param name="providerObject">Provider object to be set</param>
        public MofFactory(IProvider providerObject)
        {
            if (providerObject == null) throw new ArgumentNullException(nameof(providerObject));
            _provider = providerObject;
        }

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(IExtent extent) : this ((Extent) extent)
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
            var uriMetaClass = (elementAsMetaClass?.Extent as UriExtent)?.uri(metaClass);
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
    }
}