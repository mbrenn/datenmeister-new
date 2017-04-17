using DatenMeister.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.Web.Api
{
    [Route("api/datenmeister/client")]
    public class ClientController : Controller
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