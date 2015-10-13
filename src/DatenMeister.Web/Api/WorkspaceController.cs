using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        id = workspace.id,
                        annotation = workspace.annotation
                    });
            }

            return result;
        }
    }
}
