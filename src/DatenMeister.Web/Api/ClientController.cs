using System.Web.Http;
using DatenMeister.Modules;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/client")]
    public class ClientController : ApiController
    {
        private readonly IClientModulePlugin _clientModulePlugin;

        public ClientController(IClientModulePlugin clientModulePlugin)
        {
            _clientModulePlugin = clientModulePlugin;
        }

        [Route("get_plugins")]
        public object GetPlugins()
        {
            return new
            {
                scriptPaths = _clientModulePlugin.ScriptPaths
            };
        }
    }
}