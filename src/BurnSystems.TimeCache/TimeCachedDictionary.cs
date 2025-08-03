using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BurnSystems.TimeCache;

/// <summary>
/// This is a dictionary, which just holds the entries for a certain amount
/// of time. By holding the data for just a moment in time, it can be used
/// to cache data in which the only cache-invalidating trigger is time.
/// It is not strongly recommended to use that class for caching since an explicit
/// cache mechanismn provides a higher data integrty and performance. 
/// </summary>
/// <typeparam name="TK">Type of the key</typeparam>
/// <typeparam name="TV">Type of the value</typeparam>
public class TimeCachedDictionary<TK, TV> : IDictionary<TK, TV> where TK : notnull
{
    /// <summary>
    /// Stores the caching time for the dictionary
    /// </summary>
    public TimeSpan CachingTime { get; set; } = TimeSpan.FromSeconds(1.0);
    
    /// <summary>
    /// Stores the last cache reset
    /// </summary>
    private DateTime _lastCacheReset = DateTime.MinValue;
    
    /// <summary>
    /// Stores the dictionary being used to store the data
    /// </summary>
    private readonly Dictionary<TK, TV> _dictionary = new();

    /// <summary>
    /// Checks the cache and deletes the cache in case the last reset has occured
    /// before Caching time
    /// </summary>
    private void CheckCache()
    {
        lock (_dictionary)
        {
            if (_lastCacheReset.Add(CachingTime) < DateTime.Now)
            {
                _dictionary.Clear();
                _lastCacheReset = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// Invalidates the cache by clearing all data
    /// </summary>
    public void InvalidateCache()
    {
        lock (_dictionary)
        {
            _dictionary.Clear();
            _lastCacheReset = DateTime.MinValue;       
        }
    }

    public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        CheckCache();
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<TK, TV> item)
    {
        CheckCache();
        (_dictionary as IDictionary<TK,TV>).Add(item);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TK, TV> item)
    {
        CheckCache();
        return _dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
    {
        CheckCache();
        (_dictionary as IDictionary<TK,TV>).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TK, TV> item)
    {
        CheckCache();
        return (_dictionary as IDictionary<TK, TV>).Remove(item);
    }

    public int Count
    {
        get
        {
            CheckCache();
            return _dictionary.Count;
        }
    }

    public bool IsReadOnly => (_dictionary as IDictionary<TK,TV>).IsReadOnly;

    public void Add(TK key, TV value)
    {
        CheckCache();
        _dictionary.Add(key, value);
    }

    public bool ContainsKey(TK key)
    {
        CheckCache();
        return _dictionary.ContainsKey(key);
    }

    public bool Remove(TK key)
    {
        CheckCache();
        return _dictionary.Remove(key);
    }

    public bool TryGetValue(TK key, [MaybeNullWhen(false)] out TV value)
    {
        CheckCache();
        return  _dictionary.TryGetValue(key, out value);
    }

    public TV this[TK key]
    {
        get
        {
            CheckCache();
            return _dictionary[key];
        }
        set
        {
            CheckCache();
            _dictionary[key] = value;
        }
    }

    public ICollection<TK> Keys
    {
        get
        {
            CheckCache();
            return _dictionary.Keys;
        }
    }

    public ICollection<TV> Values
    {
        get
        {
            CheckCache();
            return _dictionary.Values;
        }
    }
}