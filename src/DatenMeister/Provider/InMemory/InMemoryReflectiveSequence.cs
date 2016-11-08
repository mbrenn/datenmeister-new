using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Provider.InMemory
{
    public class InMemoryReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        /// Stores the extent belonging to the given extent
        /// </summary>
        private IUriExtent _localExtent;

        private readonly List<object> _values;

        public InMemoryReflectiveSequence(IUriExtent localExtent)
        {
            _localExtent = localExtent;
            _values = new List<object>();
        }

        public InMemoryReflectiveSequence(IUriExtent localExtent, List<object> values)
        {
            _values = values;
            _localExtent = localExtent;
        }

        public virtual bool add(object value)
        {
            return AddInternal(value);
        }

        private bool AddInternal(object value)
        {
            _values.Add(InMemoryObject.ConvertToInMemoryElement(value, _localExtent));
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

            return _values[index];
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
            _values[index] = InMemoryObject.ConvertToInMemoryElement(value, _localExtent);
            return old;
        }

        public virtual int size()
        {
            return _values.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_values as IEnumerable).GetEnumerator();
        }

        public static InMemoryReflectiveSequence Create<T>(IUriExtent extent, List<T> values)
        {
            return new InMemoryReflectiveSequence(extent, values.Cast<object>().ToList());
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