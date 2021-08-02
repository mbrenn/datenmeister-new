﻿using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ExtentController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public ExtentController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        /// <summary>
        /// Defines the parameter to create an xmi extent.
        /// Please be aware that the implementation may access every file folder and is absolutely insecure
        /// </summary>
        public class CreateXmiExtentParams
        {
            /// <summary>
            /// Gets or sets the file path in which the extent shall be added
            /// </summary>
            public string FilePath { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the extent uri
            /// </summary>
            public string ExtentUri { get; set; } = string.Empty;
        }

        [HttpPost("api/extent/create_xmi/{workspaceId}")]
        public ActionResult<object> CreateXmi(string workspaceId, [FromBody] CreateXmiExtentParams createXmi)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);

            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
            var loaded = extentManager.CreateAndAddXmiExtent(createXmi.ExtentUri, createXmi.FilePath, workspaceId);
            return new {success = loaded.LoadingState == ExtentLoadingState.Loaded};
        }
        
        public class DeleteExtentParams
        {
            public string Workspace { get; set; } = string.Empty;
            public string ExtentUri { get; set; } = string.Empty;
        }
        
        [HttpGet("api/extent/delete")]
        public ActionResult<object> DeleteExtent([FromBody] DeleteExtentParams param)
        {

            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
            var result = extentManager.RemoveExtent(param.Workspace, param.ExtentUri);
            return new {success = result};

        }
    }
}