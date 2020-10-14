using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class ClearCollectionActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__ClearCollectionAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspaceId = action.getOrDefault<string>(_DatenMeister._Actions._ClearCollectionAction.workspace)
                              ?? WorkspaceNames.WorkspaceData;
            var path = action.getOrDefault<string>(_DatenMeister._Actions._ClearCollectionAction.path);

            var workspace = actionLogic.WorkspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                var message = $"workspace is not found ${workspaceId}";
                throw new InvalidOperationException(message);
            }

            var sourceElement = workspace.Resolve(path, ResolveType.NoMetaWorkspaces);
            if (sourceElement == null)
            {
                var message = $"path is not found ${path}";
                throw new InvalidOperationException(message);
            }

            if (sourceElement is IReflectiveCollection collection)
            {
                collection.clear();
            }
            else if (sourceElement is IExtent extent)
            {
                extent.elements().clear();
            }
            else
            {
                throw new InvalidOperationException("Unknown datatype");
            }

        }
    }
}