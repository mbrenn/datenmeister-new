using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Exceptions;
using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.EMOF.InMemory
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

        public bool add(object value)
        {
            _values.Add(value);
            return true;
        }

        public void add(int index, object value)
        {
            _values.Insert(index, value);
        }

        public bool addAll(IReflectiveSequence values)
        {
            var result = false;
            foreach (var value in values)
            {
                result |= add(value);
            }

            return result;
        }

        public void clear()
        {
            _values.Clear();
        }

        public object get(int index)
        {
            CheckIndex(index);

            return _values[index];
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public bool remove(object value)
        {
            return _values.Remove(value);
        }

        public void remove(int index)
        {
            CheckIndex(index);

            _values.RemoveAt(index);
        }

        public object set(int index, object value)
        {
            CheckIndex(index);
            var old = _values[index];
            _values[index] = value;
            return old;
        }

        public int size()
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