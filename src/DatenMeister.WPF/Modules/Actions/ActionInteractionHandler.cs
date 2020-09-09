using System.Collections.Generic;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
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
            if (IsRelevant(element))
            {
                yield return new DefaultElementInteraction(
                    "Execute ActionSet",
                    () =>
                    {
                        MessageBox.Show(element.ToString());
                    });
            }
        }
    }
}