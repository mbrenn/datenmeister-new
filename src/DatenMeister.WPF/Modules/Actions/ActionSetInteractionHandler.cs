using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.Actions;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Actions
{
    public class ActionSetInteractionHandler : BaseElementInteractionHandler
    {
        private readonly ILogger ClassLogger = new ClassLogger(typeof(ActionSetInteractionHandler));

        /// <summary>
        /// Initializes a new instance of the ActionInteractionHandler
        /// </summary>
        public ActionSetInteractionHandler()
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
                        try
                        {
                            var result = await actionLogic.ExecuteActionSet(asElement);
                            MessageBox.Show($"{result.NumberOfActions:n0} action(s) were executed.");
                        }
                        catch (Exception exc)
                        {
                            ClassLogger.Error(exc.ToString());
                            MessageBox.Show(
                                $"An exception occured during the actionset execution: \r\n\r\n{exc.Message}");
                        }
                    });
            }
        }
    }
}