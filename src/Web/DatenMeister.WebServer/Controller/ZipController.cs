using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Modules.ZipCodeExample;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/zip")]
    [ApiController]
    public class ZipController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public ZipController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }
        
        public class CreateZipExampleParam
        {
            public string Workspace { get; set; } = string.Empty;
        }

        /// <summary>
        /// Defines the result
        /// </summary>
        /// <param name="Success">Flag, whether the creation was successful</param>
        /// <param name="ExtentUri">Name of the created extent</param>
        public record CreateZipExampleResult(bool Success, string ExtentUri)
        {
            public override string ToString()
            {
                return $"{{ success = {Success}, extentUri = {ExtentUri} }}";
            }
        }

        [HttpPost("api/zip/create")]
        public ActionResult<CreateZipExampleResult> CreateZipExample([FromBody] CreateZipExampleParam param)
        {
            var zipExample = new ZipCodeExampleManager(_workspaceLogic,
                new ExtentManager(_workspaceLogic, _scopeStorage), _scopeStorage);
            var result = zipExample.AddZipCodeExample(param.Workspace);

            return new CreateZipExampleResult(true, result.contextURI());
        }
    }
}