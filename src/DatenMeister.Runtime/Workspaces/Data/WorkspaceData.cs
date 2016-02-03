using System.Collections.Generic;

namespace DatenMeister.Runtime.Workspaces.Data
{
    /// <summary>
    /// Stores the workspaces as they will be stored within the Xml file from the WorkspaceLoader
    /// </summary>
    public class WorkspaceData
    {
        /// <summary>
        /// Stores the workspaces
        /// </summary>
        public List<WorkspaceInfo> Workspaces { get; set; } = new List<WorkspaceInfo>();
    }
}