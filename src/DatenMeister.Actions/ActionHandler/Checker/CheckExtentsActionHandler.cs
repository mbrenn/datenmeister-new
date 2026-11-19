using DatenMeister.Actions.Model;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Actions.ActionHandler.Checker;

public class CheckExtentsActionHandler: IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Checker.__CheckExtentsAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        var masterUrl = action.getOrDefault<string>(_Actions._Checker._CheckExtentsAction.masterUrl);

        return null;
    }
}