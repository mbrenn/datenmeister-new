using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class DropWorkspaceActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _Actions.TheOne.__DropWorkspaceAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspace = action.getOrDefault<string>(_Actions._DropWorkspaceAction.workspace);
            if (string.IsNullOrEmpty(workspace))
            {
                throw new InvalidOperationException("workspace is not set");
            }
            
            actionLogic.WorkspaceLogic.RemoveWorkspace(workspace);
        }
    }
}