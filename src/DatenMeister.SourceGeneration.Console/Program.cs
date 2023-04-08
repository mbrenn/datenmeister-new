using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Excel.Models;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.Xmi.Provider.XMI;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.SourcecodeGenerator.SourceParser;
using DatenMeister.Types;

namespace DatenMeister.SourceGeneration.Console
{
    class Program
    {
        public const string R = "../../..";

        public static void Main(string[] args)
        {
            // First, creates
            CreateSourceForUmlAndMof();

            CreateTypescriptForUmlAndMof();

            CreateSourceForExcel();

            // CreateSourceForDataViews();

            //CreateSourceCodeForDatenMeister();

            CreateSourceCodeForDatenMeisterAllTypes();

            CreateTypescriptForDatenMeisterAllTypes();

            CreateCodeForStundenPlan();

            System.Console.WriteLine("Closing Source Code Generator");

#if !DEBUG
            File.Copy($"{R}/primitivetypes.cs", $"{R}/../DatenMeister.Core/Models/EMOF/primitivetypes.cs", true);
            File.Copy($"{R}/mof.cs", $"{R}/../DatenMeister.Core/Models/EMOF/mof.cs", true);
            File.Copy($"{R}/uml.cs", $"{R}/../DatenMeister.Core/Models/EMOF/uml.cs", true);
            File.Copy($"{R}/primitivetypes.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/primitivetypes.ts", true);
            File.Copy($"{R}/mof.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/mof.ts", true);
            File.Copy($"{R}/uml.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/uml.ts", true);
            
            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./DatenMeister.class.cs", $"{R}/../DatenMeister.Core/Models/DatenMeister.class.cs", true);
            File.Copy($"./DatenMeister.class.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/DatenMeister.class.ts", true);
            File.Copy($"./ExcelModels.class.ts", $"{R}/../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/models/ExcelModels.class.ts", true);
#endif
        }

        private static void CreateTypescriptForDatenMeisterAllTypes()
        {
            var dm = GiveMe.DatenMeister();

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

        private static void CreateSourceCodeForDatenMeisterAllTypes()
        {
            var dm = GiveMe.DatenMeister();

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

        private static void CreateSourceForExcel()
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

        private static void CreateSourceForUmlAndMof()
        {
            var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentUml, null);
            var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentMof, null);
            var primitiveTypeExtent =
                new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentPrimitiveTypes, null);

            var loader = new SimpleLoader();
            loader.LoadFromFile(new MofFactory(umlExtent), umlExtent, "data/UML.xmi");
            loader.LoadFromFile(new MofFactory(mofExtent), mofExtent, "data/MOF.xmi");
            loader.LoadFromFile(new MofFactory(primitiveTypeExtent), primitiveTypeExtent, "data/PrimitiveTypes.xmi");

            // Generates tree for UML
            var generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core.Models.EMOF"
            };

            generator.Walk(umlExtent);

            File.WriteAllText($"{R}/uml.cs", generator.Result.ToString());
            System.Console.WriteLine("TS-Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core.Models.EMOF"
            };
            generator.Walk(mofExtent);

            File.WriteAllText($"{R}/mof.cs", generator.Result.ToString());
            System.Console.WriteLine("TS-Code for MOF written");

            // Generates tree for PrimitiveTypes
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core.Models.EMOF"
            };
            generator.Walk(primitiveTypeExtent);

            File.WriteAllText($"{R}/primitivetypes.cs", generator.Result.ToString());
            System.Console.WriteLine("TS-Code for PrimitiveTypes written");
        }

        private static void CreateTypescriptForUmlAndMof()
        {
            var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentUml, null);
            var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentMof, null);
            var primitiveTypeExtent =
                new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentPrimitiveTypes, null);

            var loader = new SimpleLoader();
            loader.LoadFromFile(new MofFactory(umlExtent), umlExtent, "data/UML.xmi");
            loader.LoadFromFile(new MofFactory(mofExtent), mofExtent, "data/MOF.xmi");
            loader.LoadFromFile(new MofFactory(primitiveTypeExtent), primitiveTypeExtent, "data/PrimitiveTypes.xmi");

            // Generates tree for UML
            var generator = new TypeScriptInterfaceGenerator();
            generator.Walk(umlExtent);

            File.WriteAllText($"{R}/uml.ts", generator.Result.ToString());
            System.Console.WriteLine("TypeScript Code for UML written");

            // Generates tree for MOF
            generator = new TypeScriptInterfaceGenerator();
            generator.Walk(mofExtent);

            File.WriteAllText($"{R}/mof.ts", generator.Result.ToString());
            System.Console.WriteLine("TypeScript Code for MOF written");

            // Generates tree for PrimitiveTypes
            generator = new TypeScriptInterfaceGenerator();
            generator.Walk(primitiveTypeExtent);

            File.WriteAllText($"{R}/primitivetypes.ts", generator.Result.ToString());
            System.Console.WriteLine("C# Code for PrimitiveTypes written");
        }

        private static void CreateCodeForStundenPlan()
        {
            var formExtent = new MofUriExtent(new InMemoryProvider(), "dm:///forms.stundenplan.datenmeister/", null);
            var typeExtent = new MofUriExtent(new InMemoryProvider(), "dm:///types.stundenplan.datenmeister/", null);

            var loader = new SimpleLoader();
            loader.LoadFromFile(new MofFactory(formExtent), formExtent, "../../../../Apps/DatenMeister.StundenPlan/xmi/StundenPlan.Forms.xml");
            loader.LoadFromFile(new MofFactory(typeExtent), typeExtent, "../../../../Apps/DatenMeister.StundenPlan/xmi/StundenPlan.Types.xml");
            
            // Generates tree for Type Script
            var generator = new TypeScriptInterfaceGenerator();
            generator.Walk(typeExtent);
            
            File.WriteAllText($"{R}/StundenPlan.Types.ts", generator.Result.ToString());
            System.Console.WriteLine("TypeScript Code for StundenPlan written");
            
            // Generates tree for StundenPlan
            var classGenerator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.StundenPlan.Model"
            };

            classGenerator.Walk(typeExtent);
            
            var pathOfClassTree = $"{R}/StundenPlan.Types.cs";
            var fileContent = classGenerator.Result.ToString();
            File.WriteAllText(pathOfClassTree, fileContent);
            
            System.Console.WriteLine("C#-Code for StundenPlan written");
            
#if !DEBUG
            File.Copy($"{R}/StundenPlan.Types.ts", $"{R}/../Apps/DatenMeister.StundenPlan/resources/DatenMeister.StundenPlan.ts", true);
            File.Copy($"{R}/StundenPlan.Types.cs", $"{R}/../Apps/DatenMeister.StundenPlan/Model/DatenMeister.StundenPlan.cs", true);
#endif
        }
    }
}