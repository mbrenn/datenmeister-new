using System.Web.Http;

namespace DatenMeister.Web.Api
{

    [RoutePrefix("api/datenmeister/client")]
    public class ClientController : ApiController
    {

        [Route("get_plugins")]
        public object GetPlugins()
        {
            return null;
        }
    }
}