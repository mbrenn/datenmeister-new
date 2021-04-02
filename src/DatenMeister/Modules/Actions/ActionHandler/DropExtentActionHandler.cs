using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class DropExtentActionHandler: IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__DropExtentAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspaceName = action.getOrDefault<string>(_DatenMeister._Actions._DropExtentAction.workspace) ?? "Data";
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
            extentManager.RemoveExtent(workspaceName, extentUri);
        }
    }
}