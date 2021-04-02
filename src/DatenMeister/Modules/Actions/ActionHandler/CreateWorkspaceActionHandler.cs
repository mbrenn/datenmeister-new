using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class CreateWorkspaceActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__CreateWorkspaceAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspace = action.getOrDefault<string>(_DatenMeister._Actions._CreateWorkspaceAction.workspace);
            var annotation = action.getOrDefault<string>(_DatenMeister._Actions._CreateWorkspaceAction.annotation);
            if (string.IsNullOrEmpty(workspace))
            {
                throw new InvalidOperationException("workspace is not set");
            }
            
            actionLogic.WorkspaceLogic.AddWorkspace(
                new Workspace(workspace)
                {
                    annotation = annotation
                });
        }
    }
}