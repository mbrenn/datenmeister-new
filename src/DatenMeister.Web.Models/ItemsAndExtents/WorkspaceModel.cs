using System;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Models.ItemsAndExtents
{
    public class WorkspaceModel
    {
        private readonly Workspace<IExtent> _workspace;

        public WorkspaceModel(Workspace<IExtent> workspace)
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