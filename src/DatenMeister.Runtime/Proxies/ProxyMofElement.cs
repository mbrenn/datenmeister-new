using System;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyMofElement : ProxyMofObject, IElement, IElementSetMetaClass
    {
        protected IElement Element
        {
            get { return Object as IElement; }
        }

        public ProxyMofElement(IElement element) : base(element)
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

        public virtual void setContainer ( IElement container)
        {
            var asSetMetaClass = Element as IElementSetMetaClass;
            if (asSetMetaClass == null)
            {
                throw new InvalidOperationException("Element does not support interface IElementSetMetaClass");
            }

            asSetMetaClass.setContainer(container);
        }

        public virtual void setMetaClass(IElement metaClass)
        {
            var asSetMetaClass = Element as IElementSetMetaClass;
            if (asSetMetaClass == null)
            {
                throw new InvalidOperationException("Element does not support interface IElementSetMetaClass");
            }

            asSetMetaClass.setMetaClass(metaClass);
        }
    }
}
