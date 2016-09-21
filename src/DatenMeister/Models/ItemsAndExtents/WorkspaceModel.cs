using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Models.ItemsAndExtents
{
    public class WorkspaceModel
    {
        private readonly Workspace _workspace;

        public WorkspaceModel(Workspace workspace)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            _workspace = workspace;
        }

        public string id
        {
            get { return _workspace.id; }
        }

        public string annotation
        {
            get { return _workspace.annotation; }
        }
    }
}