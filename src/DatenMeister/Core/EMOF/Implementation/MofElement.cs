using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class MofElement : MofObject, IElement
    {
        private readonly IElement _container;

        /// <summary>
        /// Initialized a new instance of the MofElement class which is an abstraction of the provided database. 
        /// </summary>
        /// <param name="providedObject">Provided object by database</param>
        /// <param name="extent"></param>
        /// <param name="container"></param>
        public MofElement(IProviderObject providedObject, Extent extent, IElement container = null) : base (providedObject, extent)
        {
            _container = container;
        }

        /// <inheritdoc />
        public IElement metaclass => getMetaClass();

        /// <inheritdoc />
        public IElement getMetaClass()
        {
            var uri = ProviderObject.MetaclassUri;
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            return Extent.Resolver.Resolve(uri);
        }

        /// <inheritdoc />
        public IElement container()
        {
            return _container;
        }
    }
}