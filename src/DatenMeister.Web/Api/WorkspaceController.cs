using System.Collections.Generic;
using System.Web.Http;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/workspace")]
    public class WorkspaceController : ApiController
    {
        private readonly IWorkspaceCollection _workspaceCollection;

        public WorkspaceController(IWorkspaceCollection workspaceCollection)
        {
            _workspaceCollection = workspaceCollection;
        }

        [Route("all")]
        public object Get()
        {
            var result = new List<object>();
            foreach (var workspace in _workspaceCollection.Workspaces)
            {
                result.Add(
                    new
                    {
                        workspace.id,
                        workspace.annotation
                    });
            }

            return result;
        }
    }
}