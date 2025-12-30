using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Actions;

/// <summary>
/// Defines the execution state for a set of actions.
/// </summary>
/// <param name="element">The element that stores the execution state.</param>
public class ActionSetExecutionState(IElement element)
{
    /// <summary>
    /// Stores the number of actions.
    /// </summary>
    private int _numberOfActions;

    /// <summary>
    /// Gets the number of actions being executed.
    /// </summary>
    public int NumberOfActions => _numberOfActions;

    /// <summary>
    /// Increments the number of actions and updates the storage element.
    /// </summary>
    public void IncrementNumberOfActions()
    {
        Interlocked.Increment(ref _numberOfActions);
        element.set("numberOfActions", _numberOfActions);
    }
}

/// <summary>
/// Contains the logic to execute actions and action sets.
/// </summary>
/// <param name="workspaceLogic">The workspace logic to be used.</param>
/// <param name="scopeStorage">The scope storage to be used.</param>
public class ActionLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    /// <summary>
    /// Gets or sets a flag whether an asynchronous execution shall be performed
    /// that means whether the action itself shall be executed in a task. 
    /// </summary>
    private const bool AsyncExecution = false;

    /// <summary>
    /// The logger for this class.
    /// </summary>
    private static readonly ILogger ClassLogger = new ClassLogger(typeof(ActionLogic));
        
    /// <summary>
    /// Gets the workspace logic.
    /// </summary>
    public IWorkspaceLogic WorkspaceLogic { get; } = workspaceLogic;

    /// <summary>
    /// Gets the scope storage.
    /// </summary>
    public IScopeStorage ScopeStorage { get; } = scopeStorage;

    /// <summary>
    /// Executes the given action set.
    /// </summary>
    /// <param name="actionSet">Action-Set to be executed.</param>
    /// <returns>The result of the action set execution.</returns>
    public async Task<IElement?> ExecuteActionSet(IElement actionSet)
    {
        var result = InMemoryObject.CreateEmpty();
            
        var actionSetExecutionState = new ActionSetExecutionState(result);
        var actions = actionSet.getOrDefault<IReflectiveCollection>(
            _Actions._ActionSet.action);
        if (actions == null)
        {
            // Nothing to be executed
            return result;
        }
            
        foreach (var action in actions.OfType<IElement>())
        {
            var isDisabled = action.getOrDefault<bool>(_Actions._Action.isDisabled);
            if (isDisabled)
            {
                continue;
            }
                
            await ExecuteAction(action);
            actionSetExecutionState.IncrementNumberOfActions();
        }

        return result;
    }

    /// <summary>
    /// Executes a certain action 
    /// </summary>
    /// <param name="action">Action to be executed</param>
    /// <returns>The element which indicates the result of an action. It may be null, if there
    /// is no result</returns>
    public async Task<IElement?> ExecuteAction(IElement action)
    {
        IElement? result = null;
        var found = false;
        foreach (var actionHandler in ScopeStorage.Get<ActionLogicState>().ActionHandlers)
        {
            if (!actionHandler.IsResponsible(action))
            {
                continue;
            }
                
            // Defines the action to be executed
            var fct = new Func<Task<IElement?>>(async () =>
            {
                try
                {
                    return await actionHandler.Evaluate(this, action);
                }
                catch (Exception exc)
                {
                    var message =
                        $"An exception occurred during execution of {action}:\r\n\r\n{exc.Message}";
                    ClassLogger.Error(exc.ToString());
                    throw new InvalidOperationException(message, exc);
                }
            });

            // ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162
            if (AsyncExecution)
            {
                result = await Task.Run(() => fct());
            }
            else
            {
                result = await fct();
            }
#pragma warning restore CS0162
            // ReSharper restore HeuristicUnreachableCode

            found = true;
        }

        if (!found)
        {
            var metaClass = action.metaclass;
            var metaClassName = metaClass == null ? "Unknown Type" : NamedElementMethods.GetFullName(metaClass);
            var message = $"Did not found action handler for {action}: {metaClassName}";
                
            ClassLogger.Warn(message);
            throw new InvalidOperationException(message);
        }

        return result;
    }
}