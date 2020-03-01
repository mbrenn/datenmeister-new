using System;
using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Stores the data which is persistent within the process
    /// </summary>
    public class WorkspaceData
    {
        private Workspace? _default;

        /// <summary>
        /// Stores the workspaces
        /// </summary>
        public List<Workspace> Workspaces { get; set; } = new List<Workspace>();

        /// <summary>
        /// Gets or sets the default layer that shall be assumed, if no information is considered as available.
        /// </summary>
        public Workspace Default
        {
            get => _default ?? throw new InvalidOperationException("Default is not set");
            set => _default = value;
        }


        public Workspace Data
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameData)
                       ?? throw new InvalidOperationException("Data Workspace is not found");
            }
        }

        public Workspace Types
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameTypes)
                       ?? throw new InvalidOperationException("Types Workspace is not found");
            }
        }

        public Workspace Uml
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameUml) ??
                       throw new InvalidOperationException("Uml Workspace is not found");
            }
        }

        public Workspace Mof
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.NameMof) ??
                       throw new InvalidOperationException("Mof Workspace is not found");
            }
        }
    }
}