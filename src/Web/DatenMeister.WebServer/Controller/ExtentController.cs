using System;
using System.Threading.Tasks;
using System.Web;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Json;
using DatenMeister.WebServer.Library.Helper;
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
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            extent = MvcUrlEncoder.DecodePathOrEmpty(extent);

            var foundExtent = _workspaceLogic.FindExtent(workspace, extent);
            return foundExtent == null 
                ? new ExistsResults { Exists = false } 
                : new ExistsResults { Exists = true };
        }

        [HttpPost("api/extent/set_properties/{workspace}/{extent}")]
        public ActionResult<object> SetProperties(string workspace, string extent,
            [FromBody] MofObjectAsJson properties)
        {
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            extent = MvcUrlEncoder.DecodePathOrEmpty(extent);

            var foundExtent = _workspaceLogic.FindExtent(workspace, extent)
                              ?? throw new InvalidOperationException("The extent was not found");

            var asObject = new DirectJsonDeconverter(_workspaceLogic, _scopeStorage)
                               .ConvertToObject(properties)
                           ?? throw new InvalidOperationException("Should not happen");
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
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            extent = MvcUrlEncoder.DecodePathOrEmpty(extent);

            var foundExtent = _workspaceLogic.FindExtent(workspace, extent)
                              ?? throw new InvalidOperationException("The extent was not found");
            var metaObject = (foundExtent as MofExtent)?.GetMetaObject();
            return metaObject == null
                ? new ActionResult<string?>(null as string)
                : new MofJsonConverter().ConvertToJson(metaObject);
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

            /// <summary>
            /// Gets or sets the message which is given when the creation of the extent has failed
            /// </summary>
            public string Message { get; set; } = string.Empty;
        }

        [HttpPost("api/extent/create_xmi")]
        public async Task<ActionResult<CreateXmiExtentResult>> CreateXmi([FromBody] CreateXmiExtentParams param)
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
            var loaded = await extentManager.CreateAndAddXmiExtent(param.ExtentUri, param.FilePath, workspace);
            
            return new CreateXmiExtentResult
            {
                Success = loaded.LoadingState == ExtentLoadingState.Loaded,
                Skipped = false,
                Message = loaded.FailLoadingMessage
            };
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

        [HttpDelete("api/extent/delete")]
        public async Task<ActionResult<DeleteExtentResult>> DeleteExtent([FromBody] DeleteExtentParams param)
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

            var result = await extentManager.RemoveExtent(param.Workspace, param.ExtentUri);
            return new DeleteExtentResult
            {
                Success = result
            };
        }

        public class ClearExtentParams
        {
            public string Workspace { get; set; } = string.Empty;
            public string ExtentUri { get; set; } = string.Empty;
        }

        public class ClearExtentResult
        {
            public bool Success { get; set; }
        }

        [HttpPost("api/extent/clear")]
        public ActionResult<ClearExtentResult> ClearExtent([FromBody] ClearExtentParams param)
        {
            var extent = _workspaceLogic.FindExtent(param.Workspace, param.ExtentUri);
            if (extent == null)
            {
                throw new InvalidOperationException("Extent has not been found");
            }

            extent.elements().clear();
            return new ClearExtentResult
            {
                Success = true
            };
        }
        
        public class ExportXmiResult
        {
            public string Xmi { get; set; } = string.Empty;
        }

        [HttpGet("api/extent/export_xmi/{workspace}/{extent}")]
        public ActionResult<ExportXmiResult> ExportXmi(string workspace, string extent)
        {
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            extent = MvcUrlEncoder.DecodePathOrEmpty(extent);
            var foundExtent = _workspaceLogic.FindExtent(workspace, extent);
            if (foundExtent == null)
            {
                throw new InvalidOperationException("Extent has not been found");
            }
            
            var provider = new XmiProvider();
            var tempExtent = new MofUriExtent(provider, "dm:///export", _scopeStorage);

            // Now do the copying. it makes us all happy
            var extentCopier = new ExtentCopier(new MofFactory(tempExtent));
            extentCopier.Copy(foundExtent.elements(), tempExtent.elements(), CopyOptions.CopyId);

            return new ExportXmiResult
            {
                Xmi = provider.Document.ToString()
            };
        }

        public class ImportXmiParams
        {
            public string Xmi { get; set; } = string.Empty;
        }

        public class ImportXmiResult
        {
            public bool Success { get; set; }
        }

        [HttpPost("api/extent/import_xmi/{workspace}/{extent}")]
        public async Task<ActionResult<ImportXmiResult>> ImportXmi(string workspace, string extent, [FromBody] ImportXmiParams param)
        {
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            extent = MvcUrlEncoder.DecodePathOrEmpty(extent);

            // Performs the import via the action handler...
            var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
            var importXmi = new ImportXmiActionHandler();
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ImportXmiAction);
            action.set(_DatenMeister._Actions._ImportXmiAction.workspace, workspace);
            action.set(_DatenMeister._Actions._ImportXmiAction.itemUri, extent);
            action.set(_DatenMeister._Actions._ImportXmiAction.xmi, param.Xmi);
            
            await importXmi.Evaluate(actionLogic, action);

            return new ImportXmiResult { Success = true };
        }
    }
}