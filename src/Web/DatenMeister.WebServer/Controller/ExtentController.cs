using System;
using System.Web;
using DatenMeister.Core;
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

        [HttpPost("api/extent/create_xmi")]
        public ActionResult<object> CreateXmi([FromBody] CreateXmiExtentParams createXmi)
        {
            var workspace = createXmi.Workspace;
            if (string.IsNullOrEmpty(workspace)) workspace = WorkspaceNames.WorkspaceData;

            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
            var loaded = extentManager.CreateAndAddXmiExtent(createXmi.ExtentUri, createXmi.FilePath, workspace);
            return new {success = loaded.LoadingState == ExtentLoadingState.Loaded};
        }

        [HttpDelete("api/extent/delete")]
        public ActionResult<object> DeleteExtent([FromBody] DeleteExtentParams param)
        {
            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
            var result = extentManager.RemoveExtent(param.Workspace, param.ExtentUri);
            return new {success = result};
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
        }

        public class DeleteExtentParams
        {
            public string Workspace { get; set; } = string.Empty;
            public string ExtentUri { get; set; } = string.Empty;
        }
    }
}