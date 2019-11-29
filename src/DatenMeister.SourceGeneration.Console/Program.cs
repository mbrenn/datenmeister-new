using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Models;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.FastViewFilter.Model;
using DatenMeister.Modules.Forms.Model;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.SourcecodeGenerator;

namespace DatenMeister.SourceGeneration.Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            // First, creates
            CreateSourceForUmlAndMof();

            System.Console.Write("Create Sourcecode for Web-Fields...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = "datenmeister:///DatenMeister/Types/FormAndFields",
                    Name = "FormAndFields",
                    Path = "./",
                    Namespace = "DatenMeister.Models.Forms",
                    Types = FieldTypes.GetAll()
                });
            System.Console.WriteLine(" Done");

            System.Console.Write("Create Sourcecode for Excel...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = "datenmeister:///DatenMeister/Types/Excel",
                    Name = "ExcelModels",
                    Path = "./",
                    Namespace = "DatenMeister.Excel",
                    Types = ExcelModels.AllTypes
                });
            System.Console.WriteLine(" Done");

            System.Console.Write("Create Sourcecode for Management Provider...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriInternalTypesExtent,
                    Name = "ManagementProvider",
                    Path = "./",
                    Namespace = "DatenMeister.Provider.ManagementProviders.Model",
                    Types = ManagementProviderModel.AllTypes
                });

            System.Console.Write("Create Sourcecode for Fast Filter...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriInternalTypesExtent,
                    Name = "FastViewFilters",
                    Path = "./",
                    Namespace = "DatenMeister.Models.FastViewFilter",
                    Types = FastViewFilters.Types
                });

            System.Console.WriteLine(" Done");

            System.Console.Write("Create Sourcecode for DataViews...");
            SourceGenerator.GenerateSourceFor(
                new SourceGeneratorOptions
                {
                    ExtentUrl = WorkspaceNames.UriInternalTypesExtent,
                    Name = "DataViews",
                    Path = "./",
                    Namespace = "DatenMeister.Models.DataViews",
                    Types = DataViewPlugin.GetTypes()
                });

            System.Console.WriteLine(" Done");

#if !DEBUG
            File.Copy("../../primitivetypes.cs", "../../../DatenMeister/Core/Filler/primitivetypes.cs", true);
            File.Copy("../../FillThePrimitiveTypes.cs", "../../../DatenMeister/Core/Filler/FillThePrimitiveTypes.cs", true);
            File.Copy("../../mof.cs", "../../../DatenMeister/Core/Filler/mof.cs", true);
            File.Copy("../../FillTheMOF.cs", "../../../DatenMeister/Core/Filler/FillTheMOF.cs", true);
            File.Copy("../../uml.cs", "../../../DatenMeister/Core/Filler/uml.cs", true);
            File.Copy("../../FillTheUML.cs", "../../../DatenMeister/Core/Filler/FillTheUML.cs", true);

            File.Copy("./FormAndFields.filler.cs", "../../../DatenMeister/Models/Forms/FormAndFields.filler.cs", true);
            File.Copy("./FormAndFields.class.cs", "../../../DatenMeister/Models/Forms/FormAndFields.class.cs", true);
            File.Copy("./FormAndFields.dotnet.cs", "../../../DatenMeister/Models/Forms/FormAndFields.dotnet.cs", true);

            File.Copy("./ExcelModels.filler.cs", "../../../DatenMeister.Excel/Models/ExcelModels.filler.cs", true);
            File.Copy("./ExcelModels.class.cs", "../../../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy("./ExcelModels.dotnet.cs", "../../../DatenMeister.Excel/Models/ExcelModels.dotnet.cs", true);

            File.Copy("./ManagementProvider.filler.cs", "../../../DatenMeister/Models/ManagementProvider/ManagementProvider.filler.cs", true);
            File.Copy("./ManagementProvider.class.cs", "../../../DatenMeister/Models/ManagementProvider/ManagementProvider.class.cs", true);
            File.Copy("./ManagementProvider.dotnet.cs", "../../../DatenMeister/Models/ManagementProvider/ManagementProvider.dotnet.cs", true);

            File.Copy("./FastViewFilters.filler.cs", "../../../DatenMeister/Models/FastViewFilter/FastViewFilters.filler.cs", true);
            File.Copy("./FastViewFilters.class.cs", "../../../DatenMeister/Models/FastViewFilter/FastViewFilters.class.cs", true);
            File.Copy("./FastViewFilters.dotnet.cs", "../../../DatenMeister/Models/FastViewFilter/FastViewFilters.dotnet.cs", true);

            File.Copy("./DataViews.filler.cs", "../../../DatenMeister/Models/DataViews/DataViews.filler.cs", true);
            File.Copy("./DataViews.class.cs", "../../../DatenMeister/Models/DataViews/DataViews.class.cs", true);
            File.Copy("./DataViews.dotnet.cs", "../../../DatenMeister/Models/DataViews/DataViews.dotnet.cs", true);

#endif
        }

        private static void CreateSourceForUmlAndMof()
        {
            var umlExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriUmlExtent);
            var mofExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriMofExtent);
            var primitiveTypeExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriPrimitiveTypesExtent);

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

            File.WriteAllText("../../uml.cs", generator.Result.ToString());
            File.WriteAllText("../../FillTheUML.cs", extentCreator.Result.ToString());
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

            File.WriteAllText("../../mof.cs", generator.Result.ToString());
            File.WriteAllText("../../FillTheMOF.cs", extentCreator.Result.ToString());
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

            File.WriteAllText("../../primitivetypes.cs", generator.Result.ToString());
            File.WriteAllText("../../FillThePrimitiveTypes.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for PrimitiveTypes written");
        }
    }
}

