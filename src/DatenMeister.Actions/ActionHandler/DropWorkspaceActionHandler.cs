using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

public class DropWorkspaceActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__DropWorkspaceAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var workspace = action.getOrDefault<string>(_Actions._DropWorkspaceAction.workspaceId);
            if (string.IsNullOrEmpty(workspace))
            {
                throw new InvalidOperationException("workspace is not set");
            }

            actionLogic.WorkspaceLogic.RemoveWorkspace(workspace);
        });

        return null;
    }
}