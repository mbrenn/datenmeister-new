﻿using System;
using System.Threading.Tasks;
using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
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
        
        [HttpPost("api/action/{actionName}")]
        public async Task<ActionResult<object>> ExecuteAction(string actionName, [FromBody] ActionParams actionParams)
        {
            var success = true;
            if (actionParams.Parameter == null)
            {
                throw new InvalidOperationException("Parameter are not set");
            }

            var mofParameter = new DirectJsonDeconverter(_workspaceLogic).ConvertToObject(actionParams.Parameter)
                ?? throw new InvalidOperationException("Conversion was not successful");
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

                    if (!success) return new ExecuteActionResult(false, result.FailLoadingMessage, result.FailLoadingMessage);
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
                        return new ExecuteActionResult(false, exc.Message, exc.ToString());
                    }

                    break;
            }

            return new ExecuteActionResult(success, "ActionNotFound", "");
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
        public record ExecuteActionResult(bool Success, string Reason, string StackTrace)
        {
            public override string ToString()
            {
                return $"{{ success = {Success}, reason = {Reason} }}";
            }
        }
    }
}