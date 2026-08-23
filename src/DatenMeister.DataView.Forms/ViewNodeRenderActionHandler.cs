using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.DataView.Forms.Model;

namespace DatenMeister.DataView.Forms;

public class ViewNodeRenderActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Root.TheOne.__ViewNodeRenderAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        if (actionVerb != "render")
        {
            // The alert result
            var result = Core.Models.Actions.ClientActions.AlertClientAction_Wrapper.Create(InMemoryObject.TemporaryFactory);
            result.messageText = "ActionHandler 'ViewNodeRenderActionHandler' just supports rendering";
            result.name = "Information";
        
            return ActionModelHelper.CreateActionResult(false, [result.GetWrappedElement()]);
        }
        else
        {
            // Creates the action result
            var actionResult = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionResult);
            actionResult.set(_Actions._ActionResult.isSuccess, true);

            // The alert result
            var result = InMemoryObject.CreateEmpty(_Actions.TheOne.ClientActions.__AlertClientAction);
            result.set(_Actions._ClientActions._AlertClientAction.messageText, "We are good to go");
            result.set(_Actions._ClientActions._AlertClientAction.name, "Message Provider");

            // Now consolidating all the information
            actionResult.set(_Actions._ActionResult.clientActions, new[] { result });
            return actionResult;
        }
    }
}