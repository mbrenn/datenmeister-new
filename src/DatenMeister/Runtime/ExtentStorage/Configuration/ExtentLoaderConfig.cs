namespace DatenMeister.Runtime.ExtentStorage.Configuration
{
    /// <summary>
    /// Defines the storage configuration, which allows the extent getting loaded and saved. 
    /// The configuration has to be maintained during the runtime for loading and storing
    /// </summary>
    public class ExtentLoaderConfig
    {
        /// <summary>
        /// Gets or sets the extent uri
        /// </summary>
        public string ExtentUri { get; set; }

        /// <summary>
        /// Gets ors sets the workspace in which the data will be loaded
        /// </summary>
        public string Workspace { get; set; }

        /// <summary>
        /// Gets or sets the datalayer being used for the allocation of the 
        /// extent
        /// </summary>
        public string DataLayer { get; set; }
    }
}