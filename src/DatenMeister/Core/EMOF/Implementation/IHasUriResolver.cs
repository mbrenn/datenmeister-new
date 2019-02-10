namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines the interface which allows the setting and getting of an IUriResolver
    /// </summary>
    public interface IHasUriResolver
    {
        /// <summary>
        /// Gets or sets the uri resolver. This instance can be used by the Provider to retrieve information about the
        /// metaclasses
        /// </summary>
        IUriResolver UriResolver { get; set; }
    }
}