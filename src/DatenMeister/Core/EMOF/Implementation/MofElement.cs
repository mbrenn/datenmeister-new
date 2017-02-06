using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines a Mof Element according to MOF specification
    /// </summary>
    public class MofElement : MofObject, IElement, IElementSetMetaClass, IHasId, ICanSetId
    {
        /// <inheritdoc />
        public string Id
        {
            get { return ProviderObject.Id; }
            set { ProviderObject.Id = value; }
        }

        /// <summary>
        /// Initialiezs a new instance of the MofElement. This method is just used for migration
        /// </summary>
        [Obsolete]
        public MofElement() : base(new InMemoryObject(InMemoryProvider.TemporaryProvider), InMemoryProvider.TemporaryExtent)
        {

        }

        private IElement _container;

        /// <summary>
        /// Initialized a new instance of the MofElement class which is an abstraction of the provided database. 
        /// </summary>
        /// <param name="providedObject">Provided object by database</param>
        /// <param name="extent">Extent to which the object is allocated to</param>
        /// <param name="container"></param>
        public MofElement(
            IProviderObject providedObject, 
            MofExtent extent,
            IElement container = null)
            : base(providedObject, extent)
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

            return CreatedByExtent?.Resolver.Resolve(uri);
        }

        /// <inheritdoc />
        public IElement container()
        {
            return _container;
        }

        public void SetContainer(IElement container)
        {
            _container = container;
            Extent = ((MofElement) container).Extent;
        }

        /// <summary>
        /// Sets the meta class for the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to be set</param>
        public void SetMetaClass(IElement metaClass)
        {   
            var mofElement = (MofElement) metaClass;
            if (mofElement.Extent == null)
            {
                throw new InvalidOperationException("The given metaclass is not connected to an element");
            }

            ProviderObject.MetaclassUri = ((MofUriExtent) mofElement.Extent).uri(metaClass);
        }

        /// <summary>
        /// Sets the extent by which the element was created
        /// </summary>
        /// <param name="extent">Extent being used to define the the creator</param>
        /// <returns>this element. </returns>
        public new IElement CreatedBy(MofExtent extent)
        {
            base.CreatedBy(extent);
            return this;
        }
    }
}