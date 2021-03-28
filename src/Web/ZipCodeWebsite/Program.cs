using DatenMeister.Integration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ZipCodeWebsite.Models;

namespace ZipCodeWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var dm = GiveMe.DatenMeister();

            ZipCodeLogic.PrepareZipCode(dm);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}