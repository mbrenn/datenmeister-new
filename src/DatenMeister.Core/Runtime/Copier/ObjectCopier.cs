using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.Core.Runtime.Copier
{
    public static class CopyOptions
    {
        /// <summary>
        /// Gets the default copy options which create new ids for each copied element
        /// </summary>
        public static CopyOption None { get; } = new();

        /// <summary>
        /// Gets the copy options which copies also the ids
        /// </summary>
        public static CopyOption CopyId => new() {CopyId = true};
    }

    public class CopyOption
    {
        /// <summary>
        /// Gets or sets a value indicating whether the ids of the objects shall be copied or whether a new id shall be generated for each copied element
        /// </summary>
        public bool CopyId { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether references shall be cloned, so there will no UriReferences
        /// </summary>
        public bool CloneAllReferences { get; set; }

        /// <summary>
        /// True, if only the primitive elements shall be copied and not any recursion
        /// </summary>
        public bool NoRecursion { get; set; }
    }

    /// <summary>
    /// The object copier can be used to copy one mof element to another mof element
    /// </summary>
    public class ObjectCopier
    {
        /// <summary>
        /// Defines the maximum recursion depth that is accepted by the object copier
        /// </summary>
        private const int MaxRecursionDepth = 100;

        /// <summary>
        /// Contains the factory method
        /// </summary>
        private readonly IFactory _factory;

        /// <summary>
        ///     Stores the current depth
        /// </summary>
        private int _currentDepth;

        /// <summary>
        /// Stores the extent of the element to be copied.
        /// This information is used to check whether an element shall be copied or a reference
        /// shall be used. Property values referencing to another extent are not copied... Instead uri
        /// references are copied
        /// </summary>
        private IExtent? _sourceExtent;

        /// <summary>
        /// Initializes a new instance of the ObjectCopier.
        /// </summary>
        /// <param name="factory">Factory being used to get added </param>
        public ObjectCopier(IFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Copies the element as given in <c>element</c>
        /// </summary>
        /// <param name="element">Element that shall be copied</param>
        /// <param name="copyOptions">Defines the option being used for copying</param>
        /// <returns>true, if element has been successfully copied</returns>
        public IElement Copy(IObject element, CopyOption? copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;

            // Gets the source extent
            _sourceExtent = (element as IHasExtent)?.Extent ?? (element as MofElement)?.ReferencedExtent
                ?? throw new InvalidOperationException("element is not IHasExtent and not MofElement");

            IElement targetElement;
            if (element is MofObject {IsSlimUmlEvaluation: true} asObject)
            {
                targetElement = _factory.create(
                    asObject.ProviderObject.MetaclassUri == null
                        ? null
                        : new MofObjectShadow(asObject.ProviderObject.MetaclassUri));
            }
            else
            {
                targetElement = _factory.create((element as IElement)?.getMetaClass());
            }

            CopyProperties(element, targetElement, copyOptions);

            return targetElement;
        }

        /// <summary>
        /// Copies all properties from source element to target sourceElement
        /// </summary>
        /// <param name="sourceElement">Source element which is verified</param>
        /// <param name="targetElement">Target element which is verified</param>
        /// <param name="copyOptions">Options to be copied</param>
        public void CopyProperties(IObject sourceElement, IObject targetElement, CopyOption? copyOptions = null)
        {
            _currentDepth++;
            if (_currentDepth >= MaxRecursionDepth)
            {
                // Can't go deeper
                return;
            }

            copyOptions ??= CopyOptions.None;

            if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));
            if (targetElement == null) throw new ArgumentNullException(nameof(targetElement));
            if (!(sourceElement is IObjectAllProperties elementAsExt))
            {
                throw new ArgumentException($"{nameof(sourceElement)} is not of type IObjectAllProperties");
            }

            // Transfers the id, if requested by the copy options
            if (copyOptions.CopyId
                && sourceElement is IHasId sourceWithId
                && targetElement is ICanSetId targetCanSetId)
            {
                var id = sourceWithId.Id;
                if (id != null)
                {
                    targetCanSetId.Id = id;
                }
            }

            // Transfers the properties
            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var value = sourceElement.get<object>(property, !copyOptions.CloneAllReferences);
                var result = CopyValue(value, copyOptions);

                targetElement.set(property, result);
            }

            _currentDepth--;
        }

        /// <summary>
        /// Copies the value, so it can be added to the target extent
        /// </summary>
        /// <param name="value">Value to be copied</param>
        /// <param name="copyOptions">Copy options being used</param>
        /// <returns>The object that has been copied</returns>
        private object? CopyValue(object? value, CopyOption? copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;
            var noRecursion = copyOptions.NoRecursion;

            switch (value)
            {
                case null:
                case IElement _ when noRecursion:
                    return null;

                case MofObjectShadow asMofObjectShadow:
                    return asMofObjectShadow;

                case IElement valueAsElement:
                {
                    var propertyExtent = (valueAsElement as IHasExtent)?.Extent;
                    if (propertyExtent == null || propertyExtent == _sourceExtent || copyOptions.CloneAllReferences)
                    {
                        // If element is not associated to an extent
                        // Or is associated to the source extent (as it should be)
                        // Or if all references shall be cloned
                        // The element will be copied.
                        return Copy(valueAsElement, copyOptions);
                    }

                    // See above... Don't copy the elements which are references by another extent
                    return value;
                }
                case IReflectiveCollection _ when noRecursion:
                    return null;
                case IReflectiveCollection valueAsCollection:
                    return valueAsCollection
                        .Select(innerValue => CopyValue(innerValue, copyOptions));
                default:
                    return value;
            }
        }

        /// <summary>
        /// Copies the given element by using the factory
        /// </summary>
        /// <param name="factory">Factory to be used to create the element</param>
        /// <param name="element">Element to be copied</param>
        /// <param name="copyOptions">Options for copying</param>
        /// <returns>The created element that will be copied</returns>
        public static IElement Copy(IFactory factory, IObject element, CopyOption? copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;

            var copier = new ObjectCopier(factory);
            return copier.Copy(element, copyOptions);
        }

        /// <summary>
        /// Copies the given element by using the factory
        /// </summary>
        /// <param name="source">Element to be copied</param>
        /// <param name="target">Target of the element</param>
        /// <param name="copyOptions">Defines the objects being used for copying</param>
        /// <returns>The created element that will be copied</returns>
        public static void CopyPropertiesStatic(IObject source, IObject target, CopyOption? copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;

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
            if (value is MofObject mofObject)
            {
                InMemoryProvider.TemporaryExtent.AddMetaExtents((mofObject).ReferencedExtent.MetaExtents);
            }

            // Adds the data workspaces
            return Copy(InMemoryObject.TemporaryFactory, value, CopyOptions.None);
        }

        /// <summary>
        /// Copies the element for a temporary usage. Here, the in memory Object will be used
        /// </summary>
        /// <param name="value">Value to copied</param>
        /// <param name="copyOptions">Defines the objects being used for copying</param>
        /// <returns>Element being copied</returns>
        public static IReflectiveCollection CopyForTemporary(IReflectiveCollection value,
            CopyOption? copyOptions = null)
        {
            var temp = new TemporaryReflectiveCollection();
            copyOptions ??= CopyOptions.None;

            foreach (var item in value.OfType<IObject>())
            {
                temp.add(Copy(InMemoryObject.TemporaryFactory, item, copyOptions));
            }

            return temp;
        }
    }
}