using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core;
using Microsoft.AspNetCore.Mvc;
using DatenMeister.Web.Json;

namespace DatenMeister.WebServer.Controller;

[Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
[ApiController]

public class QueryController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : ControllerBase
{
    [HttpPost("api/query/query_objects")]
    public async Task<ActionResult> QueryObjects([FromBody] MofObjectAsJson queryStatementAsText)
    {   
        // We finally have the raw data. Convert it to an Xmi  
        var mofJsonDeconverter = new MofJsonDeconverter(workspaceLogic, scopeStorage);
        var queryStatement = mofJsonDeconverter.ConvertToObject(queryStatementAsText);

        // Now we have the query statement. Get source and filters
        return await Task.FromResult(Ok());

    }
}