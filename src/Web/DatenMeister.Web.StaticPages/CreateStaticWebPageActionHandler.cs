using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Web.StaticPages;

public class CreateStaticWebPageActionHandler : IActionHandler {
    public bool IsResponsible(IElement node)
    {
        throw new NotImplementedException();
    }

    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        throw new NotImplementedException();
    }
}