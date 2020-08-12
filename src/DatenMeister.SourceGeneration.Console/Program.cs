using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Models;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Models.Forms;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.DataViews;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.SourcecodeGenerator;

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

            CreateSourceForDataViews();

            CreateSourceForReports();

#if !DEBUG
            File.Copy($"{R}/primitivetypes.cs", $"{R}/../DatenMeister/Core/Filler/primitivetypes.cs", true);
            File.Copy($"{R}/FillThePrimitiveTypes.cs", $"{R}/../DatenMeister/Core/Filler/FillThePrimitiveTypes.cs",
                true);
            File.Copy($"{R}/mof.cs", $"{R}/../DatenMeister/Core/Filler/mof.cs", true);
            File.Copy($"{R}/FillTheMOF.cs", $"{R}/../DatenMeister/Core/Filler/FillTheMOF.cs", true);
            File.Copy($"{R}/uml.cs", $"{R}/../DatenMeister/Core/Filler/uml.cs", true);
            File.Copy($"{R}/FillTheUML.cs", $"{R}/../DatenMeister/Core/Filler/FillTheUML.cs", true);

            File.Copy($"./FormAndFields.filler.cs", $"{R}/../DatenMeister/Models/Forms/FormAndFields.filler.cs",
                true);
            File.Copy($"./FormAndFields.class.cs", $"{R}/../DatenMeister/Models/Forms/FormAndFields.class.cs", true);
            File.Copy($"./FormAndFields.dotnet.cs", $"{R}/../DatenMeister/Models/Forms/FormAndFields.dotnet.cs",
                true);

            File.Copy($"./ExcelModels.filler.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.filler.cs", true);
            File.Copy($"./ExcelModels.class.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy($"./ExcelModels.dotnet.cs", $"{R}/../DatenMeister.Excel/Models/ExcelModels.dotnet.cs", true);

            File.Copy($"./ManagementProvider.filler.cs",
                $"{R}/../DatenMeister/Models/ManagementProvider/ManagementProvider.filler.cs", true);
            File.Copy($"./ManagementProvider.class.cs",
                $"{R}/../DatenMeister/Models/ManagementProvider/ManagementProvider.class.cs", true);
            File.Copy($"./ManagementProvider.dotnet.cs",
                $"{R}/../DatenMeister/Models/ManagementProvider/ManagementProvider.dotnet.cs", true);

            File.Copy($"./FastViewFilters.filler.cs",
                $"{R}/../DatenMeister/Models/FastViewFilter/FastViewFilters.filler.cs", true);
            File.Copy($"./FastViewFilters.class.cs",
                $"{R}/../DatenMeister/Models/FastViewFilter/FastViewFilters.class.cs", true);
            File.Copy($"./FastViewFilters.dotnet.cs",
                $"{R}/../DatenMeister/Models/FastViewFilter/FastViewFilters.dotnet.cs", true);

            File.Copy($"./DataViews.filler.cs", $"{R}/../DatenMeister/Models/DataViews/DataViews.filler.cs", true);
            File.Copy($"./DataViews.class.cs", $"{R}/../DatenMeister/Models/DataViews/DataViews.class.cs", true);
            File.Copy($"./DataViews.dotnet.cs", $"{R}/../DatenMeister/Models/DataViews/DataViews.dotnet.cs", true);

            File.Copy($"./Reports.filler.cs", $"{R}/../DatenMeister/Models/Reports/Reports.filler.cs", true);
            File.Copy($"./Reports.class.cs", $"{R}/../DatenMeister/Models/Reports/Reports.class.cs", true);
            File.Copy($"./Reports.dotnet.cs", $"{R}/../DatenMeister/Models/Reports/Reports.dotnet.cs", true);

#endif
        }

        private static void CreateSourceForReports()
        {
            System.Console.Write("Create Sourcecode for Reports...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                    Name = "Reports",
                    Path = "./",
                    Namespace = "DatenMeister.Models.Reports",
                    Types = ReportTypes.GetTypes()
                });

            System.Console.WriteLine(" Done");
        }

        private static void CreateSourceForDataViews()
        {
            System.Console.Write("Create Sourcecode for DataViews...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriExtentInternalTypes,
                    Name = "DataViews",
                    Path = "./",
                    Namespace = "DatenMeister.Models.DataViews",
                    Types = DataViewPlugin.GetTypes()
                });

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
                    Namespace = "DatenMeister.Provider.ManagementProviders.Model",
                    Types = ManagementProviderModel.AllTypes
                });
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
                    Namespace = "DatenMeister.Excel",
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
                Namespace = "DatenMeister.Core"
            };

            generator.Walk(umlExtent);

            var extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Core._UML")
            {
                Namespace = "DatenMeister.Core.Filler"
            };
            extentCreator.Walk(umlExtent);

            File.WriteAllText($"{R}/uml.cs", generator.Result.ToString());
            File.WriteAllText($"{R}/FillTheUML.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core"
            };
            generator.Walk(mofExtent);

            extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Core._MOF")
            {
                Namespace = "DatenMeister.Core.Filler"
            };

            extentCreator.Walk(mofExtent);

            File.WriteAllText($"{R}/mof.cs", generator.Result.ToString());
            File.WriteAllText($"{R}/FillTheMOF.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for MOF written");

            // Generates tree for PrimitiveTypes
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister.Core"
            };
            generator.Walk(primitiveTypeExtent);

            extentCreator = new FillClassTreeByExtentCreator("DatenMeister.Core._PrimitiveTypes")
            {
                Namespace = "DatenMeister.Core.Filler"
            };

            extentCreator.Walk(primitiveTypeExtent);

            File.WriteAllText($"{R}/primitivetypes.cs", generator.Result.ToString());
            File.WriteAllText($"{R}/FillThePrimitiveTypes.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for PrimitiveTypes written");
        }
    }
}

