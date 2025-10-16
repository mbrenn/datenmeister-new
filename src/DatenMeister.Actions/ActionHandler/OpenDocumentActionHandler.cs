using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

public class OpenDocumentActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__DocumentOpenAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var filePath = action.getOrDefault<string>(_Actions._DocumentOpenAction.filePath);

            filePath = Environment.ExpandEnvironmentVariables(filePath);

            DotNetHelper.CreateProcess(filePath);
        });

        return null;
    }
}