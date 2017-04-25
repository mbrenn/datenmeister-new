using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyMofElement : ProxyMofObject, IElement, IElementSetMetaClass
    {
        private MofElement Element => Object as MofElement;

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

        public IObject Container
        {
            get => Element.Container as IElement;
            set => Element.Container = value;
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
