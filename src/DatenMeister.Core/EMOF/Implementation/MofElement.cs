﻿#nullable enable

using DatenMeister.Core.EMOF.Implementation.DefaultValue;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class IdIsAlreadySetException : InvalidOperationException
    {
        public IdIsAlreadySetException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// Defines a Mof Element according to MOF specification
    /// </summary>
    public class MofElement : MofObject, IElement, IElementSetMetaClass, IHasId, ICanSetId
    {
        /// <summary>
        /// Stores the cached metaclass to speed-up lookup
        /// </summary>
        private IElement? _cachedMetaClass;

        /// <summary>
        /// Initialized a new instance of the MofElement class which is an abstraction of the provided database.
        /// </summary>
        /// <param name="providedObject">Provided object by database</param>
        /// <param name="extent">Extent to which the object is allocated to</param>
        /// <param name="referenceElement"></param>
        public MofElement(
            IProviderObject providedObject,
            MofExtent? extent,
            IElement? referenceElement = null)
            : base(providedObject, extent, referenceElement)
        {
        }

        /// <inheritdoc />
        public IElement? metaclass => getMetaClass();

        public override bool isSet(string property)
        {
            if (!IsSlimUmlEvaluation)
            {
                // Checks whether we have a derived property 
                var metaClass = getMetaClass();

                if (metaclass?.GetExtentOf() is MofExtent extent
                    && metaClass != null
                    && extent.DynamicFunctionManager?.HasDerivedProperty(metaClass, property) == true)
                {
                    return true;
                }
                
                // Checks whether we are having a default Value
                var defaultValue = DefaultValueHandler.ReadDefaultValueOfProperty<object>(this, property);
                if (defaultValue != null)
                {
                    // If we have a default value, then it is quite sure that the property is set!
                    return true;
                }
            }

            return base.isSet(property);
        }
        
        /// <inheritdoc />
        IElement? IElement.getMetaClass()
        {
            return getMetaClass();
        }

        /// <inheritdoc />
        public IElement? container()
        {
            var containerElement = ProviderObject.GetContainer();
            return containerElement != null
                ? new MofElement(containerElement, Extent ?? ReferencedExtent)
                    { Extent = Extent }
                : null;
        }

        /// <summary>
        /// Sets the container for the element
        /// </summary>
        public IObject? Container
        {
            set => ProviderObject.SetContainer(((MofObject?)value)?.ProviderObject);
        }

        /// <summary>
        /// Sets the meta class for the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to be set</param>
        public void SetMetaClass(IElement? metaClass)
        {
            _cachedMetaClass = metaClass is MofElement ? metaClass : null;
            if (metaClass is MofElement mofElement)
            {
                if (mofElement.Extent == null)
                {
                    throw new InvalidOperationException("The given metaclass is not connected to an element");
                }

                ProviderObject.MetaclassUri = ((MofUriExtent)mofElement.Extent).uri(mofElement);
            }
            else
            {
                ProviderObject.MetaclassUri = metaClass?.GetUri() ?? string.Empty;
            }

            UpdateContent();
        }

        /// <inheritdoc cref="ICanSetId.Id" />
        public string? Id
        {
            get => ProviderObject.Id;
            set
            {
                if (Id == value)
                {
                    // Id is not modified, so we can skip the action
                    return;
                }

                if (Extent is MofUriExtent mofUriExtent && !string.IsNullOrEmpty(value))
                {
                    var foundValue = mofUriExtent.element($"#{value!}");
                    if (foundValue != null && !foundValue.Equals(this))
                    {
                        throw new IdIsAlreadySetException("The ID is already set within the extent.");
                    }
                }

                ProviderObject.Id = value;
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
            => SetReferencedExtent((MofExtent)extent);

        protected override (bool, object?) GetDynamicProperty(string property)
        {
            if (!IsSlimUmlEvaluation)
            {
                var metaClass = getMetaClass();
                if (!(metaclass?.GetExtentOf() is MofExtent extent))
                {
                    return (false, null);
                }

                if (metaClass != null && extent.DynamicFunctionManager != null)
                {
                    return extent.DynamicFunctionManager.GetDerivedPropertyValue(this, metaClass, property);
                }

                return (false, null);
            }

            return (false, null);
        }

        public override object? get(string property, bool noReferences, ObjectType objectType)
        {
            // Checks, if we have a dynamic property
            var (isValid, resultValue) = GetDynamicProperty(property);
            if (isValid)
            {
                return ConvertToMofObject(this, property, resultValue, noReferences);
            }
            
            // Checks, if the property is set
            if (ProviderObject.IsPropertySet(property))
            {
                var result = ProviderObject.GetProperty(property, objectType);
                return ConvertToMofObject(this, property, result, noReferences);
            }
            else
            {
                // If not set, get the default value
                var result = DefaultValueHandler.ReadDefaultValueOfProperty<object?>(this, property);
                return ConvertToMofObject(this, property, result, noReferences);
            }
        }

        /// <inheritdoc />
        public IElement? getMetaClass(bool traceFailing = true)
        {
            if (_cachedMetaClass != null)
            {
                return _cachedMetaClass;
            }

            var uri = ProviderObject.MetaclassUri;
            if (uri == null || string.IsNullOrEmpty(uri))
            {
                // No metaclass Uri is given.
                return null;
            }

            var result =
                (ReferencedExtent as IUriResolver)
                ?.Resolve(uri, ResolveType.OnlyMetaClasses | ResolveType.AlsoTypeWorkspace, traceFailing) as IElement
                ?? new MofObjectShadow(uri);

            _cachedMetaClass = result;
            return result;
        }

        /// <summary>
        /// Sets the metaclass by directly setting the uri
        /// </summary>
        /// <param name="metaClassUri">Uri to be set</param>
        public void SetMetaClass(string metaClassUri)
        {
            _cachedMetaClass = null;
            ProviderObject.MetaclassUri = metaClassUri;

            UpdateContent();
        }

        /// <summary>
        /// Sets the extent by which the element was created
        /// </summary>
        /// <param name="extent">Extent being used to define the the reportCreator</param>
        /// <returns>this element. </returns>
        public new IElement CreatedBy(MofExtent extent)
        {
            base.CreatedBy(extent);
            return this;
        }
    }
}