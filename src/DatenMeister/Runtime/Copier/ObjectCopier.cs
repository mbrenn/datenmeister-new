using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Copier
{
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
        public IElement Copy(IObject element)
        {
            // Gets the source extent
            _sourceExtent = (element as IHasExtent)?.Extent;

            var targetElement = _factory.create((element as IElement)?.getMetaClass());
            CopyProperties(element, targetElement);

            return targetElement;
        }

        /// <summary>
        /// Copies all properties from source element to target sourceElement
        /// </summary>
        /// <param name="sourceElement">Source element which is verified</param>
        /// <param name="targetElement">Target element which is verified</param>
        public void CopyProperties(IObject sourceElement, IObject targetElement)
        {
            if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));
            if (targetElement == null) throw new ArgumentNullException(nameof(targetElement));
            if (!(sourceElement is IObjectAllProperties elementAsExt))
            {
                throw new ArgumentException($"{nameof(sourceElement)} is not of type IObjectAllProperties");
            }

            // Transfers the id
            if (sourceElement is IHasId sourceWithId && targetElement is ICanSetId targetCanSetId)
            {
                targetCanSetId.Id = sourceWithId.Id;
            }

            // Transfers the properties
            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var value = sourceElement.get(property);
                var result = CopyValue(value, targetElement as IElement);

                targetElement.set(property, result);
            }
        }

        private object CopyValue(object value, IElement containingElement)
        {
            if (value == null)
            {
                return null;
            }

            if (value is IElement valueAsElement)
            {
                var propertyExtent = (valueAsElement as IHasExtent)?.Extent;
                if (propertyExtent == null || propertyExtent == _sourceExtent)
                {
                    return Copy(valueAsElement);
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
                    .Select(innerValue => CopyValue(innerValue, containingElement));
            }

            return value;
        }


        /// <summary>
        /// Copies the given element by using the factory
        /// </summary>
        /// <param name="factory">Factory to be used to create the element</param>
        /// <param name="element">Element to be copied</param>
        /// <returns>The created element that will be copied</returns>
        public static IElement Copy(IFactory factory, IObject element)
        {
            var copier = new ObjectCopier(factory);
            return copier.Copy(element);
        }
    }
}