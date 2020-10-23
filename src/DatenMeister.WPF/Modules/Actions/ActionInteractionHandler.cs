using System.Collections.Generic;
using System.Windows.Forms;
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
            OnlyElementsOfType = _DatenMeister.TheOne.Actions.__ActionSet;
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
                        var result = await actionLogic.ExecuteActionSet(asElement);
                        MessageBox.Show($"{result.NumberOfActions:n0} action(s) were executed.");
                    });
            }
        }
    }
}