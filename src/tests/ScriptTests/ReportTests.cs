using System;
using System.IO;
using System.Runtime.CompilerServices;
using Autofac;
using BurnSystems.Logging.Provider;
using DatenMeister.Integration;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;

namespace ScriptTests
{
    public static  class ReportTests
    {
        // Work-around helper method to get the source file location.
        private static string GetSourceFile([CallerFilePath] string file = "") => file;
        private static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

        public static void TestReportIssues(SimpleReportConfiguration configuration) => TestReports(true, configuration);
        
        public static void TestReportZipCode(SimpleReportConfiguration configuration) => TestReports(false, configuration);

        public static void TestReports(bool doIssues , SimpleReportConfiguration configuration)
        {
            BurnSystems.Logging.TheLog.AddProvider(new ConsoleProvider());

            var settings = new IntegrationSettings {DatabasePath = Path.Combine(GetScriptFolder(), "tmp")};

            DatenMeister.Integration.GiveMe.DropDatenMeisterStorage(settings);

            using (var dm = DatenMeister.Integration.GiveMe.DatenMeister(settings))
            {
                DatenMeister.Core.EMOF.Interface.Identifiers.IUriExtent testExtent;
                if (doIssues)
                {
                    var extentManager = dm.Resolve<ExtentManager>();
                    var loaderConfig = new XmiStorageConfiguration
                    {
                        extentUri = "dm:///",
                        filePath = "E:\\OneDrive - Office365\\OneDrive - Martin Brenn\\issues.xmi"
                    };

                    testExtent = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOnly);
                }
                else
                {
                    var zipCodeManager = dm.Resolve<ZipCodeExampleManager>();
                    testExtent = zipCodeManager.AddZipCodeExample(
                        WorkspaceNames.NameData,
                        Path.Combine(GetScriptFolder(), "plz.csv"));
                }

                configuration.rootElement = testExtent;

                var targetPath = Path.Combine(GetScriptFolder(), "tmp2");
                Directory.CreateDirectory(targetPath);

                var reportCreator = new ReportCreator(dm.WorkspaceLogic, configuration);

                using (var writer = ReportCreator.CreateRandomFile(out var fileName, targetPath))
                {
                    reportCreator.CreateReport(writer);

                    var absolutePath = Path.Combine(GetScriptFolder(), fileName);
                    Console.WriteLine(absolutePath);

                    var processStartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = absolutePath
                    };

                    System.Diagnostics.Process.Start(processStartInfo);
                }

                Console.WriteLine(testExtent);
            }
        }
    }
}