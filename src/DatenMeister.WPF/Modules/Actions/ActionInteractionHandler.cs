using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.Actions;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Actions
{
    public class ActionInteractionHandler : BaseElementInteractionHandler
    {
        /// <summary>
        /// Initializes a new instance of the ActionInteractionHandler
        /// </summary>
        public ActionInteractionHandler()
        {
            OnlyElementsOfType = _Actions.TheOne.__ActionSet;
        }
        
        public override IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (IsRelevant(element) && element is IElement asElement)
            {
                yield return new DefaultElementInteraction(
                    "Execute ActionSet",
                    async () =>
                    {
                        var actionLogic = new ActionLogic(
                            GiveMe.Scope.WorkspaceLogic,
                            GiveMe.Scope.ScopeStorage);
                        await actionLogic.ExecuteActionSet(asElement);
                    });
            }
        }
    }
}