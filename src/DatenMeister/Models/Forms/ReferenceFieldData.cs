namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Allows the selection of a certain field by navigating through the extent
    /// </summary>
    public class ReferenceFieldData : FieldData
    {
        public const string FieldType = "reference";

        public ReferenceFieldData() : base(FieldType)
        {

        }

        public ReferenceFieldData(string name, string title) : base(FieldType, name, title)
        {

        }

        /// <summary>
        /// Gets or sets the value whether the selection shall be performed inline
        /// </summary>
        public bool isSelectionInline { get; set; }

        /// <summary>
        /// Gets or sets the default extent that shall be shown, when the user clicks on the selection field
        /// </summary>
        public string defaultExtentUri { get; set; }

        /// <summary>
        /// Gets or sets the default workspace that will be shown, when the user clicks on the selection field. This
        /// property is only evaluated in case of <see cref="defaultExtentUri">defaultExtentUri</see> is null or empty. 
        /// </summary>
        public string defaultWorkspace { get; set; }

        /// <summary>
        /// Gets or sets the value whether the workspace selection shall be shown
        /// </summary>
        public bool showWorkspaceSelection { get; set; }

        /// <summary>
        /// Gets or sets the value whether the extent selection shall be shown
        /// </summary>
        public bool showExtentSelection { get; set; }
    }
}