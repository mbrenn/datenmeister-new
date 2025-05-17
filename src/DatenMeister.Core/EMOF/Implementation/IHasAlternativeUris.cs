namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines the interface that is supported by extents which are not just called
    /// by a single uri. 
    /// </summary>
    public interface IHasAlternativeUris
    {
        /// <summary>
        /// Gets a list of alternative uris for a certain extent. 
        /// </summary>
        IList<string> AlternativeUris { get; }
    }
}