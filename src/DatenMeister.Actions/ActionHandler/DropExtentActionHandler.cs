using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Actions.ActionHandler
{
    public class DropExtentActionHandler: IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__DropExtentAction) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(async () =>
            {
                var workspaceName = action.getOrDefault<string>(_DatenMeister._Actions._DropExtentAction.workspaceId) ??
                                    "Data";
                var extentUri = action.getOrDefault<string>(_DatenMeister._Actions._DropExtentAction.extentUri);

                if (string.IsNullOrEmpty(extentUri))
                {
                    throw new InvalidOperationException("extentUri is null or empty");
                }

                var workspace = actionLogic.WorkspaceLogic.GetWorkspace(workspaceName);
                if (workspace == null)
                {
                    throw new InvalidOperationException($"Workspace is not found {workspaceName}");
                }

                var extentManager = new ExtentManager(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
                await extentManager.RemoveExtent(workspaceName, extentUri);
            });

            return null;
        }
    }
}