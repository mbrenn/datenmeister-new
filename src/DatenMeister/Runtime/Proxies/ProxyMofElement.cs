using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyMofElement : ProxyMofObject, IElement, IElementSetMetaClass
    {
        protected MofElement Element
        {
            get { return Object as MofElement; }
        }

        public ProxyMofElement(MofElement element) : base(element)
        {
        }

        /// <summary>
        /// Gets the proxied element which can be used to dereference the 
        /// content
        /// </summary>
        /// <returns>Returns the proxied element</returns>
        public new IElement GetProxiedElement()
        {
            return Element;
        }

        public virtual IElement metaclass => Element.metaclass;

        public virtual IElement container()
        {
            return Element.container();
        }

        public virtual IElement getMetaClass()
        {
            return Element.getMetaClass();
        }

        public virtual void SetContainer ( IElement container)
        {
            var asSetMetaClass = Element as MofElement;
            if (asSetMetaClass == null)
            {
                throw new InvalidOperationException("Element does not support interface IElementSetMetaClass");
            }

            asSetMetaClass.SetContainer(container);
        }

        public virtual void SetMetaClass(IElement metaClass)
        {
            var asSetMetaClass = Element as IElementSetMetaClass;
            if (asSetMetaClass == null)
            {
                throw new InvalidOperationException("Element does not support interface IElementSetMetaClass");
            }

            asSetMetaClass.SetMetaClass(metaClass);
        }
    }
}
