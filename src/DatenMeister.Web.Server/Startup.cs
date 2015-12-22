using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;


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
