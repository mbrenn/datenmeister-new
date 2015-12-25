using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyMofElement : IElement
    {
        protected IElement _element;

        public ProxyMofElement(IElement element)
        {
            _element = element;
        }

        public virtual IElement metaclass
        {
            get { return _element.metaclass; }
        }

        public virtual IElement container()
        {
            return _element.container();
        }

        public virtual bool equals(object other)
        {
            return _element.equals(other);
        }

        public object get(object property)
        {
            return _element.get(property);
        }

        public IElement getMetaClass()
        {
            return _element.getMetaClass();
        }

        public bool isSet(object property)
        {
            return _element.isSet(property);
        }

        public void set(object property, object value)
        {
            _element.set(property, value);
        }

        public void unset(object property)
        {
            _element.unset(property);
        }
    }
}