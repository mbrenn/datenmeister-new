using System;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the factory according to the MOF specification
    /// </summary>
    public class MofFactory : IFactory
    {
        /// <summary>
        /// Stores the provider, which is used to create the elements
        /// </summary>
        private readonly IProvider _provider;

        /// <summary>
        /// Gets the Mof Extent being connected to the factory
        /// </summary>
        public MofExtent Extent { get; }

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(MofExtent extent)
        {
            Extent = extent ?? throw new ArgumentNullException(nameof(extent));

            _provider = extent.Provider;
            if (_provider == null) throw new ArgumentNullException(nameof(_provider));
        }

        /// <summary>
        /// Initializes a new instance of the MofFactory
        /// </summary>
        /// <param name="collection">Colleciton to be used</param>
        public MofFactory(IReflectiveCollection collection) : this(((IHasExtent) collection).Extent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MofFactory by retrieving an object
        /// </summary>
        /// <param name="value">Value to be set</param>
        public MofFactory(IObject value)
        {
            var asMofObject = value as MofObject ?? throw new ArgumentException("value is null or not of Type MofObject");
            var extent = asMofObject.Extent;
            if (extent != null)
            {
                // First, try the correct way via the extent.
                Extent = extent;
                _provider = extent.Provider;
            }
            else if (asMofObject.ReferencedExtent != null)
            {
                Extent = asMofObject.ReferencedExtent;
                _provider = Extent.Provider;
            }
            else
            {
                // If not available, do it via the providerobject
                _provider = asMofObject.ProviderObject.Provider;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(IExtent extent) : this((MofExtent) extent)
        {
        }

        /// <inheritdoc />
        public IElement package => throw new NotImplementedException();

        /// <inheritdoc />
        public IElement create(IElement metaClass)
        {
            string uriMetaClass;
            if (metaClass is MofObjectShadow shadow)
            {
                uriMetaClass = shadow.Uri;
            }
            else
            {
                var elementAsMetaClass = metaClass as MofElement;
                var extentAsMofUriExtent = elementAsMetaClass?.Extent as MofUriExtent;
                if (elementAsMetaClass != null && extentAsMofUriExtent == null)
                {
                    throw new InvalidOperationException(
                        "We cannot create an instance by a metaclass which is not connected to an extent. It won't be found later on.");
                }

                if (extentAsMofUriExtent != null)
                {
                    Extent?.AddMetaExtent(extentAsMofUriExtent);
                }

                uriMetaClass = ((MofUriExtent) elementAsMetaClass?.Extent)?.uri(metaClass);
            }

            if (Extent == null)
            {
                throw new InvalidOperationException("Extent may not be null");
            }

            // If the instance of given type was created, add the extent's type to the meta extents
            if (metaClass?.GetExtentOf() is IUriExtent typeExtent)
            {
                Extent.AddMetaExtent(typeExtent);
            }

            return new MofElement(_provider.CreateElement(uriMetaClass), Extent).CreatedBy(Extent);
        }

        /// <summary>
        /// Creates an element by getting a dotnet value.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="id">Id of the element that shall be set</param>
        /// <returns>The created .Net object</returns>
        public IObject createFrom(object value, string id = null)
        {
            var result = (IProviderObject) Extent.ConvertForSetting(value);

            if (result != null && !string.IsNullOrEmpty(id))
            {
                result.Id = id;
            }

            return new MofElement(result, Extent);
        }

        /// <inheritdoc />
        public IObject createFromString(IElement dataType, string value)
            => throw new NotImplementedException();

        /// <inheritdoc />
        public string convertToString(IElement dataType, IObject value)
            => throw new NotImplementedException();


        public static MofFactory CreateByExtent(IUriExtent loadedExtent)
            => new MofFactory(loadedExtent);

        /// <summary>
        /// Creates an element within the same extent as the given
        /// element 'value' and finds the metaclass via
        /// the metaclass finder.
        /// </summary>
        /// <typeparam name="TFilledType">Type filler being used to find the element</typeparam>
        /// <param name="value">Value which is used to find the associated extent</param>
        /// <param name="metaClassFinder">The function to derive the metaclass out of the filler</param>
        /// <returns>Created element</returns>
        public static IElement CreateElementFor<TFilledType>(
            IObject value,
            Func<TFilledType, IElement> metaClassFinder)
            where TFilledType : class, new()
        {
            var factory = new MofFactory(value);
            return factory.Create(metaClassFinder);
        }

        /// <summary>
        /// Creates an element within the same extent as the given
        /// element 'value' and finds the metaclass via
        /// the metaclass finder.
        /// </summary>
        /// <typeparam name="TFilledType">Type filler being used to find the element</typeparam>
        /// <param name="value">Extent being used for the factory</param>
        /// <param name="metaClassFinder">The function to derive the metaclass out of the filler</param>
        /// <returns>Created element</returns>
        public static IElement CreateElementFor<TFilledType>(
            IExtent value,
            Func<TFilledType, IElement> metaClassFinder)
            where TFilledType : class, new()
        {
            var factory = new MofFactory(value);
            return factory.Create(metaClassFinder);
        }
    }
}