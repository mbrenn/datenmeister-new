namespace DatenMeister.Web.Json
{
    /// <summary>
    /// Defines the link to the item by defining the workspace and the
    /// Url to the item itself. Unfortunately, the item link does not specify the
    /// workspace in which the item is belonging to.
    /// </summary>
    public record ItemLink
    {
        /// <summary>
        /// Gets or 
        /// </summary>
        public string Workspace { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the url to the item 
        /// </summary>
        public string ItemUrl { get; set; } = string.Empty;
    }
}