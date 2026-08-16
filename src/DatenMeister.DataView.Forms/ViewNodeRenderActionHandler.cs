using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.DataView.Forms;

public class ViewNodeRenderActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        throw new NotImplementedException();
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        // Creates the action result
        var actionResult = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionResult);
        actionResult.set(_Actions._ActionResult.isSuccess, true);
        
        // The alert result
        var result = InMemoryObject.CreateEmpty(_Actions.TheOne.ClientActions.__AlertClientAction);
        result.set(_Actions._ClientActions._AlertClientAction.messageText, "This is a message"); 
        result.set(_Actions._ClientActions._AlertClientAction.name, "Message Provider");
        
        // Now consolidating all the information
        actionResult.set(_Actions._ActionResult.clientActions, new[]{result});
        return actionResult;
    }
}