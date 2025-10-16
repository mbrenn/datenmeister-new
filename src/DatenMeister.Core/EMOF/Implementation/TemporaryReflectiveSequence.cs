using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.Core.EMOF.Implementation;

/// <summary>
/// Defines a reflective sequence that models an enumerable to a reflective sequence
/// </summary>
public class TemporaryReflectiveSequence : TemporaryReflectiveCollection, IReflectiveSequence
{
    public TemporaryReflectiveSequence()
    {
    }

    public TemporaryReflectiveSequence(IEnumerable<object?> values) : base(values)
    {
    }

    public void add(int index, object value)
    {
        throw new NotImplementedException();
    }

    public object? get(int index) => Values.ElementAt(index);

    public void remove(int index)
    {
        throw new NotImplementedException();
    }

    public object set(int index, object value) => throw new NotImplementedException();
}