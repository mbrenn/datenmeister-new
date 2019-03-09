using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Runtime.Copier
{
    [Flags]
    public enum CopyOptions
    {
        None = 0x00,
        CopyId = 0x01
    }

    /// <summary>
    /// The object copier can be used to copy one mof element to another mof element
    /// </summary>
    public class ObjectCopier
    {
        /// <summary>
        /// Contains the factory method
        /// </summary>
        private readonly IFactory _factory;

        /// <summary>
        /// Stores the extent of the element to be copied. 
        /// This information is used to check whether an element shall be copied or a reference
        /// shall be used. Property values referencing to another extent are not copied... Instead uri 
        /// references are copied
        /// </summary>
        private IExtent _sourceExtent;

        /// <summary>
        /// Gets or sets a value indicating whether references shall be cloned, so there will no UriReferences
        /// </summary>
        public bool CloneAllReferences { get; set; }

        /// <summary>
        /// Initializes a new instance of the ObjectCopier. 
        /// </summary>
        /// <param name="factory"></param>
        public ObjectCopier(IFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Copies the element as given in <c>element</c>
        /// </summary>
        /// <param name="element">Element that shall be copied</param>
        /// <returns>true, if element has been successfully copied</returns>
        public IElement Copy(IObject element, CopyOptions copyOptions = CopyOptions.None)
        {
            // Gets the source extent
            _sourceExtent = (element as IHasExtent)?.Extent ?? (element as MofElement)?.CreatedByExtent;

            var targetElement = _factory.create((element as IElement)?.getMetaClass());
            CopyProperties(element, targetElement, copyOptions);

            return targetElement;
        }

        /// <summary>
        /// Copies all properties from source element to target sourceElement
        /// </summary>
        /// <param name="sourceElement">Source element which is verified</param>
        /// <param name="targetElement">Target element which is verified</param>
        public void CopyProperties(IObject sourceElement, IObject targetElement, CopyOptions copyOptions = CopyOptions.None)
        {
            if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));
            if (targetElement == null) throw new ArgumentNullException(nameof(targetElement));
            if (!(sourceElement is IObjectAllProperties elementAsExt))
            {
                throw new ArgumentException($"{nameof(sourceElement)} is not of type IObjectAllProperties");
            }

            // Transfers the id, if requested by the copy options
            if (copyOptions.HasFlag(CopyOptions.CopyId)
                && sourceElement is IHasId sourceWithId 
                && targetElement is ICanSetId targetCanSetId)
            {
                targetCanSetId.Id = sourceWithId.Id;
            }

            // Transfers the properties
            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var value = sourceElement.get(property);
                var result = CopyValue(value, targetElement as IElement, copyOptions);

                targetElement.set(property, result);
            }
        }

        private object CopyValue(object value, IElement containingElement, CopyOptions copyOptions)
        {
            if (value == null)
            {
                return null;
            }

            if (value is IElement valueAsElement)
            {
                var propertyExtent = (valueAsElement as IHasExtent)?.Extent;
                if (propertyExtent == null || propertyExtent == _sourceExtent || CloneAllReferences)
                {
                    return Copy(valueAsElement, copyOptions);
                }
                else
                {
                    // See above... Don't copy the elements which are references by another extent
                    return value;
                }
            }

            if (value is IReflectiveCollection valueAsCollection)
            {
                return valueAsCollection
                    .Select(innerValue => CopyValue(innerValue, containingElement, copyOptions));
            }

            return value;
        }

        /// <summary>
        /// Copies the given element by using the factory
        /// </summary>
        /// <param name="factory">Factory to be used to create the element</param>
        /// <param name="element">Element to be copied</param>
        /// <returns>The created element that will be copied</returns>
        public static IElement Copy(IFactory factory, IObject element, CopyOptions copyOptions = CopyOptions.None)
        {
            var copier = new ObjectCopier(factory);
            return copier.Copy(element, copyOptions);
        }

        /// <summary>
        /// Copies the given element by using the factory
        /// </summary>
        /// <param name="factory">Factory to be used to create the element</param>
        /// <param name="element">Element to be copied</param>
        /// <returns>The created element that will be copied</returns>
        public static void CopyPropertiesStatic(IObject source, IObject target, CopyOptions copyOptions = CopyOptions.None)
        {
            var copier = new ObjectCopier(new MofFactory(target));
            copier.CopyProperties(source, target, copyOptions);
        }

        /// <summary>
        /// Copies the element for a temporary usage. Here, the in memory Object will be used
        /// </summary>
        /// <param name="value">Value to copied</param>
        /// <returns>Element being copied</returns>
        public static IObject CopyForTemporary(IObject value)
        {
            return Copy(InMemoryObject.TemporaryFactory, value, CopyOptions.None);
        }
    }
}