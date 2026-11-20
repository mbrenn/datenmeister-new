using DatenMeister.Actions.Model;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler.Checker;

public class CheckExtentsActionHandler() : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Checker.__CheckExtentsAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        var workspaceLogic = actionLogic.WorkspaceLogic;
        var masterWorkspace = action.getOrDefault<string?>(_Actions._Checker._CheckExtentsAction.masterWorkspace)
                              ?? WorkspaceNames.WorkspaceData;
        var masterUrl = action.getOrDefault<string>(_Actions._Checker._CheckExtentsAction.masterUrl);
        var derivedWorkspace = action.getOrDefault<string?>(_Actions._Checker._CheckExtentsAction.derivedWorkspace)
                               ?? WorkspaceNames.WorkspaceData;
        var derivedUrl = action.getOrDefault<string>(_Actions._Checker._CheckExtentsAction.derivedUrl);
        var reportWorkspace = action.getOrDefault<string?>(_Actions._Checker._CheckExtentsAction.reportWorkspace)
                              ?? WorkspaceNames.WorkspaceData;
        var reportUrl = action.getOrDefault<string>(_Actions._Checker._CheckExtentsAction.reportUrl);
        var checkers = action.getOrDefault<IReflectiveCollection>(_Actions._Checker._CheckExtentsAction.checkers);

        var masterCollection = workspaceLogic.FindObjectOrCollection(masterWorkspace, masterUrl) as IReflectiveCollection;
        var derivedCollection = workspaceLogic.FindObjectOrCollection(derivedWorkspace, derivedUrl) as IReflectiveCollection;
        var reportCollection = workspaceLogic.FindObjectOrCollection(reportWorkspace, reportUrl) as IReflectiveCollection;

        foreach (var checker in checkers)
        {
            
        }

        return null;
    }
}