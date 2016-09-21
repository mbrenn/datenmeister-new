using System.Collections.Generic;

namespace DatenMeister.Runtime.Workspaces.Data
{
    /// <summary>
    /// Stores the workspaces as they will be serialized as an Xml file from the WorkspaceLoader
    /// </summary>
    public class WorkspaceFileData
    {
        /// <summary>
        /// Stores the workspaces
        /// </summary>
        public List<WorkspaceInfo> Workspaces { get; set; } = new List<WorkspaceInfo>();
    }
}