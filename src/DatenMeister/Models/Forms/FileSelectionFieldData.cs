namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// This field data allows the selection of
    /// a file by the user via the well-known file selection dialogs
    /// </summary>
    public class FileSelectionFieldData: FieldData
    {
        public FileSelectionFieldData()
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
        
        /// <summary>
        /// Gets or sets the filter for the save dialog
        /// See https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netcore-3.1#Microsoft_Win32_FileDialog_Filter
        /// </summary>
        public string? filter { get; set; }
    }
}