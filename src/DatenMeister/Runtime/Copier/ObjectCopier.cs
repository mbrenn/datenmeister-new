using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Copier
{
    /// <summary>
    /// The object copier can be used to copy one mof element to another mof element
    /// </summary>
    public class ObjectCopier
    {
        private readonly IFactory _factory;

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
            var targetElement = _factory.create((element as IElement)?.getMetaClass());
            CopyProperties(element, targetElement);

            return targetElement;
        }

        /// <summary>
        /// Copies all properties from source element to target sourceElement
        /// </summary>
        /// <param name="sourceElement">Source element which is verified</param>
        /// <param name="targetElement">Target element which is verified</param>
        private void CopyProperties(IObject sourceElement, IObject targetElement)
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
                var copiedElement = Copy(valueAsElement);
                return copiedElement;
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