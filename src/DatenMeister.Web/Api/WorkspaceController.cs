using System.Collections.Generic;
using System.Web.Http;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/workspace")]
    public class WorkspaceController : ApiController
    {
        [Route("all")]
        public object Get()
        {
            var result = new List<object>();
            foreach (var workspace in Core.TheOne.Workspaces)
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