using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Integration.DotNet;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Actions;

public class ActionInteractionHandler : BaseElementInteractionHandler
{
    /// <summary>
    /// Initializes a new instance of the ActionInteractionHandler
    /// </summary>
    public ActionInteractionHandler()
    {
        OnlyElementsOfType = _Actions.TheOne.__Action;
    }
        
    public override IEnumerable<IElementInteraction> GetInteractions(IObject element)
    {
        if (IsRelevant(element)
            && element is IElement asElement
            && asElement.getMetaClass()?.equals(_Actions.TheOne.__ActionSet) != true)
        {
            yield return new DefaultElementInteraction(
                "Execute Action",
                async () =>
                {
                    var actionLogic = new ActionLogic(
                        GiveMe.Scope.WorkspaceLogic,
                        GiveMe.Scope.ScopeStorage);
                    try
                    {
                        await actionLogic.ExecuteAction(asElement);
                        MessageBox.Show("Action was executed.");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show($"An exception occured during the action execution: \r\n{exc}");
                    }
                });
        }
    }
}