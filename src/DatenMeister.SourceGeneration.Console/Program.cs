using CommandLine;
using CommandLine.Text;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.SourcecodeGenerator;
// ReSharper disable InconsistentNaming

namespace DatenMeister.SourceGeneration.Console;

internal class Program
{
#if DEBUG
    private const bool DryRun = true;
#else
    private const bool DryRun = false;
#endif
    public static async Task Main(string[] args)
    {
        // We do not need to interrupt the execution, since the loading will always 
        // be completely reinitialized
        ExtentConfigurationLoader.BreakOnFailedWorkspaceLoading = false;
        
        if (args.Length == 0)
        {
            await PerformStandardProcedure();
        }
        else
        {
            var value = Parser.Default.ParseArguments<CommandOptions>(args);
            value.WithParsed(x =>
            {
                if (CreateCodeForTypes(x.PathXml, x.PathTarget, x.Namespace).GetAwaiter().GetResult())
                {
                    System.Console.WriteLine("Sourcecode Generation finished");
                }
            });
            value.WithNotParsed(_ => System.Console.WriteLine(HelpText.AutoBuild(value, h => h)));
        }
    }

    public static Task<IDatenMeisterScope> GiveMeDatenMeister()
    {
        var integrationSettings = new IntegrationSettings
        {
            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Bootstrap"),
            IsReadOnly = true
        };

        return GiveMe.DatenMeister(integrationSettings);
    }

    /// <summary>
    /// Performs the standard procedure
    /// </summary>
    private static async Task PerformStandardProcedure()
    {
        var R = StandardProcedure.R;

        /*System.Console.WriteLine("Clean up .xmi-Files");

        await CleanUpProcedure.CleanUpExtent(
            $"{R}/../DatenMeister.Core/XmiFiles/Forms/DatenMeister.xmi",
            "dm:///intern.datenmeister.forms/",
            DryRun);
        await CleanUpProcedure.CleanUpExtent(
            $"{R}/../DatenMeister.Core/XmiFiles/Types/DatenMeister.xmi",
            "dm:///intern.datenmeister.types/",
            DryRun);*/

        System.Console.WriteLine("Perform the standard procedure.");

        // First, creates
        StandardProcedure.CreateSourceForExcel();

        System.Console.WriteLine("Closing Source Code Generator");


#if !DEBUG
            var T = StandardProcedure.T;
            
            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./ExcelModels.wrapper.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.wrapper.cs", true);
            File.Copy($"./ExcelModels.class.ts",
                $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/ExcelModels.class.ts", true);
#endif
    }

    private static async Task<bool> CreateCodeForTypes(string pathXml, string pathTarget, string theNamespace)
    {
        System.Console.WriteLine("Reading from: " + pathXml);
        System.Console.WriteLine("Writing to  : " + pathTarget);
        System.Console.WriteLine();

        if (!File.Exists(pathXml))
        {
            System.Console.WriteLine($"'{pathXml}' was not found! Operation aborted");
            System.Console.WriteLine($"Current environment path: {Environment.CurrentDirectory}");
            return false;
        }
            
        await CleanUpProcedure.CleanUpExtent(
            pathXml,
            "dm:///intern.datenmeister.forms/",
            DryRun);

        await using var dm = await GiveMeDatenMeister();
        var filename = Path.GetFileNameWithoutExtension(pathXml);

        var typeExtent = new MofUriExtent(
            XmiProvider.CreateByFile(pathXml),
            "dm:///_internal/types/internal", null);

        dm.WorkspaceLogic.AddExtent(dm.WorkspaceLogic.GetDataWorkspace(), typeExtent);

        // Generates tree for Type Script
        var generator = new TypeScriptInterfaceGenerator();
        generator.Walk(typeExtent);

        if (!Directory.Exists(pathTarget))
        {
            Directory.CreateDirectory(pathTarget);
        }

        await File.WriteAllTextAsync(Path.Combine(pathTarget, $"{filename}.ts"), generator.Result.ToString());
        System.Console.WriteLine($"TypeScript Code for {theNamespace} written");

        // Generates tree for StundenPlan
        var classGenerator = new ClassTreeGenerator
        {
            Namespace = theNamespace
        };

        classGenerator.Walk(typeExtent);

        var pathOfClassTree = Path.Combine(pathTarget, $"{filename}.cs");
        var fileContent = classGenerator.Result.ToString();
        await File.WriteAllTextAsync(pathOfClassTree, fileContent);
        
        // Generate wrapper
        
        // Generates tree for StundenPlan
        var wrapperGenerator = new WrapperTreeGenerator
        {
            Namespace = theNamespace
        };

        wrapperGenerator.Walk(typeExtent);

        var pathOfWrapper = Path.Combine(pathTarget, $"{filename}.wrapper.cs");
        var wrapperFileContent = wrapperGenerator.Result.ToString();
        await File.WriteAllTextAsync(pathOfWrapper, wrapperFileContent);

        System.Console.WriteLine($"C#-Code for {theNamespace} written");

        return true;
    }
}