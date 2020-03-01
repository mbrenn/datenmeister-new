using DatenMeister.Runtime.Workspaces;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Runtime.ExtentStorage.Configuration
{
    /// <summary>
    /// Defines the storage configuration, which allows the extent getting loaded and saved.
    /// The configuration has to be maintained during the runtime for loading and storing
    /// </summary>
    public class ExtentLoaderConfig
    {
        public ExtentLoaderConfig(string extentUri)
        {
            this.extentUri = extentUri;
        }

        /// <summary>
        /// Gets or sets the extent uri
        /// </summary>
        public string extentUri { get; set; }

        /// <summary>
        /// Gets ors sets the workspace in which the data will be loaded
        /// </summary>
        public string workspaceId { get; set; } = WorkspaceNames.NameData;
    }
}