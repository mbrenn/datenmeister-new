using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.DataLayer
{
    /// <summary>
    /// Stores the data which is persistent within the process
    /// </summary>
    public class WorkspaceData
    {
        /// <summary>
        /// Stores the workspaces
        /// </summary>
        public List<Workspace> Workspaces { get; set; } = new List<Workspace>();

        public WorkspaceData()
        {
        }

        /// <summary>
        /// Gets or sets the default layer that shall be assumed, if no information is considered as available.
        /// </summary>
        public Workspace Default { get; set; }
    }
}