using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DatenMeister.Json;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]

    public class QueryController : ControllerBase
    {
        private IWorkspaceLogic _workspaceLogic;
        private IScopeStorage _scopeStorage;

        public QueryController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        [HttpPost("api/query/query_objects")]
        public async Task<ActionResult> QueryObjects([FromBody] MofObjectAsJson queryStatementAsText)
        {   
            // We finally have the raw data. Convert it to an Xmi  
            var mofJsonDeconverter = new MofJsonDeconverter(_workspaceLogic, _scopeStorage);
            var queryStatement = mofJsonDeconverter.ConvertToObject(queryStatementAsText);

            // Now we have the query statement. Get source and filters
            return await Task.FromResult(Ok());

        }
    }
}
