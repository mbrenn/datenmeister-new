using DatenMeister.Integration.DotNet;
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