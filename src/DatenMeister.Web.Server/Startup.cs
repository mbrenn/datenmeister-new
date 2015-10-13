using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DatenMeister.Web.Server.Startup))]

namespace DatenMeister.Web.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            StartDatenmeister(app);
        }
    }
}
