using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

/// <summary>
/// Defines the logging action handler
/// </summary>
public class ActionSetActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__ActionSet) == true;
    }

    /// <summary>
    /// Evaluates the plugin 
    /// </summary>
    /// <param name="actionLogic">Action plugin to be added</param>
    /// <param name="action">Action to be executed</param>
    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        return await actionLogic.ExecuteActionSet(action);
    }
}