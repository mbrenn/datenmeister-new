using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyMofElement : IElement
    {
        protected readonly IElement Element;

        public ProxyMofElement(IElement element)
        {
            Element = element;
        }

        /// <summary>
        /// Gets the proxied element which can be used to dereference the 
        /// content
        /// </summary>
        /// <returns>Returns the proxied element</returns>
        public IElement GetProxiedElement()
        {
            return Element;
        }

        public virtual IElement metaclass => Element.metaclass;

        public virtual IElement container()
        {
            return Element.container();
        }

        public virtual bool equals(object other)
        {
            return Element.equals(other);
        }

        public virtual object get(object property)
        {
            return Element.get(property);
        }

        public virtual IElement getMetaClass()
        {
            return Element.getMetaClass();
        }

        public virtual bool isSet(object property)
        {
            return Element.isSet(property);
        }

        public virtual void set(object property, object value)
        {
            Element.set(property, value);
        }

        public virtual void unset(object property)
        {
            Element.unset(property);
        }
    }
}
