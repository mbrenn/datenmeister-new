using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines a Mof Element according to MOF specification
    /// </summary>
    public class MofElement : MofObject, IElement, IElementSetMetaClass, IHasId
    {
        private IElement _container;

        /// <summary>
        /// Initialized a new instance of the MofElement class which is an abstraction of the provided database. 
        /// </summary>
        /// <param name="providedObject">Provided object by database</param>
        /// <param name="extent">Extent to which the object is allocated to</param>
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

        public void SetContainer(IElement container)
        {
            _container = container;
        }

        /// <summary>
        /// Sets the meta class for the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to be set</param>
        public void SetMetaClass(IElement metaClass)
        {
            var mofElement = (MofElement) metaClass;
            ProviderObject.MetaclassUri = (mofElement.Extent as UriExtent).uri(metaClass);
        }

        /// <inheritdoc />
        public string Id => ProviderObject.Id;
    }
}