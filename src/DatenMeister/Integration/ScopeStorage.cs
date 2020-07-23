using System;
using System.Collections.Generic;

namespace DatenMeister.Integration
{
    /// <summary>
    /// Defines the scope storage engine being used to retrieve static data which is available
    /// during the complete life cycle
    /// </summary>
    public class ScopeStorage : IScopeStorage
    {
        /// <summary>
        /// Stores the items and is also used to be thread safe
        /// </summary>
        private readonly Dictionary<Type, object> _storage = new Dictionary<Type, object>();

        public void Add<T>(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            lock (_storage)
            {
                _storage[typeof(T)] = item;
            }
        }

        public T Get<T>() where T : new()
        {
            lock (_storage)
            {
                if (_storage.TryGetValue(typeof(T), out var result))
                {
                    return (T) result;
                }

                var newResult = new T();
                Add(newResult);
                return newResult;
            }
        }

        /// <summary>
        /// Tries to get a value and returns null, if value is not found
        /// </summary>
        /// <typeparam name="T">Type to be retrieved</typeparam>
        /// <returns>The found storage item or null, if not found</returns>
        public T TryGet<T>()
        {
            lock (_storage)
            {
                if (_storage.TryGetValue(typeof(T), out var result))
                {
                    return (T) result;
                }
            }

            return default!;
        }
    }
}