using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.Actions;

/// <summary>
/// Stores some helper class to have a simpler usage of the action and action result model class
/// </summary>
public static class ActionModelHelper
{
    /// <summary>
    /// Creates an action result consisting of the clientelements according actionResultElements
    /// </summary>
    /// <param name="success">true, if it shall be returned as a success</param>
    /// <param name="actionResultElements">The elements being returned</param>
    /// <param name="factory">Factory to be used, if null, the internal TemporaryFactory is used.</param>
    /// <returns>The returned action result.</returns>
    public static IElement CreateActionResult(bool success, IElement [] actionResultElements, IFactory? factory = null)
    {
        factory ??= InMemoryObject.TemporaryFactory;
        var actionResult = factory.create(_Actions.TheOne.__ActionResult);
        actionResult.set(_Actions._ActionResult.isSuccess, success);
        
        // Now consolidating all the information
        actionResult.set(_Actions._ActionResult.clientActions, actionResultElements);
        return actionResult;
    }
}