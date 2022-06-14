using System;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{

    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ExtentController : ControllerBase
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ExtentController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }
        
        public record ExistsResults
        {
            public bool Exists { get; set; }
        }

        [HttpGet("api/extent/exists/{workspace}/{extent}")]
        public ActionResult<ExistsResults> Exists(string workspace, string extent)
        {
            workspace = HttpUtility.UrlDecode(workspace);
            extent = HttpUtility.UrlDecode(extent);

            var foundExtent = _workspaceLogic.FindExtent(workspace, extent);
            return foundExtent == null 
                ? new ExistsResults { Exists = false } 
                : new ExistsResults { Exists = true };
        }

        [HttpPost("api/extent/set_properties/{workspace}/{extent}")]
        public ActionResult<object> SetProperties(string workspace, string extent,
            [FromBody] MofObjectAsJson properties)
        {
            workspace = HttpUtility.UrlDecode(workspace);
            extent = HttpUtility.UrlDecode(extent);

            var foundExtent = _workspaceLogic.FindExtent(workspace, extent)
                              ?? throw new InvalidOperationException("The extent was not found");

            var asObject = DirectJsonDeconverter.ConvertToObject(properties);
            var asAllProperties = asObject as IObjectAllProperties
                                  ?? throw new InvalidOperationException(
                                      "For whatever reason, the properties could not be converted");

            foreach (var property in asAllProperties.getPropertiesBeingSet())
            {
                foundExtent.set(property, asObject.get(property));
            }

            return new {success = true};
        }

        [HttpGet("api/extent/get_properties/{workspace}/{extent}")]
        public ActionResult<string?> GetProperties(string workspace, string extent)
        {
            workspace = HttpUtility.UrlDecode(workspace);
            extent = HttpUtility.UrlDecode(extent);

            var foundExtent = _workspaceLogic.FindExtent(workspace, extent)
                              ?? throw new InvalidOperationException("The extent was not found");
            var metaObject = (foundExtent as MofExtent)?.GetMetaObject();
            return metaObject == null
                ? new ActionResult<string?>(null as string)
                : new MofJsonConverter().ConvertToJson(metaObject);
        }

        [HttpPost("api/extent/create_xmi")]
        public ActionResult<CreateXmiExtentResult> CreateXmi([FromBody] CreateXmiExtentParams param)
        {
            var workspace = param.Workspace;
            if (string.IsNullOrEmpty(workspace)) workspace = WorkspaceNames.WorkspaceData;
            
            if (param.SkipIfExisting && 
                _workspaceLogic.FindExtent(param.Workspace, param.ExtentUri) != null)
            {
                return new CreateXmiExtentResult
                {
                    Success = true,
                    Skipped = true
                };
            }

            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
            var loaded = extentManager.CreateAndAddXmiExtent(param.ExtentUri, param.FilePath, workspace);
            return new CreateXmiExtentResult
                {Success = loaded.LoadingState == ExtentLoadingState.Loaded};
        }

        [HttpDelete("api/extent/delete")]
        public ActionResult<DeleteExtentResult> DeleteExtent([FromBody] DeleteExtentParams param)
        {
            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);

            if (param.SkipIfNotExisting && 
                _workspaceLogic.FindExtent(param.Workspace, param.ExtentUri) == null)
            {
                return new DeleteExtentResult
                {
                    Success = true,
                    Skipped = true
                };
            }

            var result = extentManager.RemoveExtent(param.Workspace, param.ExtentUri);
            return new DeleteExtentResult
            {
                Success = result
            };
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

            /// <summary>
            /// Creates a new workspace
            /// </summary>
            public string Workspace { get; set; } = string.Empty;
            
            public bool SkipIfExisting { get; set; }
        }

        public class CreateXmiExtentResult
        {
            public bool Success { get; set; }
        
            public bool Skipped { get; set; }
        }

        public class DeleteExtentParams
        {
            public string Workspace { get; set; } = string.Empty;
            public string ExtentUri { get; set; } = string.Empty;
            
            public bool SkipIfNotExisting { get; set; }
        }

        public class DeleteExtentResult
        {
            public bool Success { get; set; }
        
            public bool Skipped { get; set; }
        }
    }
}