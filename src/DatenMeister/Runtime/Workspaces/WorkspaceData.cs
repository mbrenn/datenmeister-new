using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Runtime.Workspaces
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

        /// <summary>
        /// Gets or sets the default layer that shall be assumed, if no information is considered as available.
        /// </summary>
        public Workspace Default { get; set; }


        public Workspace Data
        {
            get { return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameData); }
        }

        public Workspace Types
        {
            get { return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameTypes); }
        }

        public Workspace Uml
        {
            get { return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameUml); }
        }

        public Workspace Mof
        {
            get { return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameMof); }
        }


    }
}