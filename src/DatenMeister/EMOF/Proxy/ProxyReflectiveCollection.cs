using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveCollection : IReflectiveCollection
    {
        protected readonly IReflectiveCollection Collection;
        
        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being returned. 
        /// </summary>
        public Func<object, object> PublicizeElementFunc { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being returned. 
        /// </summary>
        public Func<object, object> PrivatizeElementFunc { get; set; }

        public ProxyReflectiveCollection(IReflectiveCollection collection)
        {
            Collection = collection;
            PublicizeElementFunc = x => x;
            PrivatizeElementFunc = x => x;
        }

        public virtual bool add(object value)
        {
            value = PrivatizeElementFunc(value);
            return Collection.add(value);
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            var result = false;
            foreach (var element in value.Select(x => PrivatizeElementFunc(x)))
            {
                result |= Collection.add(element);
            }

            return result;
        }

        public virtual void clear()
        {
            Collection.clear();
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            foreach (var obj in Collection)
            {
                yield return PublicizeElementFunc(obj);
            }
        }

        public virtual bool remove(object value)
        {
            value = PrivatizeElementFunc(value);
            return Collection.remove(value);
        }

        public virtual int size()
        {
            return Collection.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ProxyReflectiveCollection ActivateObjectConversion()
        {
            PublicizeElementFunc = x =>
            {
                var element = x as IElement;
                if (element != null)
                {
                    return new ProxyMofElement(element);
                }

                var asObject = x as IObject;
                if ( asObject != null )
                {
                    return new ProxyMofObject(asObject);
                }

                return x;
            };

            PrivatizeElementFunc = x =>
            {
                var element = x as ProxyMofElement;
                return element != null ? element.GetProxiedElement() : x;
            };

            return this;
        }
    }
}
