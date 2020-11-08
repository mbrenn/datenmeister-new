using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    /// <summary>
    /// Defines the logging action handler
    /// </summary>
    public class ActionSetActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__ActionSet) == true;
        }

        /// <summary>
        /// Evaluates the plugin 
        /// </summary>
        /// <param name="actionLogic">Action plugin to be added</param>
        /// <param name="action">Action to be executed</param>
        public async void Evaluate(ActionLogic actionLogic, IElement action)
        {
            await actionLogic.ExecuteActionSet(action);
        }
    }
}