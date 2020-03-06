using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyReflectiveCollection : IReflectiveCollection, IHasExtent
    {
        protected IReflectiveCollection Collection { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the
        /// reflective collection is being extracted out of the reflective collection.
        /// </summary>
        public Func<object?, object?>? PublicizeElementFunc { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the
        /// reflective collection is being stored into the reflective collection.
        /// </summary>
        public Func<object?, object?>? PrivatizeElementFunc { get; set; }

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

        public virtual IEnumerator<object?> GetEnumerator()
        {
            foreach (var obj in Collection)
            {
                yield return PublicizeElementFunc(obj);
            }
        }

        public virtual bool remove(object? value)
        {
            value = PrivatizeElementFunc(value);
            return Collection.remove(value);
        }

        public virtual int size() =>
            Collection.size();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

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
                if (x is IElement asElement)
                    return publicizeElement(asElement);

                return x is IObject asObject ? publicizeObject(asObject) : x;
            };

            PrivatizeElementFunc = x =>
            {
                if (x is TElementType asElement)
                    return privatizeElement(asElement);

                return x is TObjectType asObject ? privatizeObject(asObject) : x;
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

        /// <summary>
        /// Gets the extent associated to the parent extent
        /// </summary>
        public IExtent? Extent => (Collection as IHasExtent)?.Extent;
    }
}
