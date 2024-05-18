using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Excel.Models;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.Xmi.Provider.XMI;
using DatenMeister.SourcecodeGenerator.SourceParser;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.Types;
using System.Reflection;
using System;
using System.Threading.Tasks;

namespace DatenMeister.SourceGeneration.Console;

public class StandardProcedure
{
    /// <summary>
    /// Defines the target path into which the files shall be stored in case
    /// of a Release Build. This is done by copying the files from t
    /// </summary>
    public const string R = "../../..";

    /// <summary>
    /// Defines the target path into which the files will be stored temporarily.
    /// </summary>
    public const string T = "./";

    public static async Task CreateTypescriptForDatenMeisterAllTypes()
    {
        using var dm = await GiveMe.DatenMeister();

        System.Console.Write("Create TypeScript for DatenMeister...");

        var pseudoExtent = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage).InternalTypes;

        ////////////////////////////////////////
        // Creates the class tree

        // Creates the source parser which is needed to navigate through the package
        var sourceParser = new ElementSourceParser();
        var classTreeGenerator = new TypeScriptInterfaceGenerator(sourceParser);

        classTreeGenerator.Walk(pseudoExtent);

        var pathOfClassTree = "DatenMeister.class.ts";
        var fileContent = classTreeGenerator.Result.ToString();
        File.WriteAllText(pathOfClassTree, fileContent);
        System.Console.WriteLine(" Done");
    }

    public static async Task CreateSourceCodeForDatenMeisterAllTypes()
    {
        using var dm = await GiveMe.DatenMeister();

        System.Console.Write("Create Sourcecode for DatenMeister...");

        var pseudoExtent = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage).InternalTypes;

        ////////////////////////////////////////
        // Creates the class tree

        // Creates the source parser which is needed to navigate through the package
        var sourceParser = new ElementSourceParser();
        var classTreeGenerator = new ClassTreeGenerator(sourceParser)
        {
            Namespace = "DatenMeister.Core.Models"
        };

        classTreeGenerator.Walk(pseudoExtent);

        var pathOfClassTree = "DatenMeister.class.cs";
        var fileContent = classTreeGenerator.Result.ToString();
        File.WriteAllText(pathOfClassTree, fileContent);
        System.Console.WriteLine(" Done");
    }

    public static void CreateSourceForExcel()
    {
        System.Console.Write("Create Sourcecode for Excel...");
        SourceGenerator.GenerateSourceFor(
            new SourceGeneratorOptions
            {
                ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                Name = "ExcelModels",
                Path = "./",
                Namespace = "DatenMeister.Excel.Models",
                Types = ExcelModels.AllTypes
            });
        System.Console.WriteLine(" Done");
    }

    public static void CreateSourceForUmlAndMof()
    {
        var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentUml, null);
        var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentMof, null);
        var primitiveTypeExtent =
            new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentPrimitiveTypes, null);

        var loader = new SimpleLoader();
        loader.LoadFromFile(new MofFactory(umlExtent), umlExtent, Path.Combine(AssemblyDirectory, "data/UML.xmi"));
        loader.LoadFromFile(new MofFactory(mofExtent), mofExtent, Path.Combine(AssemblyDirectory, "data/MOF.xmi"));
        loader.LoadFromFile(new MofFactory(primitiveTypeExtent), primitiveTypeExtent, Path.Combine(AssemblyDirectory, "data/PrimitiveTypes.xmi"));

        // Generates tree for UML
        var generator = new ClassTreeGenerator
        {
            Namespace = "DatenMeister.Core.Models.EMOF"
        };

        generator.Walk(umlExtent);

        File.WriteAllText($"{T}/uml.cs", generator.Result.ToString());
        System.Console.WriteLine("TS-Code for UML written");

        // Generates tree for MOF
        generator = new ClassTreeGenerator
        {
            Namespace = "DatenMeister.Core.Models.EMOF"
        };
        generator.Walk(mofExtent);

        File.WriteAllText($"{T}/mof.cs", generator.Result.ToString());
        System.Console.WriteLine("TS-Code for MOF written");

        // Generates tree for PrimitiveTypes
        generator = new ClassTreeGenerator
        {
            Namespace = "DatenMeister.Core.Models.EMOF"
        };
        generator.Walk(primitiveTypeExtent);

        File.WriteAllText($"{T}/primitivetypes.cs", generator.Result.ToString());
        System.Console.WriteLine("TS-Code for PrimitiveTypes written");
    }

    public static void CreateTypescriptForUmlAndMof()
    {
        var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentUml, null);
        var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentMof, null);
        var primitiveTypeExtent =
            new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentPrimitiveTypes, null);

        var loader = new SimpleLoader();
        loader.LoadFromFile(new MofFactory(umlExtent), umlExtent, Path.Combine(AssemblyDirectory, "data/UML.xmi"));
        loader.LoadFromFile(new MofFactory(mofExtent), mofExtent, Path.Combine(AssemblyDirectory, "data/MOF.xmi"));
        loader.LoadFromFile(new MofFactory(primitiveTypeExtent), primitiveTypeExtent, Path.Combine(AssemblyDirectory, "data/PrimitiveTypes.xmi"));

        // Generates tree for UML
        var generator = new TypeScriptInterfaceGenerator();
        generator.Walk(umlExtent);

        File.WriteAllText($"{T}/uml.ts", generator.Result.ToString());
        System.Console.WriteLine("TypeScript Code for UML written");

        // Generates tree for MOF
        generator = new TypeScriptInterfaceGenerator();
        generator.Walk(mofExtent);

        File.WriteAllText($"{T}/mof.ts", generator.Result.ToString());
        System.Console.WriteLine("TypeScript Code for MOF written");

        // Generates tree for PrimitiveTypes
        generator = new TypeScriptInterfaceGenerator();
        generator.Walk(primitiveTypeExtent);

        File.WriteAllText($"{T}/primitivetypes.ts", generator.Result.ToString());
        System.Console.WriteLine("C# Code for PrimitiveTypes written");
    }
    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = Assembly.GetExecutingAssembly().Location ?? throw new InvalidOperationException("Unknown CodeBase");
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}

