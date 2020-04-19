namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// This field data allows the selection of
    /// a file by the user via the well-known file selection dialogs
    /// </summary>
    public class FileSelectionFieldData: FieldData
    {
        public FileSelectionFieldData(): base()
        {
        }

        public FileSelectionFieldData(string name, string title)
            : base (name, title)
        {
            
        }

        /// <summary>
        /// Gets or sets the default extension
        /// </summary>
        public string? defaultExtension { get; set; }

        /// <summary>
        /// Gets or sets the information whether the user will be
        /// using a save dialog or an open dialog
        /// </summary>
        public bool isSaving { get; set; }
        
        /// <summary>
        /// Gets or sets the path to the directory
        /// </summary>
        public string? initialPathToDirectory { get; set; }
    }
}