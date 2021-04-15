using DatenMeister.WebServer.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DatenMeister.WebServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appNavigation = new AppNavigationDefinition();
            appNavigation.Items.Add(new AppNavigationItem {Title = "Home", Url = "/", Image = "home"});
            appNavigation.Items.Add(new AppNavigationItem {Title = "About", Url = "/About", Image="about"});

            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton(appNavigation);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            var config = new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            };

            var extensionProvider = new FileExtensionContentTypeProvider();
            extensionProvider.Mappings.Add(".dll", "application/octet-stream");
            config.ContentTypeProvider = extensionProvider;

            app.UseStaticFiles(config);
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}
