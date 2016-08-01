using System;
using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Helper
{
    public class ReflectiveSequenceForExtent : IReflectiveSequence, IExtentCachesObject
    {
        private readonly IReflectiveSequence _reflectiveSequence;
        private readonly IUriExtent _extent;
        private readonly HashSet<object> _cachedObjects = new HashSet<object>();

        public ReflectiveSequenceForExtent(IUriExtent extent, IReflectiveSequence sequence)
        {
            if (extent == null) throw new ArgumentNullException(nameof(extent));
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            _reflectiveSequence = sequence;
            _extent = extent;
        }

        public bool add(object value)
        {
            lock (_cachedObjects)
            {
                var result = _reflectiveSequence.add(value);
                if (result)
                {
                    _cachedObjects.Add(value);
                }

                (value as MofObject)?.AddToExtent(_extent);

                return result;
            }
        }

        public void add(int index, object value)
        {
            lock (_cachedObjects)
            {
                _reflectiveSequence.add(index, value);
                _cachedObjects.Add(value);
            }
        }

        public object get(int index)
        {
            return _reflectiveSequence.get(index);
        }

        public bool addAll(IReflectiveSequence values)
        {
            lock (_cachedObjects)
            {
                var result = _reflectiveSequence.addAll(values);
                if (result)
                {
                    foreach (var value in values)
                    {
                        _cachedObjects.Add(value);
                    }
                }

                return result;
            }
        }

        public void clear()
        {
            lock (_cachedObjects)
            {
                _reflectiveSequence.clear();
                _cachedObjects.Clear();
            }
        }

        public void remove(int index)
        {
            lock (_cachedObjects)
            {
                var value = get(index);

                _reflectiveSequence.remove(index);

                _cachedObjects.Remove(value);
            }
        }

        public bool remove(object value)
        {
            lock (_cachedObjects)
            {
                var result = _reflectiveSequence.remove(value);
                if (result)
                {
                    _cachedObjects.Remove(value);
                }

                (value as MofObject)?.RemoveFromExtent(_extent);

                return result;
            }
        }

        public int size()
        {
            return _reflectiveSequence.size();
        }

        public object set(int index, object value)
        {
            lock (_cachedObjects)
            {
                var oldObject = _reflectiveSequence.set(index, value);

                _cachedObjects.Remove(oldObject);
                _cachedObjects.Add(value);

                (oldObject as MofObject)?.RemoveFromExtent(_extent);

                return oldObject;
            }
        }

        public bool HasObject(IObject value)
        {
            lock (_cachedObjects)
            {
                return _cachedObjects.Contains(value);
            }
        }

        /// <summary>Gibt einen Enumerator zurück, der die Auflistung durchläuft.</summary>
        /// <returns>Ein Enumerator, der zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        public IEnumerator<object> GetEnumerator()
        {
            return _reflectiveSequence.GetEnumerator();
        }

        /// <summary>Gibt einen Enumerator zurück, der eine Auflistung durchläuft.</summary>
        /// <returns>Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}