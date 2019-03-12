using System;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Models.ItemsAndExtents
{
    public class WorkspaceModel
    {
        private readonly Workspace _workspace;

        public WorkspaceModel(Workspace workspace)
        {
            _workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        }

        public string id => _workspace.id;

        public string annotation => _workspace.annotation;
    }
}