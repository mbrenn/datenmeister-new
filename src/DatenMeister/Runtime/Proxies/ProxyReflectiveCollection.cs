﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyReflectiveCollection : IReflectiveCollection
    {
        protected readonly IReflectiveCollection Collection;
        
        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being extracted out of the reflective collection. 
        /// </summary>
        public Func<object, object> PublicizeElementFunc { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being stored into the reflective collection. 
        /// </summary>
        public Func<object, object> PrivatizeElementFunc { get; set; }
        

        public ProxyReflectiveCollection(IReflectiveCollection collection)
        {
            Collection = collection;
            ActivateObjectConversion();
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

        public ProxyReflectiveCollection ActivateObjectConversion<TObjectType, TElementType>(
            Func<IElement, TElementType> publicizeElement,
            Func<IObject, TObjectType> publicizeObject,
            Func<TElementType, IElement> privatizeElement,
            Func<TObjectType, IObject> privatizeObject) 
            where TObjectType : class 
            where TElementType : class
        {
            PublicizeElementFunc = x =>
            {
                var asElement = x as IElement;
                if (asElement != null)
                {
                    return publicizeElement(asElement);
                }

                var asObject = x as IObject;
                return asObject != null ? publicizeObject(asObject) : x;
            };

            PrivatizeElementFunc = x =>
            {
                var asElement = x as TElementType;
                if (asElement != null)
                {
                    return privatizeElement(asElement);
                }

                var asObject = x as TObjectType;
                return asObject != null ? privatizeObject(asObject) : x;
            };

            return this;
        }

        public ProxyReflectiveCollection ActivateObjectConversion()
        {
            return ActivateObjectConversion(
                x => new ProxyMofElement((MofElement) x),
                x => new ProxyMofObject(x),
                x => x.GetProxiedElement(),
                x => x.GetProxiedElement());
        }
    }
}
