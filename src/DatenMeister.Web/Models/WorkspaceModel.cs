using DatenMeister.EMOF.Interface.Identifiers;
using System;

namespace DatenMeister.Web.Models
{
    public class WorkspaceModel
    {
        private Workspace<IExtent> _workspace;

        public string id
        {
            get { return _workspace.id; }
        }

        public string annotation
        {
            get { return _workspace.annotation; }
        }

        public WorkspaceModel(Workspace<IExtent> workspace)
        {
            if ( workspace == null )
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            _workspace = workspace;
        }
    }
}
