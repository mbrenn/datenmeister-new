using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.EMOF.InMemory
{
    public class MofReflectiveSequence : IReflectiveSequence
    {
        private readonly List<object> _values;

        public MofReflectiveSequence()
        {
            _values = new List<object>();
        }

        public MofReflectiveSequence(List<object> values)
        {
            _values = values;
        }

        public virtual bool add(object value)
        {
            return addInternal(value);
        }

        private bool addInternal(object value)
        {
            _values.Add(value);
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
                result |= addInternal(value);
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
            _values[index] = value;
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

        public static MofReflectiveSequence Create<T>(List<T> values)
        {
            return new MofReflectiveSequence(values.Cast<object>().ToList());
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