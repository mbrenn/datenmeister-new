// ReSharper disable InconsistentNaming
namespace DatenMeister.Runtime.Workspaces.Data
{
    /// <summary>
    /// Stores the information for a workspace which can be stored as an xml file
    /// </summary>
    public class WorkspaceInfo
    {
        public WorkspaceInfo(string id, string annotation)
        {
            this.id = id;
            this.annotation = annotation;
        }

        public string id { get; set; }
        public string annotation { get; set; }
    }
}