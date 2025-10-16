// ReSharper disable InconsistentNaming

namespace DatenMeister.Core.Interfaces.MOF.Common;

public interface IReflectiveSequence : IReflectiveCollection
{
    void add(int index, object value);

    object? get(int index);

    void remove(int index);

    object? set(int index, object value);
}