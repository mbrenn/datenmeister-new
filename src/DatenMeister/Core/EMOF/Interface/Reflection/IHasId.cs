namespace DatenMeister.Core.EMOF.Interface.Reflection
{
    /// <summary>
    /// Defines an interface when it has an id, which can be used
    /// </summary>
    public interface IHasId
    {
        /// <summary>
        /// Gets the id of the object
        /// </summary>
        string? Id { get; }
    }
}