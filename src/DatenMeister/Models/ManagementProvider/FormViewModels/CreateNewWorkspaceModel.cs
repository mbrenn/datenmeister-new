namespace DatenMeister.Models.ManagementProvider.FormViewModels
{
    /// <summary>
    /// Defines the model which creates a new workspace
    /// </summary>
    public class CreateNewWorkspaceModel
    {
        /// <summary>
        /// Gets or sets the id of the workspace
        /// </summary>
        public string? id { get; set; }

        /// <summary>
        /// Gets or sets the annotation of the workspace
        /// </summary>
        public string? annotation { get; set; }
    }
}