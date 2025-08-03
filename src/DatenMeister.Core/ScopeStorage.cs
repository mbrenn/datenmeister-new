using BurnSystems.Logging;
using DatenMeister.Core.Models;

namespace DatenMeister.Core;

/// <summary>
/// Defines the scope storage engine being used to retrieve static data which is available
/// during the complete life cycle
/// </summary>
public class ScopeStorage : IScopeStorage
{
    /// <summary>
    /// Stores the items and is also used to be thread safe
    /// </summary>
    private readonly Dictionary<Type, object> _storage = new();

    /// <summary>
    /// Defines the class logger
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(ScopeStorage));

    public IScopeStorage Add<T>(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        lock (_storage)
        {
            _storage[typeof(T)] = item;
#if DEBUG
            if (_storage.Count(x => x.Key.FullName == typeof(T).FullName) != 1)
                Logger.Error(
                    "Something really broke down. The Storage type was added twice with the same Fullname: " +
                    $"{typeof(T).FullName}");
#endif
        }

        return this;
    }

    public T Get<T>() where T : new()
    {
        return Get<T>(false);
    }

    public T Get<T>(bool traceMessageInCaseOfCreation) where T : new()
    {
        lock (_storage)
        {
            if (_storage.TryGetValue(typeof(T), out var result))
            {
                return (T)result;
            }

            if (traceMessageInCaseOfCreation)
                Logger.Trace($"Type of {typeof(T).FullName} not available, so we have to create an empty one.");

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

    /// <summary>
    /// Removes the instance from the scope storage
    /// </summary>
    /// <typeparam name="T">Type to be removed</typeparam>
    public void Remove<T>()
    {
        lock (_storage)
        {
            _storage.Remove(typeof(T));
        }
    }
}