using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Copier
{
    public class ObjectCopier
    {
        private readonly IFactory _factory;

        public ObjectCopier(IFactory factory)
        {
            _factory = factory;
        }

        public IElement Copy(IElement element)
        {
            var targetElement = _factory.create(element.getMetaClass());

            var elementAsExt = element as IObjectAllProperties;
            if (elementAsExt == null)
            {
                throw new ArgumentException("element is not of type IObjectAllProperties");
            }

            foreach (var property in elementAsExt.getPropertiesBeingSet())
            {
                var value = element.get(property);
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

            return targetElement;
        }
    }
}