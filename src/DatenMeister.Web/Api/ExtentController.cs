using DatenMeister.EMOF.Interface.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/extent")]
    public class ExtentController : ApiController
    {
        [Route("all")]
        public object Get(string id)
        {
            var result = new List<object>();
            var workspace = Core.TheOne.Workspaces.Where(x => x.id == id).First();

            foreach (var extent in workspace.extent.Cast<IUriExtent>())
            {
                result.Add(
                    new
                    {
                        uri = extent.contextURI(),
                        count = extent.elements().Count()
                    });
            }

            return result;
        }
    }
}
