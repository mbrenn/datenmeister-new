namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// This interface can be implemented by all elements, which know there uri and
    /// where the extension method GetUri can directly access the given field without the
    /// need to lookup and query the extent. This interface is mainly used by the shadow objects
    /// which are somehow out of context
    /// </summary>
    public interface IKnowsUri
    {
        /// <summary>
        /// Gets the uri
        /// </summary>
        string Uri { get; }
    }
}