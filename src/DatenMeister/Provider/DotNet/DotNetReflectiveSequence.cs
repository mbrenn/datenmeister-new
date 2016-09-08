using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetReflectiveSequence<T> : IReflectiveSequence, IDotNetReflectiveSequence
    {
        private readonly IList<T> _list;
        private readonly IDotNetTypeLookup _typeLookup;
        private readonly DotNetElement _container;

        /// <summary>
        /// Stores the extent if it is directly owned by the extent itself (extent.elements())
        /// </summary>
        private DotNetExtent _extent;

        public DotNetReflectiveSequence(IList<T> list, IDotNetTypeLookup typeLookup, DotNetElement container)
        {
            _list = list;
            _typeLookup = typeLookup;
            _container = container;
        }

        public void SetExtent(DotNetExtent extent)
        {
            _extent = extent;
        }

        /// <summary>Gibt einen Enumerator zurück, der die Auflistung durchläuft.</summary>
        /// <returns>Ein Enumerator, der zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        public IEnumerator<object> GetEnumerator()
        {
            lock (_list)
            {
                foreach (var value in _list)
                {
                    yield return _typeLookup.CreateDotNetElementIfNecessary(value, _container, _extent);
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
                return _typeLookup.CreateDotNetElementIfNecessary(_list[index], _container, _extent);
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
                var oldValue = _typeLookup.CreateDotNetElementIfNecessary(_list[index], _container, _extent);
                _list[index] = (T) Extensions.ConvertToNative(value);
                return oldValue;
            }
        }
    }
}
