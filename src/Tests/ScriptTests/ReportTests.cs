using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Reports;
using DatenMeister.Reports.Simple;

namespace ScriptTests
{
    public static  class ReportTests
    {
        // Work-around helper method to get the source file location.
        private static string GetSourceFile([CallerFilePath] string file = "") => file;
        
        private static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

        public static async Task TestReportIssues(IElement configuration) => await TestReports(true, configuration);
        
        public static async Task TestReportZipCode(IElement configuration) => await TestReports(false, configuration);

        public static async Task TestReports(bool doIssues , IElement configuration)
        {
            TheLog.ClearProviders();
            TheLog.AddProvider(new ConsoleProvider());

            var settings = new IntegrationSettings {DatabasePath = Path.Combine(GetScriptFolder(), "tmp")};

            GiveMe.DropDatenMeisterStorage(settings);

            using var dm = await GiveMe.DatenMeister(settings);
            IUriExtent testExtent;
            if (doIssues)
            {
                var extentManager = dm.Resolve<ExtentManager>();

                var loaderConfig =
                    InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
                loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, "dm:///");
                loaderConfig.set(
                    _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, 
                    "E:\\OneDrive - Office365\\OneDrive - Martin Brenn\\issues.xmi");
                    
                testExtent = (await extentManager.LoadExtent(loaderConfig)).Extent;
            }
            else
            {
                var zipCodeManager = dm.Resolve<ZipCodeExampleManager>();
                testExtent = await zipCodeManager.AddZipCodeExample(
                    WorkspaceNames.WorkspaceData,
                    Path.Combine(GetScriptFolder(), "plz.csv"));
            }

            configuration.set(_DatenMeister._Reports._SimpleReportConfiguration.rootElement, testExtent!.contextURI());

            var targetPath = Path.Combine(GetScriptFolder(), "tmp2");
            Directory.CreateDirectory(targetPath);

            var reportCreator = new SimpleReportCreator(dm.WorkspaceLogic, dm.ScopeStorage, configuration);

            using (var writer = ReportHelper.CreateRandomFile(out var fileName, targetPath))
            {
                reportCreator.CreateReport(writer);

                var absolutePath = Path.Combine(GetScriptFolder(), fileName);
                DotNetHelper.CreateProcess(absolutePath);
            }
        }
    }
}