using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class SimpleReportActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__SimpleReportAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspace = action.getOrDefault<string>(_DatenMeister._Actions._SimpleReportAction.workspaceId)
                            ?? WorkspaceNames.WorkspaceData;
            
            var path = action.getOrDefault<string>(_DatenMeister._Actions._SimpleReportAction.path);
            
            
        }
    }
}