using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetReflectiveSequence<T> : IReflectiveSequence
    {
        private readonly IList<T> _list;
        private readonly IDotNetTypeLookup _typeLookup;

        public DotNetReflectiveSequence(IList<T> list, IDotNetTypeLookup typeLookup)
        {
            _list = list;
            _typeLookup = typeLookup;
        }

        /// <summary>Gibt einen Enumerator zurück, der die Auflistung durchläuft.</summary>
        /// <returns>Ein Enumerator, der zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        public IEnumerator<object> GetEnumerator()
        {
            lock (_list)
            {
                foreach (var value in _list)
                {
                    yield return _typeLookup.CreateDotNetElementIfNecessary(value);
                }
            }
        }

        /// <summary>Gibt einen Enumerator zurück, der eine Auflistung durchläuft.</summary>
        /// <returns>Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool add(object value)
        {
            lock (_list)
            {
                _list.Add((T) Extensions.ConvertToNative(value));
                return true;
            }
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
            lock (_list)
            {
                _list.Clear();
            }
        }

        public bool remove(object value)
        {
            lock (_list)
            {
                return _list.Remove((T) Extensions.ConvertToNative(value));
            }
        }

        public int size()
        {
            return _list.Count;
        }

        public void add(int index, object value)
        {
            lock (_list)
            {
                _list.Insert(index, (T) Extensions.ConvertToNative(value));
            }
        }

        public object get(int index)
        {
            lock (_list)
            {
                return _typeLookup.CreateDotNetElementIfNecessary(_list[index]);
            }
        }

        public void remove(int index)
        {
            lock (_list)
            {
                _list.RemoveAt(index);
            }
        }

        public object set(int index, object value)
        {
            lock (_list)
            {
                var oldValue = _typeLookup.CreateDotNetElementIfNecessary(_list[index]);
                _list[index] = (T) Extensions.ConvertToNative(value);
                return oldValue;
            }
        }
    }
}
