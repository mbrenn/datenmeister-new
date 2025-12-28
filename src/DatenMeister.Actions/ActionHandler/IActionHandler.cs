using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Actions.ActionHandler;

/// <summary>
/// Defines the interface for an action handler.
/// </summary>
public interface IActionHandler
{
    /// <summary>
    /// Checks whether this action handler is responsible for the given action node.
    /// </summary>
    /// <param name="node">The action node to be evaluated.</param>
    /// <returns>True, if the action handler is responsible.</returns>
    public bool IsResponsible(IElement node);

    /// <summary>
    /// Evaluates the given action.
    /// </summary>
    /// <param name="actionLogic">The action logic context.</param>
    /// <param name="action">The action to be handled.</param>
    /// <returns>The result of the action evaluation.</returns>
    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action);
}