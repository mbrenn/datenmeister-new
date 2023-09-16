using DatenMeister.Integration.DotNet;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Library;
using DatenMeister.WebServer.Library.PageRegistration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Math.EC.Endo;

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
            appNavigation.Items.Add(new AppNavigationItem {Title = "Actions", Url = "/Actions", Image="actions"});
            appNavigation.Items.Add(new AppNavigationItem {Title = "Settings", Url = "/Settings", Image="settings"});
            appNavigation.Items.Add(new AppNavigationItem
            {
                Title = "Workspaces", Url = "/ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces", Image = "workspaces"
            });

            services.AddControllers();
            services.AddRazorPages();
            services.AddSingleton(appNavigation);

            services.AddSingleton(GiveMe.Scope.WorkspaceLogic);
            services.AddSingleton(GiveMe.Scope.ScopeStorage);
            
            var extentController = new ExtentItemsController(
                GiveMe.Scope.WorkspaceLogic,
                GiveMe.Scope.ScopeStorage);
            services.AddSingleton(extentController);
            
            var workspaceController = new WorkspaceController(
                GiveMe.Scope.WorkspaceLogic,
                GiveMe.Scope.ScopeStorage);
            services.AddSingleton(workspaceController);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            var config = new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            };

            var extensionProvider = new FileExtensionContentTypeProvider();
            extensionProvider.Mappings.Add(".dll", "application/octet-stream");
            config.ContentTypeProvider = extensionProvider;

            
            app.UseStaticFiles(config); // Twice to get rid of issue for blazor client*/
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                var data = GiveMe.Scope.ScopeStorage.Get<PageRegistrationData>();
                foreach (var entry in data.PageFactories)
                {
                    endpoints.MapGet(
                        entry.Url, async context =>
                        {
                            context.Response.ContentType = entry.ContentType;
                            await using var pageStream = entry.PageStreamFunction();
                            await pageStream.CopyToAsync(context.Response.Body);
                        });
                }
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}