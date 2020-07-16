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
        private Dictionary<Type, object> _storage = new Dictionary<Type, object>();

        public void Add<T>(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            lock (_storage)
            {
                _storage[typeof(T)] = item;
            }
        }

        public T Get<T>()
        {
            lock (_storage)
            {
                if (_storage.TryGetValue(typeof(T), out var result))
                {
                    return (T) result;
                }
                
                throw new InvalidOperationException($"Instance of {typeof(T)} is not found in storage");
            }
        }
    }
}