#r "nuget:autofac,4.6.1"
#r "../../deliverables/DatenMeister/DatenMeister.dll"

using Autofac;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Integration;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Runtime.Workspaces;
using System.Runtime.CompilerServices;

// Work-around helper method to get the source file location.
private static string GetSourceFile([CallerFilePath] string file = "") => file;

BurnSystems.Logging.TheLog.AddProvider(new ConsoleProvider());

var settings = new IntegrationSettings();
settings.DatabasePath = "tmp";

DatenMeister.Integration.GiveMe.DropDatenMeisterStorage(settings);

using(var dm = DatenMeister.Integration.GiveMe.DatenMeister(settings))
{
    var zipCodeManager = dm.Resolve<ZipCodeExampleManager>();
    var testExtent = zipCodeManager.AddZipCodeExample(WorkspaceNames.NameData, "plz.csv");
    
    var reportCreator = dm.Resolve<ReportCreator>();
    using (var writer = ReportCreator.CreateRandomFile(out var fileName, "tmp"))
    {
        var configuration = new ReportConfiguration();
        configuration.rootElement = testExtent;
        configuration.showDescendents = false;
        configuration.showRootElement = false;
        reportCreator.CreateReport(writer, configuration);

        var absolutePath = Path.Combine(Path.GetDirectoryName(GetSourceFile()), fileName);
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

Console.WriteLine("Hello world!");
