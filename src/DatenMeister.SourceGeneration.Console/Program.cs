using System.IO;
using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Models;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Models.Forms;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.NetCore;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourceGeneration.Console
{
    class Program
    {
        public const string R = "../../..";

        public static void Main(string[] args)
        {
            // First, creates
            CreateSourceForUmlAndMof();

            CreateSourceForWebFields();

            CreateSourceForExcel();

            CreateSourceForManagementProvider();

            CreateSourceForFastFilter();

            // CreateSourceForDataViews();
            
            //CreateSourceCodeForDatenMeister();

            CreateSourceCodeForDatenMeisterAllTypes();

#if !DEBUG
            File.Copy($"{R}/primitivetypes.cs", $"{R}/../DatenMeister/Models/EMOF/primitivetypes.cs", true);
            File.Copy($"{R}/mof.cs", $"{R}/../DatenMeister/Models/EMOF/mof.cs", true);
            File.Copy($"{R}/uml.cs", $"{R}/../DatenMeister/Models/EMOF/uml.cs", true);
            
            File.Copy($"./FormAndFields.class.cs", $"{R}/../DatenMeister/Models/Forms/FormAndFields.class.cs", true);
            File.Copy($"./FormAndFields.dotnet.cs", $"{R}/../DatenMeister/Models/Forms/FormAndFields.dotnet.cs", true);
            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./ExcelModels.dotnet.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.dotnet.cs", true);
            File.Copy($"./ManagementProvider.class.cs",
                $"{R}/../DatenMeister/Models/ManagementProvider/ManagementProvider.class.cs", true);
            File.Copy($"./ManagementProvider.dotnet.cs",
                $"{R}/../DatenMeister/Models/ManagementProvider/ManagementProvider.dotnet.cs", true);
            File.Copy($"./FastViewFilters.class.cs",
                $"{R}/../DatenMeister/Models/FastViewFilter/FastViewFilters.class.cs", true);
            File.Copy($"./FastViewFilters.dotnet.cs",
                $"{R}/../DatenMeister/Models/FastViewFilter/FastViewFilters.dotnet.cs", true);
            File.Copy($"./DatenMeister.class.cs", $"{R}/../DatenMeister/Models/DatenMeister.class.cs", true);
#endif
        }

        private static void CreateSourceCodeForDatenMeister()
        {
            System.Console.Write("Create Sourcecode for DatenMeister...");
            using var stream = typeof(MofObject).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.XmiFiles.Types.DatenMeister.xmi");

            var document = XDocument.Load(stream);
            var pseudoProvider = new XmiProvider(document);
            var pseudoExtent = new MofUriExtent(pseudoProvider, WorkspaceNames.UriExtentInternalTypes);

            ////////////////////////////////////////
            // Creates the class tree

            // Creates the source parser which is needed to navigate through the package
            var sourceParser = new ElementSourceParser();
            var classTreeGenerator = new ClassTreeGenerator(sourceParser)
            {
                Namespace = "DatenMeister.Models"
            };

            classTreeGenerator.Walk(pseudoExtent);

            var pathOfClassTree = "DatenMeister.class.cs";
            var fileContent = classTreeGenerator.Result.ToString();
            File.WriteAllText(pathOfClassTree, fileContent);
            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceCodeForDatenMeisterAllTypes()
        {
            var dm = GiveMeDotNetCore.DatenMeister();
            
            System.Console.Write("Create Sourcecode for DatenMeister...");

            var pseudoExtent = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage).InternalTypes;

            ////////////////////////////////////////
            // Creates the class tree

            // Creates the source parser which is needed to navigate through the package
            var sourceParser = new ElementSourceParser();
            var classTreeGenerator = new ClassTreeGenerator(sourceParser)
            {
                Namespace = "DatenMeister.Models"
            };

            classTreeGenerator.Walk(pseudoExtent);

            var pathOfClassTree = "DatenMeister.class.cs";
            var fileContent = classTreeGenerator.Result.ToString();
            File.WriteAllText(pathOfClassTree, fileContent);
            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceForFastFilter()
        {
            System.Console.Write("Create Sourcecode for Fast Filter...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                    Name = "FastViewFilters",
                    Path = "./",
                    Namespace = "DatenMeister.Models.FastViewFilter",
                    Types = FastViewFilters.Types
                });

            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceForManagementProvider()
        {
            System.Console.Write("Create Sourcecode for Management Provider...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                    Name = "ManagementProvider",
                    Path = "./",
                    Namespace = "DatenMeister.Models.ManagementProviders",
                    Types = ManagementProviderModel.AllTypes
                });
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

        private static void CreateSourceForWebFields()
        {
            System.Console.Write("Create Sourcecode for Web-Fields...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                    Name = "FormAndFields",
                    Path = "./",
                    Namespace = "DatenMeister.Models.Forms",
                    Types = FieldTypes.GetAll()
                });
            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceForUmlAndMof()
        {
            var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentUml);
            var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentMof);
            var primitiveTypeExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentPrimitiveTypes);

            var loader = new SimpleLoader();
            loader.LoadFromFile(new MofFactory(umlExtent), umlExtent, "data/UML.xmi");
            loader.LoadFromFile(new MofFactory(mofExtent), mofExtent, "data/MOF.xmi");
            loader.LoadFromFile(new MofFactory(primitiveTypeExtent), primitiveTypeExtent, "data/PrimitiveTypes.xmi");

            // Generates tree for UML
            var generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Models.EMOF"
            };

            generator.Walk(umlExtent);

            /*var extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Models.EMOF._UML")
            {
                Namespace = "DatenMeister.Models.EMOF"
            };
            extentCreator.Walk(umlExtent);*/

            File.WriteAllText($"{R}/uml.cs", generator.Result.ToString());
            // File.WriteAllText($"{R}/FillTheUML.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Models.EMOF"
            };
            generator.Walk(mofExtent);

            /*7extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Models.EMOF._MOF")
            {
                Namespace = "DatenMeister.Models.EMOF"
            };

            extentCreator.Walk(mofExtent);*/

            File.WriteAllText($"{R}/mof.cs", generator.Result.ToString());
            // File.WriteAllText($"{R}/FillTheMOF.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for MOF written");

            // Generates tree for PrimitiveTypes
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Models.EMOF"
            };
            generator.Walk(primitiveTypeExtent);

            /*extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Models.EMOF._PrimitiveTypes")
            {
                Namespace = "DatenMeister.Models.EMOF"
            };

            extentCreator.Walk(primitiveTypeExtent);*/

            File.WriteAllText($"{R}/primitivetypes.cs", generator.Result.ToString());
            // File.WriteAllText($"{R}/FillThePrimitiveTypes.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for PrimitiveTypes written");
        }
    }
}

