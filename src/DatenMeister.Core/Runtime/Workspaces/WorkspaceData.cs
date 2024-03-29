﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Core.Runtime.Workspaces
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
        public List<Workspace> Workspaces { get; set; } = new();

        /// <summary>
        /// Gets or sets the default layer that shall be assumed, if no information is considered as available.
        /// </summary>
        public Workspace? Default
        {
            get => _default;
            set => _default = value;
        }

        public Workspace Data
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.WorkspaceData)
                       ?? throw new InvalidOperationException("Data Workspace is not found");
            }
        }

        public Workspace Types
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.WorkspaceTypes)
                       ?? throw new InvalidOperationException("Types Workspace is not found");
            }
        }

        public Workspace Uml
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.WorkspaceUml) ??
                       throw new InvalidOperationException("Uml Workspace is not found");
            }
        }

        public Workspace Mof
        {
            get
            {
                return Workspaces.FirstOrDefault(x => x.id == WorkspaceNames.WorkspaceMof) ??
                       throw new InvalidOperationException("Mof Workspace is not found");
            }
        }

        /// <summary>
        /// This event is called whenever a new workspace will be added
        /// </summary>
        public event EventHandler<WorkspaceEventArgs>? WorkspaceAdded;

        /// <summary>
        /// This event is called whenever a workspace is removed
        /// </summary>
        public event EventHandler<WorkspaceEventArgs>? WorkspaceRemoved;
        
        internal void OnWorkspaceAdded(IWorkspaceLogic workspaceLogic, string workspaceId)
        {
            WorkspaceAdded?.Invoke(workspaceLogic, new WorkspaceEventArgs
            {
                Id = workspaceId
            });
        }

        internal void OnWorkspaceRemoved(IWorkspaceLogic workspaceLogic, string workspaceId)
        {
            WorkspaceRemoved?.Invoke(workspaceLogic, new WorkspaceEventArgs
            {
                Id = workspaceId
            });
        }
    }

    /// <summary>
    /// Defines the event class for workspace related event
    /// </summary>
    public class WorkspaceEventArgs : EventArgs
    {
        public string Id { get; set; } = string.Empty;
    }
}