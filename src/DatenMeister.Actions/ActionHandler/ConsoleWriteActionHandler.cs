using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

public class ConsoleWriteActionHandler: IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__ConsoleWriteAction) == true;
    }

    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var wrapper = new DatenMeister.Core.Models.Actions.ConsoleWriteAction_Wrapper(action);
        Console.WriteLine(wrapper.text);
        return Task.FromResult<IElement?>(null);
    }
}