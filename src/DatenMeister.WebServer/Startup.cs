using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RazorPagesMovie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region snippet_ConfigureServices
        /*public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<RazorPagesMovieContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("RazorPagesMovieContext")));
        }*/
        #endregion
        
        #region snippet_UseSqlite
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

           /* services.AddDbContext<RazorPagesMovieContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("MovieContext")));*/
        }
        #endregion
    

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            
           /* if (env.IsDevelopment())
            {
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }*/

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
