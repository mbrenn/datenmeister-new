using System.Collections.Generic;

namespace DatenMeister.BootStrap.PublicSettings
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

        /// <summary>
        /// Gets or sets the path of the file being loaded.
        /// This information is not retrieved from the content. This information is directly written
        /// by the PublicSettingsHandler
        /// </summary>
        public string settingsFilePath { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets a flag indicating whether the complete DatenMeister shall run in Read-Only mode. 
        /// </summary>
        public bool isReadOnly { get; set; }
        
        /// <summary>
        /// Defines the standard location of the logging
        /// </summary>
        public LogLocation logLocation { get; set; }
        
        /// <summary>
        /// Defines an enumeration of environmental variables which are added at startup.
        /// </summary>
        public List<PublicEnvironmentVariable> environmentVariable { get; } = new List<PublicEnvironmentVariable>();
    }

    public class PublicEnvironmentVariable
    {
        public string? key { get; set; }
        public string? value { get; set; }
    }

    /// <summary>
    /// Defines the file locations for the logging
    /// </summary>
    public enum LogLocation
    {
        /// <summary>
        /// Storage in application folder
        /// </summary>
        Application, 
        
        /// <summary>
        /// Storage in local appdata
        /// </summary>
        LocalAppData,
        
        /// <summary>
        /// Storage in desktop
        /// </summary>
        Desktop,
        
        /// <summary>
        /// No use
        /// </summary>
        None
    }
}