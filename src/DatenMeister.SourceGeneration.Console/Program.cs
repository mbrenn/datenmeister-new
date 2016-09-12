using System.IO;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Excel.Models;
using DatenMeister.Models.Forms;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.XMI;

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
                    Name = "ExcelModels",
                    Path = "./",
                    Namespace = "DatenMeister.Excel",
                    Types = ExcelModels.AllTypes
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
            File.Copy("./FormAndFields.dotnet.cs", "../../../DatenMeister/Integration/Modules/FormAndFields.dotnet.cs", true);
            
            File.Copy("./ExcelModels.filler.cs", "../../../DatenMeister.Excel/Models/ExcelModels.filler.cs", true);
            File.Copy("./ExcelModels.class.cs", "../../../DatenMeister.Excel/Models/ExcelModels.class.cs", true);
            File.Copy("./ExcelModels.dotnet.cs", "../../../DatenMeister.Excel/Models/ExcelModels.dotnet.cs", true);
#endif
        }

        private static void CreateSourceForUmlAndMof()
        {
            var factory = new MofFactory();
            var umlExtent = new MofUriExtent(Locations.UriUml);
            var mofExtent = new MofUriExtent(Locations.UriMof);
            var primitiveTypeExtent = new MofUriExtent(Locations.UriPrimitiveTypes);
            var loader = new SimpleLoader(factory);
            loader.LoadFromFile(umlExtent, "data/UML.xmi");
            loader.LoadFromFile(mofExtent, "data/MOF.xmi");
            loader.LoadFromFile(primitiveTypeExtent, "data/PrimitiveTypes.xmi");

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

