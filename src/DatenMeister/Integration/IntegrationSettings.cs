namespace DatenMeister.Integration
{
    public class IntegrationSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the complete MOF and UML integration shall
        /// be performed or if a slim integration without having the complete meta model is sufficient.
        /// The slim integration does not load the Xmi files
        /// </summary>
        public bool PerformSlimIntegration { get; set; }

        public string PathToXmiFiles { get; set; } // = "App_Data";

        /// <summary>
        /// Gets or sets a value indicating whether the data environment including all the metamodels shall be established
        /// </summary>
        public bool EstablishDataEnvironment { get; set; } = true;

        /// <summary>
        /// True, if the default extents like users, user types, user views and other automatically generated
        /// extents shall be created as empty
        /// </summary>
        public bool InitializeDefaultExtents { get; set; }

        /// <summary>
        /// Gets or sets the name of the database path in which the databases are stored per default
        /// </summary>
        public string DatabasePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the loading of an extent may fail without interrupting the complete initialization of the DatenMeister
        /// </summary>
        public bool AllowNoFailOfLoading { get; set; }

        public IntegrationSettings()
        {
            DatabasePath = GiveMe.DefaultDatabasePath;
        }
    }
}