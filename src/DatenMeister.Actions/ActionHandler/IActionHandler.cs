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
    /// <param name="actionVerb">The verb under which the action shall be executed. It is null, if no verb is specified.</param>
    /// <returns>The result of the action evaluation.</returns>
    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb);
}

/// <summary>
/// Some action handler support multiple verbs.
/// To allow the automatic offering of these to the enduser, these can be listed. 
/// </summary>
public interface IActionHandlerWithVerbs : IActionHandler
{
    /// <summary>
    /// Gets a list of verbs which is supported by that action handler
    /// </summary>
    public string[] SupportedVerbs { get; }
}


