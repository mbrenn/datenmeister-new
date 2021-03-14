using System.IO;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;

namespace ZipCodeWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var dm = GiveMe.DatenMeister();

            PrepareZipCode(dm);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
        /// <summary>
        /// Gets or sets the zipcode extent
        /// </summary>
        public static IUriExtent? ZipCodeExtent { get; set; }
        
        /// <summary>
        /// Prepares the zipcode
        /// </summary>
        /// <param name="dm"></param>
        private static void PrepareZipCode(IDatenMeisterScope dm)
        {
            var manager = new ZipCodeExampleManager(
                dm.WorkspaceLogic,
                new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage), 
                dm.ScopeStorage);

            var foundExtent = 
                dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceData, "dm:///zipcodes/")
                as IUriExtent;
            
            if (foundExtent == null)
            {
                foundExtent = manager.AddZipCodeExample(
                    WorkspaceNames.WorkspaceData, 
                    "dm:///zipcodes/",
                    null,
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program))!.Location!)!,
                        "Loaded/zipcodes.csv"));
            }

            ZipCodeExtent = foundExtent;
        }
    }
}