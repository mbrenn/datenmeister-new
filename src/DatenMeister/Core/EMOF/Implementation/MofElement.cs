using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines a Mof Element according to MOF specification
    /// </summary>
    public class MofElement : MofObject, IElement, IElementSetMetaClass, IHasId, ICanSetId
    {
        /// <inheritdoc cref="ICanSetId.Id" />
        public string Id
        {
            get => ProviderObject.Id;
            set => ProviderObject.Id = value;
        }

        /// <summary>
        /// Initialized a new instance of the MofElement class which is an abstraction of the provided database. 
        /// </summary>
        /// <param name="providedObject">Provided object by database</param>
        /// <param name="extent">Extent to which the object is allocated to</param>
        /// <param name="referenceElement"></param>
        public MofElement(
            IProviderObject providedObject, 
            MofExtent extent,
            IElement referenceElement = null)
            : base(providedObject, extent)
        {
            if (ReferencedExtent == null)
            {
                ReferencedExtent = ((MofElement) referenceElement)?.ReferencedExtent;
            }
        }

        /// <summary>
        /// Sets the referenced extent being used to resolve uris
        /// </summary>
        /// <param name="extent">Extent to be set as start for references</param>
        /// <returns>The element itself for chaining</returns>
        public MofElement SetReferencedExtent(MofExtent extent)
        {
            ReferencedExtent = extent;
            return this;
        }

        /// <summary>
        /// Sets the referenced extent being used to resolve uris
        /// </summary>
        /// <param name="extent">Extent to be set as start for references</param>
        /// <returns>The element itself for chaining</returns>
        public MofElement SetReferencedExtent(IUriExtent extent)
        {
            return SetReferencedExtent((MofExtent) extent);
        }

        /// <inheritdoc />
        public IElement metaclass => getMetaClass();

        /// <inheritdoc />
        public IElement getMetaClass()
        {
            var uri = ProviderObject.MetaclassUri;
            if (string.IsNullOrEmpty(uri))
            {
                // No metaclass Uri is given.
                return null;
            }

            var result = (ReferencedExtent as IUriResolver)?.Resolve(uri, ResolveType.OnlyMetaClasses);
            if (result == null)
            {
                result = new MofObjectShadow(uri);
            }

            return result;
        }

        /// <inheritdoc />
        public IElement container()
        {
            var containerElement = ProviderObject.GetContainer();
            return containerElement != null
                ? new MofElement(containerElement, Extent ?? ReferencedExtent)
                    {Extent = Extent}
                : null;
        }

        /// <summary>
        /// Sets the container for the element
        /// </summary>
        public IObject Container
        {
            set => ProviderObject.SetContainer(((MofObject)value).ProviderObject);
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

            Extent?.ChangeEventManager?.SendChangeEvent(this);
        }

        /// <summary>
        /// Sets the metaclas by directly setting the uri
        /// </summary>
        /// <param name="metaClassUri">Uri to be set</param>
        public void SetMetaClass(string metaClassUri)
        {
            ProviderObject.MetaclassUri = metaClassUri;
            Extent?.ChangeEventManager?.SendChangeEvent(this);
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

    /// <summary>
    /// Defines a mofobject which is created on the fly to reference
    /// to a specific object which could not be looked up. This supports usages
    /// of DatenMeister with typed instances but without having the full MOF database in memory
    /// </summary>
    public class MofObjectShadow : IElement, IKnowsUri
    {
        /// <summary>
        /// Gets the uri, which describes the given element
        /// </summary>
        public string Uri { get; }

        public MofObjectShadow(string uri)
        {
            Uri = uri;
        }

        public bool @equals(object other)
        {
            return MofObject.AreEqual(this, other as IObject);
        }

        public object get(string property)
        {
            return null;
        }

        public void set(string property, object value)
        {
            throw new NotImplementedException("This is just a shadow object which cannot store data");
        }

        public bool isSet(string property)
        {
            return false;
        }

        public void unset(string property)
        {
            throw new NotImplementedException("This is just a shadow object which cannot store data");
        }

        public IElement metaclass => getMetaClass();

        public IElement getMetaClass()
        {
            return null;
        }

        public IElement container()
        {
            return null;
        }
    }

    /// <summary>
    /// This interface can be implemented by all elements, which know there uri and 
    /// where the extension method GetUri can directly access the given field without the
    /// need to lookup and query the extent. This interface is mainly used by the shadow objects
    /// which are somehow out of context
    /// </summary>
    public interface IKnowsUri
    {
        /// <summary>
        /// Gets the uri
        /// </summary>
        string Uri { get; }
    }
}