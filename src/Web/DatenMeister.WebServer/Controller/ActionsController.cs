using System;
using System.Threading.Tasks;
using DatenMeister.Actions;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    public class ActionsController : ControllerBase
    {
        [HttpPost("api/action/{actionName}")]
        public async Task<ActionResult<object>> ExecuteAction(string actionName, [FromBody] ActionParams actionParams)
        {
            var success = true;
            if (actionParams.Parameter == null)
            {
                throw new InvalidOperationException("Parameter are not set");
            }

            var mofParameter = DirectJsonDeconverter.ConvertToObject(actionParams.Parameter);
            switch (actionName)
            {
                case "Workspace.Extent.Xmi.Create":
                    if (mofParameter.metaclass?.@equals(_DatenMeister.TheOne.ExtentLoaderConfigs
                            .__XmiStorageLoaderConfig) != true)
                    {
                        throw new InvalidOperationException("Wrong metaclass. Expected XmiStorageLoaderConfig");
                    }

                    var workspaceLogic = GiveMe.Scope.WorkspaceLogic;
                    var extentManager = new ExtentManager(workspaceLogic, GiveMe.Scope.ScopeStorage);
                    var result = extentManager.LoadExtent(
                        mofParameter,
                        ExtentCreationFlags.LoadOrCreate);
                    success =
                        result.LoadingState is ExtentLoadingState.Loaded or ExtentLoadingState.LoadedReadOnly;

                    if (!success) return new ExecuteActionResult(false, result.FailLoadingMessage);
                    break;

                case "Execute":
                    var actionLogic = new ActionLogic(GiveMe.Scope.WorkspaceLogic, GiveMe.Scope.ScopeStorage);
                    try
                    {
                        await actionLogic.ExecuteAction(
                            mofParameter
                        );

                    }
                    catch (Exception exc)
                    {
                        return new ExecuteActionResult(false, exc.Message);
                    }

                    break;
            }

            return new ExecuteActionResult(success, "ActionNotFound");
        }

        public class ActionParams
        {
            /// <summary>
            /// Gets or sets the parameter for the action
            /// </summary>
            public MofObjectAsJson? Parameter { get; set; }
        }
        
        public record ExecuteActionResult(bool Success, string Reason)
        {
            public override string ToString()
            {
                return $"{{ success = {Success}, reason = {Reason} }}";
            }
        }
    }
}