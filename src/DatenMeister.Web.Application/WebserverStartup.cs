using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DatenMeister.Web.Application.WebserverStartup))]
namespace DatenMeister.Web.Application
{
    public class WebserverStartup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            app.UseWelcomePage("/");
        }
    }
}
