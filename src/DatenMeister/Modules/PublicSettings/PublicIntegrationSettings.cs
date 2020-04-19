namespace DatenMeister.Modules.PublicSettings
{
    /// <summary>
    /// This class defines the integration settings being configurable for the user
    /// </summary>
    public class PublicIntegrationSettings
    {
        /// <summary>
        /// Gets or sets the title of the window
        /// </summary>
        public string? windowTitle { get; set; }
        
        /// <summary>
        /// Gets or sets the path to the database
        /// </summary>
        public string? databasePath { get; set; }
    }
}