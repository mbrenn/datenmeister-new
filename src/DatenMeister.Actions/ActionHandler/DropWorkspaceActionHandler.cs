using System;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler
{
    public class DropWorkspaceActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__DropWorkspaceAction) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                var workspace = action.getOrDefault<string>(_DatenMeister._Actions._DropWorkspaceAction.workspace);
                if (string.IsNullOrEmpty(workspace))
                {
                    throw new InvalidOperationException("workspace is not set");
                }

                actionLogic.WorkspaceLogic.RemoveWorkspace(workspace);
            });

            return null;
        }
    }
}