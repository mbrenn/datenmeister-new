using DatenMeister.Integration.DotNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using ZipCodeWebsite.Models;

namespace ZipCodeWebsite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var dm = await GiveMe.DatenMeister();

            await ZipCodeLogic.PrepareZipCode(dm);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}