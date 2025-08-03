using System.Collections;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.Runtime.Proxies;

/// <summary>
/// Abstracts a reflective collection/sequence into a list
/// </summary>
/// <typeparam name="T">Type of the elements of the list</typeparam>
public class ReflectiveList<T> : IList<T> where T : notnull
{
    private readonly IReflectiveCollection _collection;

    private readonly IReflectiveSequence? _sequence;

    /// <summary>
    /// Stores the function that is used to convert an object to the given type
    /// </summary>
    private readonly Func<object?, T> _wrapFunc;

    /// <summary>
    /// Converts the function that is used to unwrap the function
    /// </summary>
    private readonly Func<T, object?> _unwrapFunc;


    public ReflectiveList(IReflectiveCollection collection)
    {
        _collection = collection;
        _sequence = _collection as IReflectiveSequence;

        _wrapFunc = x => ((T) x!);
        _unwrapFunc = x => x;
    }

    public ReflectiveList(IReflectiveCollection collection, Func<object?, T> wrapFunc, Func<T, object?> unwrapFunc)
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
        var unwrap = _unwrapFunc(item);
        if (unwrap == null) return;
            
        _collection.add(unwrap);
    }

    public void Clear()
    {
        _collection.clear();
    }

    public bool Contains(T item)
    {
        var unwrapped = _unwrapFunc(item);
        return _collection.Any(x => x != null && x.Equals(unwrapped));
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (_sequence != null)
        {
            foreach (var value in array)
            {
                var unWrappedValue = _unwrapFunc(value);
                if (unWrappedValue == null) continue;
                _sequence.add(arrayIndex, unWrappedValue);
                arrayIndex++;
            }
        }
        else
        {
            foreach (var value in array)
            {
                var unWrappedValue = _unwrapFunc(value);
                if (unWrappedValue == null) continue;
                    
                _collection.add(unWrappedValue);
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
        if (unwrapped == null) return -1;
        foreach (var value in _collection)
        {
            if (value != null && value.Equals(unwrapped))
            {
                return n;
            }

            n++;
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        var unWrappedValue = _unwrapFunc(item);
        if (unWrappedValue == null) return;
            
        if (_sequence != null)
        {

            _sequence.add(index, unWrappedValue);
        }
        else
        {
            _collection.add(unWrappedValue);
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
                
            var unwrapped = _unwrapFunc(value);
            if (unwrapped == null) return;
                
            if (_sequence != null)
            {
                _sequence.set(index, unwrapped);
            }
            else
            {
                throw new NotImplementedException("Collection is not given");
            }
        }
    }
}