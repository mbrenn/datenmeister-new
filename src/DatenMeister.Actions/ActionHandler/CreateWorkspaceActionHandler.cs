using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler;

public class CreateWorkspaceActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__CreateWorkspaceAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var workspace = action.getOrDefault<string>(_Actions._CreateWorkspaceAction.workspaceId);
            var annotation = action.getOrDefault<string>(_Actions._CreateWorkspaceAction.annotation);
            if (string.IsNullOrEmpty(workspace))
            {
                throw new InvalidOperationException("workspace is not set");
            }

            actionLogic.WorkspaceLogic.AddWorkspace(
                new Workspace(workspace)
                {
                    annotation = annotation
                });
        });

        return null;
    }
}