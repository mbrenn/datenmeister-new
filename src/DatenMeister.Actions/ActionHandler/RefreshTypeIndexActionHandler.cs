using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.TypeIndexAssembly;

namespace DatenMeister.Actions.ActionHandler;

/// <summary>
/// Defines the action handler that triggers a refresh of the type index.
/// </summary>
public class RefreshTypeIndexActionHandler : IActionHandler
{
    /// <summary>
    /// Checks if the given node is a RefreshTypeIndexAction.
    /// </summary>
    /// <param name="node">The node to be checked.</param>
    /// <returns>True, if the handler is responsible.</returns>
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__RefreshTypeIndexAction) == true;
    }

    /// <summary>
    /// Evaluates the action and triggers the refresh of the type index.
    /// </summary>
    /// <param name="actionLogic">The action logic to be used.</param>
    /// <param name="action">The action to be executed.</param>
    /// <returns>Null, as the result is not needed.</returns>
    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var waitForRefresh = action.getOrDefault<bool>(_Actions._RefreshTypeIndexAction.waitForRefresh);
        var typeIndexLogic = new TypeIndexLogic(actionLogic.WorkspaceLogic);
        await typeIndexLogic.TriggerUpdateOfIndex(waitForRefresh);
        return null;
    }
}