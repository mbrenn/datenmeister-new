using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler;

public class ClearCollectionActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__ClearCollectionAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var workspaceId = action.getOrDefault<string>(_Actions._ClearCollectionAction.workspaceId)
                              ?? WorkspaceNames.WorkspaceData;
            var path = action.getOrDefault<string>(_Actions._ClearCollectionAction.path);

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
        });

        return null;
    }
}