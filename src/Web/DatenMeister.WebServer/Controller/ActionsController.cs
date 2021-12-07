using System;
using DatenMeister.Core.Helper;
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
        public class ActionParams
        {
            /// <summary>
            /// Gets or sets the parameter for the action
            /// </summary>
            public MofObjectAsJson? Parameter { get; set; }    
        }


        [HttpPost("api/action/{actionName}")]
        public ActionResult<object> ExecuteAction(string actionName, [FromBody] ActionParams actionParams)
        {
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
                    extentManager.LoadExtent(
                        mofParameter,
                        ExtentCreationFlags.LoadOrCreate);
                    break;
            }
            return new { success = false, reason = "ActionNotFound" };
        }
    }
}