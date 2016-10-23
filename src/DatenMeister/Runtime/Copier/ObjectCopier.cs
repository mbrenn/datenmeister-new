using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Copier
{
    public class ObjectCopier
    {
        private readonly IFactory _factory;

        public ObjectCopier(IFactory factory)
        {
            _factory = factory;
        }

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
            var elementAsExt = sourceElement as IObjectAllProperties;
            if (elementAsExt == null)
            {
                throw new ArgumentException($"{nameof(sourceElement)} is not of type IObjectAllProperties");
            }

            // Transfers the id
            var sourceWithId = sourceElement as IHasId;
            var targetCanSetId = targetElement as ICanSetId;
            if (sourceWithId != null && targetCanSetId != null)
            {
                targetCanSetId.Id = sourceWithId.Id;
            }

            // Transfers the properties
            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                object result;

                var value = sourceElement.get(property);
                result = CopyValue(value);

                targetElement.set(property, result);
            }
        }

        private object CopyValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            var valueAsElement = value as IElement;
            if (valueAsElement != null)
            {
                return Copy(valueAsElement);
            }

            var valueAsCollection = value as IReflectiveCollection;
            if (valueAsCollection != null)
            {
                return valueAsCollection
                    .Select(innerValue => CopyValue(innerValue))
                    .ToList();
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