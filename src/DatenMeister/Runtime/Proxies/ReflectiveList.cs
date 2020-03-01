using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Runtime.Proxies
{
    /// <summary>
    /// Abstracts a reflective collection/sequence into a list
    /// </summary>
    /// <typeparam name="T">Type of the elements of the list</typeparam>
    public class ReflectiveList<T> : IList<T> where T : notnull
    {
        private readonly IReflectiveCollection _collection;

        private readonly IReflectiveSequence _sequence;

        /// <summary>
        /// Stores the function that is used to convert an object to the given type
        /// </summary>
        private readonly Func<object, T> _wrapFunc;

        /// <summary>
        /// Converts the function that is used to unwrap the function
        /// </summary>
        private readonly Func<T, object> _unwrapFunc;


        public ReflectiveList(IReflectiveCollection collection)
        {
            _collection = collection;
            _sequence = _collection as IReflectiveSequence ??
                        throw new InvalidOperationException("Collection is not IReflectiveSequence");

            _wrapFunc = x => (T) x;
            _unwrapFunc = x => x;
        }

        public ReflectiveList(IReflectiveCollection collection, Func<object, T> wrapFunc, Func<T, object> unwrapFunc)
        {
            _collection = collection;
            _sequence = _collection as IReflectiveSequence;
            _wrapFunc = wrapFunc;
            _unwrapFunc = unwrapFunc;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in _collection)
            {
                yield return _wrapFunc(value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            _collection.add(_unwrapFunc(item));
        }

        public void Clear()
        {
            _collection.clear();
        }

        public bool Contains(T item)
        {
            var unwrapped = _unwrapFunc(item);
            return _collection.Any(x => x.Equals(unwrapped));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_sequence != null)
            {
                foreach (var value in array)
                {
                    _sequence.add(arrayIndex, _unwrapFunc(value));
                    arrayIndex++;
                }
            }
            else
            {
                foreach (var value in array)
                {
                    _collection.add(_unwrapFunc(value));
                    arrayIndex++;
                }
            }
        }

        public bool Remove(T item) => _collection.remove(_unwrapFunc(item));

        public int Count => _collection.size();

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            var n = 0;
            var unwrapped = _unwrapFunc(item);
            foreach (var value in _collection)
            {
                if (value.Equals(unwrapped))
                {
                    return n;
                }

                n++;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (_sequence != null)
            {
                _sequence.add(index, _unwrapFunc(item));
            }
            else
            {
                _collection.add(_unwrapFunc(item));
            }
        }

        public void RemoveAt(int index)
        {
            if (_sequence != null)
            {
                _sequence.remove(index);
            }
            else
            {
                throw new NotImplementedException("Collection is not given");
            }
        }

        public T this[int index]
        {
            get => _wrapFunc(_collection.ElementAt(index));
            set
            {
                if (_sequence != null)
                {
                    _sequence.set(index, _unwrapFunc(value));
                }
                else
                {
                    throw new NotImplementedException("Collection is not given");
                }
            }
        }
    }
}