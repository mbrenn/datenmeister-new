using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyMofObject : IHasProxiedObject, IObject, IObjectAllProperties
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
        public IObject GetProxiedElement() =>
            Object;

        public virtual bool equals(object? other) =>
            Object.equals(other);

        public virtual object get(string property) =>
            Object.get(property);

        public virtual void set(string property, object? value)
        {
            Object.set(property, value);
        }

        public virtual bool isSet(string property) =>
            Object.isSet(property);

        public virtual void unset(string property)
        {
            Object.unset(property);
        }

        /// <summary>
        /// Gets the properties of the stored object, if the object
        /// supports the interface
        /// </summary>
        /// <returns>Enumeration of objects</returns>
        public virtual IEnumerable<string> getPropertiesBeingSet()
        {
            if (!(Object is IObjectAllProperties asAllProperties))
                throw new InvalidOperationException("Proxied element does not support interface IObjectAllProperties");

            return asAllProperties.getPropertiesBeingSet();
        }
    }
}