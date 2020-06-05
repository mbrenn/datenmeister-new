#nullable enable

using System;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.EMOF.Implementation.DefaultValue;
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
        private IProvider _provider;

        /// <summary>
        /// Gets the Mof Extent being connected to the factory
        /// </summary>
        public MofExtent? Extent { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(MofExtent extent)
        {
            InitializeByMofExtent(extent);
            _provider = _provider ?? throw new InvalidOperationException("Provider is not set");
        }

        /// <summary>
        /// Initializes a new instance of the MofFactory
        /// </summary>
        /// <param name="collection">Colleciton to be used</param>
        public MofFactory(IReflectiveCollection collection) : this(((IHasExtent) collection).Extent ?? throw new InvalidOperationException("extent"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the MofFactory by retrieving an object
        /// </summary>
        /// <param name="value">Value to be set</param>
        public MofFactory(IObject value)
        {
            // Checks, if the given value is a mof extent, and if yes, initialize by mof extent
            if (value is MofExtent mofExtent)
            {
                InitializeByMofExtent(mofExtent);
                _provider = _provider ?? throw new InvalidOperationException("Provider is not set");
                return;
            }

            if (value is MofObjectShadow)
                throw new InvalidOperationException("A MofObjectShadow cannot be used as argument for MofFactory");

            var asMofObject = value as MofObject
                              ?? throw new InvalidOperationException(
                                  "value is null or not of type MofObject: It is "
                                  + (value == null ? "null" : value.GetType().FullName));
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

        /// <summary>
        /// Performs the initialization (constructor) by the MofExtent
        /// </summary>
        /// <param name="extent"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void InitializeByMofExtent(MofExtent extent)
        {
            Extent = extent ?? throw new ArgumentNullException(nameof(extent));

            _provider = extent.Provider;
            if (_provider == null) throw new ArgumentNullException(nameof(_provider));
        }

        /// <inheritdoc />
        public IElement package => throw new NotImplementedException();

        /// <inheritdoc />
        public IElement create(IElement? metaClass)
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

                // Sets the uri of the metaclas
                if (metaClass == null)
                {
                    uriMetaClass = string.Empty;
                }
                else
                {
                    uriMetaClass = (elementAsMetaClass?.Extent as MofUriExtent)?.uri(metaClass) ?? string.Empty;     
                }
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

            var created = new MofElement(_provider.CreateElement(uriMetaClass), Extent).CreatedBy(Extent);
            AutoEnumerateHandler.HandleNewItem(Extent, created);
            DefaultValueHandler.HandleNewItem(created);

            return created;
        }

        /// <summary>
        /// Creates an element by getting a dotnet value.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="id">Id of the element that shall be set</param>
        /// <returns>The created .Net object</returns>
        public IObject createFrom(object value, string? id = "")
        {
            if (Extent == null)
            {
                throw new InvalidOperationException("Extent must set to convert a .Net Type value");
            }

            var result = Extent.ConvertForSetting(value) as IProviderObject;
            if (result == null)
            {
                throw new InvalidOperationException("Object could not be converted to an I");
            }

            if (!string.IsNullOrEmpty(id))
            {
                result.Id = id ?? string.Empty;
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

        /// <summary>
        /// Just a short call to create a new mof factory instance and call the create method
        /// </summary>
        /// <param name="extent">Extent for which the element will be created. The element will not be included
        /// into the extent</param>
        /// <param name="metaClass">Meta class whose element will be created</param>
        /// <returns>The created element</returns>
        public static IElement Create(IExtent extent, IElement metaClass)
        {
            return new MofFactory(extent).create(metaClass);
        }

        /// <summary>
        /// Just a short call to create a new mof factory instance and call the create method
        /// </summary>
        /// <param name="element">Element to be included</param>
        /// <param name="metaClass">Meta class whose element will be created</param>
        /// <returns>The created element</returns>
        public static IElement Create(IObject element, IElement metaClass)
        {
            return new MofFactory(element).create(metaClass);
        }
    }
}