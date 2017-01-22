using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Implements the uri resolver that is also capable to resolve through all extents of a workspace
    /// </summary>
    public class WorkspaceUriResolver : IUriResolver
    {
        private IWorkspaceLogic _workspaceLogic;

        public WorkspaceUriResolver(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <inheritdoc />
        public IElement Resolve(string uri)
        {
            foreach (var workspace in _workspaceLogic.Workspaces)
            {
                foreach (var extent in workspace.extent)
                {
                    var found = (extent as IUriExtent)?.element(uri);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            throw new InvalidOperationException($"The given element with uri {uri} is not found within all workspaces");
        }
    }
}