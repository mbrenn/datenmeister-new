using System.Collections.Generic;

namespace DatenMeister.Runtime.Workspaces.Data
{
    public class WorkspaceData
    {
        /// <summary>
        /// Stores the workspaces
        /// </summary>
        public List<WorkspaceInfo> Workspaces { get; set; } = new List<WorkspaceInfo>();
    }
}