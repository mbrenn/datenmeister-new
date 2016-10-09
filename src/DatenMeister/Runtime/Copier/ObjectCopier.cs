using System;
using System.Collections.Generic;
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
        public void CopyProperties(IObject sourceElement, IObject targetElement)
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
                var value = sourceElement.get(property);
                if (value is IElement)
                {
                    targetElement.set(property, Copy(value as IElement));
                }
                if (value is IReflectiveCollection)
                {
                    var list = new List<object>();
                    foreach (var innerValue in (value as IReflectiveCollection))
                    {
                        list.Add(Copy(innerValue as IElement));
                    }

                    targetElement.set(property, list);
                }
                else
                {
                    targetElement.set(property, value);
                }
            }
        }
    }
}