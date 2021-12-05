using System;
using DatenMeister.Integration.DotNet;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    public class ActionsController : ControllerBase
    {
        public class ActionParams
        {
            /// <summary>
            /// Gets or sets the parameter for the action
            /// </summary>
            public MofObjectAsJson? Parameter { get; set; }    
        }
        
        public ActionResult<object> ExecuteAction(string actionName, [FromBody] ActionParams actionParams)
        {
            if (actionParams.Parameter == null)
            {
                throw new InvalidOperationException("Parameter are not set");
            }
            
            var mofParameter = new MofJsonDeconverter()
                .ConvertToObject(actionParams.Parameter);
            switch (actionName)
            {
                case "Workspace.Extent.Xmi.Create":
                    
                    break;
            }
            return new { success = false, reason = "ActionNotFound" };
        }
    }
}