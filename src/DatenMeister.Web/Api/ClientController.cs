using System.Web.Http;
using DatenMeister.Web.Models.Modules;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/client")]
    public class ClientController : ApiController
    {
        private IClientModulePlugin _clientModulePlugin;

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