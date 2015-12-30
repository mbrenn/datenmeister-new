using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyMofObject : IObject
    {
        protected readonly IObject Object;

        public ProxyMofObject(IObject value)
        {
            Object = value;
        }

        /// <summary>
        /// Gets the proxied element which can be used to dereference the 
        /// content
        /// </summary>
        /// <returns>Returns the proxied element</returns>
        public IObject GetProxiedElement()
        {
            return Object;
        }

        public virtual bool @equals(object other)
        {
            return Object.@equals(other);
        }

        public virtual object get(object property)
        {
            return Object.get(property);
        }

        public virtual void set(object property, object value)
        {
            Object.set(property, value);
        }

        public virtual bool isSet(object property)
        {
            return Object.isSet(property);
        }

        public virtual void unset(object property)
        {
            Object.unset(property);
        }
    }
}