using System.Collections;
using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.Core.EMOF.Implementation;

public class TemporaryReflectiveCollection(IEnumerable<object?> values, bool isReadOnly) : IReflectiveCollection
{
    /// <summary>
    /// Defines the event arguments for deletion
    /// </summary>
    public class DeleteEventArgs : EventArgs
    {
        public object? DeleteObject { get; set; }
    }

    protected IEnumerable<object?> Values = values;

    /// <summary>
    /// Gets or sets a value whether the temporary collection is read-only and hinders adding new items
    /// </summary>
    public bool IsReadOnly { get; set; } = isReadOnly;

    /// <summary>
    /// Add or removes the events
    /// </summary>
    public event EventHandler<DeleteEventArgs>? OnDelete;

    public TemporaryReflectiveCollection() : this(new List<object?>(), false)
    {
    }

    public TemporaryReflectiveCollection(IEnumerable<object?> values) : this(values, false)
    {
    }

    /// <inheritdoc />
    public IEnumerator<object?> GetEnumerator()
        => Values.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => Values.GetEnumerator();

    /// <summary>
    /// Checks whether this reflective collection is read-only and throws an exception if yes
    /// </summary>
    private void CheckForReadOnly()
    {
        if (IsReadOnly)
            throw new InvalidOperationException("The temporary reflective collection is read-only");
    }

    /// <inheritdoc />
    public virtual bool add(object value)
    {
        CheckForReadOnly();
        (Values as IList<object>)?.Add(value);
        return true;
    }

    /// <inheritdoc />
    public virtual bool addAll(IReflectiveSequence value)
    {
        CheckForReadOnly();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public virtual void clear()
    {
        CheckForReadOnly();

        if (Values.GetType().IsArray)
        {
            Values = [];
        }
        else
        {
            var onDelete = OnDelete;
            if (onDelete != null)
            {
                foreach (var item in Values.Where(x=>x != null))
                {
                    onDelete.Invoke(this, new DeleteEventArgs{DeleteObject = item!});
                }
            }
                
            (Values as IList)?.Clear();
        }
    }

    /// <inheritdoc />
    public virtual bool remove(object? value)
    {
        if (value == null)
        {
            return false;
        }
            
        CheckForReadOnly();

        var onDelete = OnDelete;
        if (onDelete != null)
        {
            onDelete.Invoke(this, new DeleteEventArgs {DeleteObject = value});
            if (Values.Any(x => x?.Equals(value) == true))
            {
                (Values as IList)?.Remove(value);
                return true;
            }

            return false;
        }

        throw new InvalidOperationException("OnDelete event is not set, so deletion is not supported");
    }

    /// <inheritdoc />
    public virtual int size()
        => Values.Count();
}