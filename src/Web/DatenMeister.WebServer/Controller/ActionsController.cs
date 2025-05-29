using System.Diagnostics;
using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.Web.Json;
using DatenMeister.WebServer.Library.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller;

[Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
public class ActionsController : ControllerBase
{
    private readonly IScopeStorage _scopeStorage;
    private readonly IWorkspaceLogic _workspaceLogic;

    public ActionsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
    }
        
    [HttpPost("api/action/execute_directly/{actionName}")]
    public async Task<ActionResult<ExecuteActionResult>> ExecuteAction(string actionName, [FromBody] ActionParams actionParams)
    {
        var success = true;
        if (actionParams.Parameter == null)
        {
            throw new InvalidOperationException("Parameter are not set");
        }

        var mofParameter = 
            new DirectJsonDeconverter(_workspaceLogic, _scopeStorage)
                .ConvertToObject(actionParams.Parameter) as IElement
            ?? throw new InvalidOperationException("Conversion was not successful");

        switch (actionName)
        {
            case "Workspace.Extent.Xmi.Create":
                if (Debugger.IsAttached)
                {
                    // Figure out when this method is called. 
                    // It needs to be transfered by using the Execute
                    Debugger.Break();
                }

                if (mofParameter.metaclass?.@equals(_DatenMeister.TheOne.ExtentLoaderConfigs
                        .__XmiStorageLoaderConfig) != true)
                {
                    throw new InvalidOperationException("Wrong metaclass. Expected XmiStorageLoaderConfig");
                }

                var workspaceLogic = GiveMe.Scope.WorkspaceLogic;
                var extentManager = new ExtentManager(workspaceLogic, GiveMe.Scope.ScopeStorage);
                var result = await extentManager.LoadExtent(
                    mofParameter,
                    ExtentCreationFlags.LoadOrCreate);
                success =
                    result.LoadingState is ExtentLoadingState.Loaded or ExtentLoadingState.LoadedReadOnly;

                if (!success) return new ExecuteActionResult(false, result.FailLoadingMessage, result.FailLoadingMessage);
                break;

            case "Execute":
                var actionLogic = new ActionLogic(GiveMe.Scope.WorkspaceLogic, GiveMe.Scope.ScopeStorage);
                try
                {
                    var resultOfAction = await actionLogic.ExecuteAction(
                        mofParameter
                    );
                        
                    var resultText = string.Empty;
                    if (resultOfAction != null)
                    {
                        resultText = MofJsonConverter.ConvertToJsonWithDefaultParameter(resultOfAction);
                    }

                    return new ExecuteActionResult(true, string.Empty, string.Empty, resultText);


                }
                catch (Exception exc)
                {
                    return new ExecuteActionResult(false, exc.Message, exc.ToString());
                }
        }

        return new ExecuteActionResult(success, "ActionNotFound", "");
    }

    [HttpPost("api/action/execute/{workspaceId}/{itemUri}")]
    public async Task<ActionResult<ExecuteActionResult>> ExecuteAction(string workspaceId, string itemUri)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);
            
        var action = GiveMe.Scope.WorkspaceLogic.FindObject(workspaceId, itemUri) as IElement;
        if (action == null)
        {
            return NotFound($"Action was not found: {itemUri} in Workspace {workspaceId}");
        }
            
        var actionLogic = new ActionLogic(GiveMe.Scope.WorkspaceLogic, GiveMe.Scope.ScopeStorage);
        try
        {
            var result = await actionLogic.ExecuteAction(
                action
            );

            var resultText = string.Empty;
            if (result != null)
            {
                resultText = MofJsonConverter.ConvertToJsonWithDefaultParameter(result);
            }

            return new ExecuteActionResult(true, string.Empty, string.Empty, resultText);

        }
        catch (Exception exc)
        {
            return new ExecuteActionResult(false, exc.Message, exc.ToString());
        }
    }

    public class ActionParams
    {
        /// <summary>
        /// Gets or sets the parameter for the action
        /// </summary>
        public MofObjectAsJson? Parameter { get; set; }
    }
        
    /// <summary>
    /// Defines the record for the ExecuteAction
    /// </summary>
    /// <param name="Success">true, if the action has been executed successfully</param>
    /// <param name="Reason">Reason why it was not created successfully</param>
    /// <param name="StackTrace">The corresponding stacktrace</param>
    /// <param name="Result">The resulting Json Text</param>
    public record ExecuteActionResult(bool Success, string Reason, string StackTrace, string? Result = null)
    {
        public override string ToString()
        {
            return $"{{ success = {Success}, reason = {Reason} }}";
        }
    }
}