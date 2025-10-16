using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

public class ConsoleWriteActionHandler: IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.OSIntegration.__ConsoleWriteAction) == true;
    }

    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var wrapper = new DatenMeister.Core.Models.Actions.OSIntegration.ConsoleWriteAction_Wrapper(action);
        Console.WriteLine(wrapper.text);
        return Task.FromResult<IElement?>(null);
    }
}