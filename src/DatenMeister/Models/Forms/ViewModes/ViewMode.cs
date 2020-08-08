namespace DatenMeister.Models.Forms.ViewModes
{
    /// <summary>
    /// Defines a view mode
    /// </summary>
    public class ViewMode
    {
        /// <summary>
        /// Gets or sets the name of the view node.
        /// </summary>
        public string? name { get; set; }
        
        /// <summary>
        /// Gets or sets the id of the viewmode
        /// </summary>
        public string? id { get; set; }
        
        /// <summary>
        /// Gets or sets the default extent type to which the view mode will be associated
        /// </summary>
        public string? defaultExtentType { get; set; }
    }
}