using CommandLine;
using CommandLine.Text;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Integration.DotNet;
using DatenMeister.SourcecodeGenerator;

namespace DatenMeister.SourceGeneration.Console;

class Program
{
#if DEBUG
    const bool dryRun = true;
#else
        const bool dryRun = false;
#endif
    public static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            await PerformStandardProcedure();
        }
        else
        {
            var value = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
            value.WithParsed(x => CreateCodeForTypes(x.PathXml, x.PathTarget, x.Namespace).Wait());
            value.WithNotParsed(x => System.Console.WriteLine(HelpText.AutoBuild(value, h => h)));
        }
    }

    public static Task<IDatenMeisterScope> GiveMeDatenMeister()
    {
        var integrationSettings = new IntegrationSettings
        {
            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Bootstrap")
        };

        return GiveMe.DatenMeister(integrationSettings);
    }

    /// <summary>
    /// Performs the standard procedure
    /// </summary>
    private static async Task PerformStandardProcedure()
    {
        var R = StandardProcedure.R;

        System.Console.WriteLine("Clean up .xmi-Files");

        await CleanUpProcedure.CleanUpExtent(
            $"{R}/..//DatenMeister.Core/XmiFiles/Forms/DatenMeister.xmi",
            "dm:///intern.datenmeister.forms/",
            dryRun);
        await CleanUpProcedure.CleanUpExtent(
            $"{R}/..//DatenMeister.Core/XmiFiles/Types/DatenMeister.xmi",
            "dm:///intern.datenmeister.types/",
            dryRun);

        System.Console.WriteLine("Perform the standard procedure.");

        // First, creates
        StandardProcedure.CreateSourceForUmlAndMof();

        StandardProcedure.CreateTypescriptForUmlAndMof();

        StandardProcedure.CreateSourceForExcel();

        await StandardProcedure.CreateSourceCodeForDatenMeisterAllTypes();

        await StandardProcedure.CreateTypescriptForDatenMeisterAllTypes();

        System.Console.WriteLine("Closing Source Code Generator");


#if !DEBUG
            var T = StandardProcedure.T;

            File.Copy($"{T}/primitivetypes.cs", $"{R}/../DatenMeister.Core/Models/EMOF/primitivetypes.cs", true);
            File.Copy($"{T}/mof.cs", $"{R}/../DatenMeister.Core/Models/EMOF/mof.cs", true);
            File.Copy($"{T}/uml.cs", $"{R}/../DatenMeister.Core/Models/EMOF/uml.cs", true);
            File.Copy($"{T}/primitivetypes.ts",
                $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/primitivetypes.ts", true);
            File.Copy($"{T}/mof.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/mof.ts", true);
            File.Copy($"{T}/uml.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/uml.ts", true);

            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./DatenMeister.class.cs", $"{R}/../DatenMeister.Core/Models/DatenMeister.class.cs", true);
            File.Copy($"./DatenMeister.class.ts",
                $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/DatenMeister.class.ts", true);
            File.Copy($"./ExcelModels.class.ts",
                $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/ExcelModels.class.ts", true);

            File.Delete($"{T}/primitivetypes.cs");
            File.Delete($"{T}/primitivetypes.ts");
            File.Delete($"{T}/primitivetypes.js");
            File.Delete($"{T}/mof.cs");
            File.Delete($"{T}/mof.ts");
            File.Delete($"{T}/mof.js");
            File.Delete($"{T}/uml.cs");
            File.Delete($"{T}/uml.ts");
            File.Delete($"{T}/uml.js");
#endif
    }

    public static async Task CreateCodeForTypes(string pathXml, string pathTarget, string theNamespace)
    {
        System.Console.WriteLine("Reading from: " + pathXml);
        System.Console.WriteLine("Writing to  : " + pathTarget);
        System.Console.WriteLine();
            
        await CleanUpProcedure.CleanUpExtent(
            pathXml,
            "dm:///intern.datenmeister.forms/",
            dryRun);

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

        File.WriteAllText(Path.Combine(pathTarget, $"{filename}.ts"), generator.Result.ToString());
        System.Console.WriteLine($"TypeScript Code for {theNamespace} written");

        // Generates tree for StundenPlan
        var classGenerator = new ClassTreeGenerator
        {
            Namespace = theNamespace
        };

        classGenerator.Walk(typeExtent);

        var pathOfClassTree = Path.Combine(pathTarget, $"{filename}.cs");
        var fileContent = classGenerator.Result.ToString();
        File.WriteAllText(pathOfClassTree, fileContent);

        System.Console.WriteLine($"C#-Code for {theNamespace} written");
    }
}