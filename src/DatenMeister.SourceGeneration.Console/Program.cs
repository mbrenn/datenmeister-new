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
            var value = BurnSystems.CommandLine.Parser.ParseIntoOrShowUsage<CommandOptions>(args);
            
            if (value != null)
            {
                if (string.IsNullOrEmpty(value.XmiNamespace))
                {
                    value.XmiNamespace = "dm:///_internal/types/internal";
                }
                
                await CreateCodeForTypes(value);
                
                System.Console.WriteLine("Sourcecode Generation finished");
            }
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
    private static Task PerformStandardProcedure()
    {
        /*System.Console.WriteLine("Clean up .xmi-Files");

        await CleanUpProcedure.CleanUpExtent(
            $"{R}/../DatenMeister.Core/XmiFiles/Forms/DatenMeister.xmi",
            "dm:///intern.datenmeister.forms/",
            DryRun);
        await CleanUpProcedure.CleanUpExtent(
            $"{R}/../DatenMeister.Core/XmiFiles/Types/DatenMeister.xmi",
            "dm:///intern.datenmeister.types/",
            DryRun);
        await CleanUpProcedure.CleanUpExtent(
            pathXml,
            "dm:///intern.datenmeister.forms/",
            DryRun);
            */

        System.Console.WriteLine("Perform the standard procedure.");

        // First, creates
        StandardProcedure.CreateSourceForExcel();

        System.Console.WriteLine("Closing Source Code Generator");


#if !DEBUG
        
            var R = StandardProcedure.R;
            var T = StandardProcedure.T;
            
            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./ExcelModels.wrapper.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.wrapper.cs", true);
            File.Copy($"./ExcelModels.class.ts",
                $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/ExcelModels.class.ts", true);
#endif
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Generates source code files based on provided type definitions.
    /// This method processes the given XMI file and generates corresponding TypeScript interfaces,
    /// C# classes, and wrapper classes based on the type definitions found in the XMI.
    /// The generated files are saved to the specified target directory.
    /// </summary>
    /// <param name="options">The command options containing paths, namespaces, and other configurations for source code generation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the code generation was successful.
    /// Returns false if the input file does not exist or if no valid types were found in the XMI.</returns>
    private static async Task<bool> CreateCodeForTypes(CommandOptions options)
    {
        var typesFromAssembly = new TypeUriMappingLogic();
        typesFromAssembly.LoadAssemblies();
        
        var pathXml = options.PathXml;
        var pathTarget = options.PathTarget;
        var codeNamespace = options.CodeNamespace;
        var xmiNamespace = options.XmiNamespace;
        System.Console.WriteLine("Reading from  : " + pathXml);
        System.Console.WriteLine("Writing to    : " + pathTarget);
        System.Console.WriteLine("Code Namespace: " + codeNamespace);
        System.Console.WriteLine("Xmi Namespace : " + xmiNamespace);
        System.Console.WriteLine();

        if (!File.Exists(pathXml))
        {
            System.Console.WriteLine($"'{pathXml}' was not found! Operation aborted");
            System.Console.WriteLine($"Current environment path: {Environment.CurrentDirectory}");
            return false;
        }

        await using var dm = await GiveMeDatenMeister();
        var filename = Path.GetFileNameWithoutExtension(pathXml);

        var typeExtent = new MofUriExtent(
            XmiProvider.CreateByFile(pathXml),
            options.XmiNamespace, null);

        dm.WorkspaceLogic.AddExtent(dm.WorkspaceLogic.GetDataWorkspace(), typeExtent);
        if (!typeExtent.elements().Any())
        {
            System.Console.WriteLine("Aborted creation because no item is loaded.");
            return false;
        }

        // Generates tree for Type Script
        var generator = new TypeScriptInterfaceGenerator();
        generator.Walk(typeExtent);
        
        if (generator.TotalWalked == 0)
        {
            System.Console.WriteLine("Aborted creation because no package or class has been found.");
            return false;
        }

        if (!Directory.Exists(pathTarget))
        {
            Directory.CreateDirectory(pathTarget);
        }

        await File.WriteAllTextAsync(Path.Combine(pathTarget, $"{filename}.ts"), generator.Result.ToString());
        System.Console.WriteLine($"TypeScript Code for {codeNamespace} written");

        // Generates tree for StundenPlan
        var classGenerator = new ClassTreeGenerator
        {
            Namespace = codeNamespace
        };

        classGenerator.Walk(typeExtent);
        
        var pathOfClassTree = Path.Combine(pathTarget, $"{filename}.cs");
        var fileContent = classGenerator.Result.ToString();
        await File.WriteAllTextAsync(pathOfClassTree, fileContent);
        
        System.Console.WriteLine($"C#-Code for {codeNamespace} written");
        
        // Generate wrapper
        
        // Generates tree for StundenPlan
        var wrapperGenerator = new WrapperTreeGenerator
        {
            Namespace = codeNamespace,
            TypeUriMapping = typesFromAssembly
        };

        wrapperGenerator.Walk(typeExtent);

        var pathOfWrapper = Path.Combine(pathTarget, $"{filename}.wrapper.cs");
        var wrapperFileContent = wrapperGenerator.Result.ToString();
        await File.WriteAllTextAsync(pathOfWrapper, wrapperFileContent);

        System.Console.WriteLine($"C#-Wrapper-Code for {codeNamespace} written");

        return true;
    }
}