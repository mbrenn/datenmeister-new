namespace DatenMeister.Web.PostModels
{
    /// <summary>
    /// Used to create a new extent
    /// </summary>
    public class ExtentAddModel
    {
        /// <summary>
        /// Gets or sets the type of the 
        /// extent to be created.
        /// It can be 'xmi' or 'csv'
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Gets or sets the workspace, where extent will be created
        /// </summary>
        public string workspace { get; set; }

        /// <summary>
        /// Gets or sets the contexturi of the new extent
        /// </summary>
        public string contextUri { get; set; }

        /// <summary>
        /// Gets or sets the filename
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// Gets or sets the name of the extent that shall be created
        /// </summary>
        public string name { get; set; }
    }
}