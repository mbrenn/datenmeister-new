﻿using System;
using System.Collections;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyReflectiveSequence : ProxyReflectiveCollection, IReflectiveSequence
    {
        /// <summary>
        ///     Stores the sequence
        /// </summary>
        protected IReflectiveSequence Sequence => (IReflectiveSequence) Collection;

        public ProxyReflectiveSequence(IReflectiveSequence sequence) : base(sequence)
        {
        }

        public virtual void add(int index, object value)
        {
            if (PrivatizeElementFunc == null)
                throw new InvalidOperationException("PrivatizeElementFunc is not set");

            Sequence.add(index, PrivatizeElementFunc(value));
        }

        public virtual object? get(int index)
        {
            if (PublicizeElementFunc == null)
                throw new InvalidOperationException("PublicizeElementFunc is not set");
            
            return PublicizeElementFunc(Sequence.get(index));
        }

        public virtual void remove(int index)
        {
            Sequence.remove(index);
        }

        public virtual object set(int index, object value)
        {
            if (PublicizeElementFunc == null)
                throw new InvalidOperationException("PublicizeElementFunc is not set");
            
            return PublicizeElementFunc(Sequence.set(index, PrivatizeElementFunc(value)));
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public new ProxyReflectiveSequence ActivateObjectConversion()
        {
            base.ActivateObjectConversion();
            return this;
        }
    }
}
