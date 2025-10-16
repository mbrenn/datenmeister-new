namespace DatenMeister.Core.Interfaces.MOF.Reflection;

/// <summary>
///     Implements the interface according to MOF Core Specificaton 2.5, clause 9.4
/// </summary>
public interface IObject
{
    bool equals(object? other);

    object? get(string property);

    void set(string property, object? value);

    bool isSet(string property);

    void unset(string property);
}