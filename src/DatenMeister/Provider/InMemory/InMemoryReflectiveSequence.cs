using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.InMemory
{
    public class InMemoryReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        /// Stores the extent belonging to the given extent
        /// </summary>
        private readonly IUriExtent _localExtent;

        private InMemoryObject _owner;

        private readonly List<object> _values;

        public InMemoryReflectiveSequence(IUriExtent localExtent, InMemoryObject owner)
        {
            _localExtent = localExtent;
            _owner = owner;
            _values = new List<object>();
        }

        public InMemoryReflectiveSequence(IUriExtent localExtent, InMemoryObject owner, List<object> values)
        {
            _values = values;
            _localExtent = localExtent;
            _owner = owner;
        }

        public virtual bool add(object value)
        {
            return AddInternal(value);
        }

        private bool AddInternal(object value)
        {
            _values.Add(InMemoryObject.ConvertToInMemoryElement(value, null, _localExtent));
            return true;
        }

        public virtual void add(int index, object value)
        {
            _values.Insert(index, value);
        }

        public virtual bool addAll(IReflectiveSequence values)
        {
            var result = false;
            foreach (var value in values)
            {
                result |= AddInternal(value);
            }

            return result;
        }

        public virtual void clear()
        {
            _values.Clear();
        }

        public virtual object get(int index)
        {
            CheckIndex(index);

            return InMemoryObject.VerifyExtentOfObject(_owner, _values[index]);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public virtual bool remove(object value)
        {
            return _values.Remove(value);
        }

        public virtual void remove(int index)
        {
            CheckIndex(index);

            _values.RemoveAt(index);
        }

        public virtual object set(int index, object value)
        {
            CheckIndex(index);
            var old = _values[index];
            _values[index] = InMemoryObject.ConvertToInMemoryElement(value, null, _localExtent);
            return InMemoryObject.VerifyExtentOfObject(_owner, old);
        }

        public virtual int size()
        {
            return _values.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_values as IEnumerable).GetEnumerator();
        }

        public static InMemoryReflectiveSequence Create<T>(IUriExtent extent, InMemoryObject owner, List<T> values)
        {
            return new InMemoryReflectiveSequence(extent, owner, values.Cast<object>().ToList());
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index > _values.Count)
            {
                throw new MofException("IndexOutOfBoundsException");
            }
        }
    }
}