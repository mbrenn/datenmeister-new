using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Actions;

public class ActionSetExecutionState
{
    private readonly IElement _element;

    public ActionSetExecutionState(IElement element)
    {
        _element = element;
    }
        
    /// <summary>
    /// Stores the number of actions
    /// </summary>
    private int _numberOfActions;

    /// <summary>
    /// Stores the number of actions being executed
    /// </summary>
    public int NumberOfActions => _numberOfActions;

    public void IncrementNumberOfActions()
    {
        Interlocked.Increment(ref _numberOfActions);
        _element.set("numberOfActions", _numberOfActions);
    }
}

public class ActionLogic
{
    /// <summary>
    /// Gets or sets a flag whether an asynchronous execution shall be performed
    /// that means whether the action itself shall be executed in a task. 
    /// </summary>
    private const bool AsyncExecution = true;

    private static readonly ILogger ClassLogger = new ClassLogger(typeof(ActionLogic));
        
    public IWorkspaceLogic WorkspaceLogic { get; }
    public IScopeStorage ScopeStorage { get; }

    public ActionLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        WorkspaceLogic = workspaceLogic;
        ScopeStorage = scopeStorage;
    }
        
    /// <summary>
    /// Executes the given action
    /// </summary>
    /// <param name="actionSet">Actions-Set to be executed</param>
    public async Task<IElement?> ExecuteActionSet(IElement actionSet)
    {
        var result = InMemoryObject.CreateEmpty();
            
        var actionSetExecutionState = new ActionSetExecutionState(result);
        var actions = actionSet.getOrDefault<IReflectiveCollection>(
            _DatenMeister._Actions._ActionSet.action);
        if (actions == null)
        {
            // Nothing to be executed
            return result;
        }
            
        foreach (var action in actions.OfType<IElement>())
        {
            var isDisabled = action.getOrDefault<bool>(_DatenMeister._Actions._Action.isDisabled);
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