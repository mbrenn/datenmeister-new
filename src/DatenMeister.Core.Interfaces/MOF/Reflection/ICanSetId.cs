namespace DatenMeister.Core.Interfaces.MOF.Reflection;

/// <summary>
/// Defines an interface when it has an id, which can be set
/// </summary>
public interface ICanSetId
{
    /// <summary>
    /// Sets the id of the object
    /// </summary>
    string Id { set; }
}