using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        ///     Stores the sequence
        /// </summary>
        protected readonly IReflectiveSequence Sequence;

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

        public ProxyReflectiveSequence(IReflectiveSequence sequence)
        {
            Sequence = sequence;
        }

        public virtual bool add(object value)
        {
            return Sequence.add(PrivatizeElementFunc(value));
        }

        public virtual void add(int index, object value)
        {
            Sequence.add(index, PrivatizeElementFunc(value));
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            var result = false;

            foreach (var element in value.Select(x => PrivatizeElementFunc(x)))
            {
                result |= Sequence.add(element);
            }

            return result;
        }

        public virtual void clear()
        {
            Sequence.clear();
        }

        public virtual object get(int index)
        {
            return PublicizeElementFunc( Sequence.get(index));
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            foreach (var obj in Sequence)
            {
                yield return PublicizeElementFunc(obj);
            }
        }

        public virtual bool remove(object value)
        {
            return Sequence.remove(PrivatizeElementFunc(value));
        }

        public virtual void remove(int index)
        {
            Sequence.remove(index);
        }

        public virtual object set(int index, object value)
        {
            return PublicizeElementFunc (Sequence.set(index, PrivatizeElementFunc(value)));
        }

        public virtual int size()
        {
            return Sequence.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ProxyReflectiveSequence ActivateObjectConversion()
        {
            PublicizeElementFunc = x =>
            {
                var element = x as IElement;
                if (element != null)
                {
                    return new ProxyMofElement(element);
                }

                if (x is IObject)
                {
                    throw new InvalidOperationException("Not supported");
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
