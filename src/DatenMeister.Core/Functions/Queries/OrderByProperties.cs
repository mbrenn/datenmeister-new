using System.Collections;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

namespace DatenMeister.Core.Functions.Queries;

public class OrderByProperties : IReflectiveCollection, IHasExtent
{
    private readonly List<string> _orderByProperty;
    private readonly IReflectiveCollection _parent;

    public OrderByProperties(IReflectiveCollection parent, IEnumerable<string> properties)
    {
        if (properties == null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        _orderByProperty = properties.ToList();
    }

    public bool add(object value)
        => _parent.add(value);

    public bool addAll(IReflectiveSequence value) =>
        _parent.addAll(value);

    public void clear()
    {
        _parent.clear();
    }

    public IEnumerator<object> GetEnumerator()
    {
        // If there is no ordering
        if (_orderByProperty.Count == 0)
        {
            foreach (var item in _parent)
            {
                if (item != null)
                {
                    yield return item;
                }
            }

            yield break;
        }

        // Build up the Query
        var firstColumn = _orderByProperty[0];
        if (firstColumn.StartsWith("!"))
        {
            firstColumn = firstColumn.Substring(1);
        }
            
        var current =
            firstColumn.StartsWith("!")
                ? _parent
                    .OrderByDescending(x => SelectObject(x, firstColumn))
                : _parent
                    .OrderBy(x => SelectObject(x, firstColumn));

        for (var n = 1; n < _orderByProperty.Count; n++)
        {
            var currentColumn = _orderByProperty[n];
            if (currentColumn.StartsWith("!"))
            {
                currentColumn = currentColumn.Substring(1);
                current = current
                    .ThenByDescending(x => SelectObject(x, currentColumn));
            }
            else
            {
                current = current
                    .ThenBy(x => SelectObject(x, currentColumn));
            }
        }
            
        foreach (var item in current)
        {
            if (item == null) continue;
                
            yield return item;
        }

        object? SelectObject(object? x, string column)
        {
            if (x is IObject asObject)
            {
                var value = asObject.getOrDefault<object>(column);
                if (value is int || value is double)
                {
                    return value;
                }

                return value?.ToString();
            }

            return null;
        }
    }

    public bool remove(object? value) => _parent.remove(value);

    public int size() =>
        _parent.size();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    /// <summary>
    /// Gets the extent associated to the parent extent
    /// </summary>
    public IExtent? Extent =>
        (_parent as IHasExtent)?.Extent;
}