using System.IO;
using DatenMeister.EMOF.InMemory;
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
#if !DEBUG
            File.Copy("../../primitivetypes.cs", "../../../DatenMeister/Filler/primitivetypes.cs", true);
            File.Copy("../../FillThePrimitiveTypes.cs", "../../../DatenMeister/Filler/FillThePrimitiveTypes.cs", true);
            File.Copy("../../mof.cs", "../../../DatenMeister/Filler/mof.cs", true);
            File.Copy("../../FillTheMOF.cs", "../../../DatenMeister/Filler/FillTheMOF.cs", true);
            File.Copy("../../uml.cs", "../../../DatenMeister/Filler/uml.cs", true);
            File.Copy("../../FillTheUML.cs", "../../../DatenMeister/Filler/FillTheUML.cs", true);
            
            File.Copy("./FormAndFields.filler.cs", "../../../DatenMeister.Web.Models/Forms/FormAndFields.filler.cs", true);
            File.Copy("./FormAndFields.class.cs", "../../../DatenMeister.Web.Models/Forms/FormAndFields.class.cs", true);
            File.Copy("./FormAndFields.dotnet.cs", "../../../DatenMeister.Integration/Modules/FormAndFields.dotnet.cs", true);
#endif
        }

        private static void CreateSourceForUmlAndMof()
        {
            var factory = new MofFactory();
            var umlExtent = new MofUriExtent(Locations.UriUml);
            var mofExtent = new MofUriExtent(Locations.UriMof);
            var primitiveTypeExtent = new MofUriExtent(Locations.UriPrimitiveTypes);
            var loader = new SimpleLoader(factory);
            loader.Load(umlExtent, "data/UML.xmi");
            loader.Load(mofExtent, "data/MOF.xmi");
            loader.Load(primitiveTypeExtent, "data/PrimitiveTypes.xmi");

            // Generates tree for UML
            var generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister"
            };

            generator.Walk(umlExtent);

            var extentCreator = new FillClassTreeByExtentCreator("DatenMeister._UML")
            {
                Namespace = "DatenMeister.Filler"
            };
            extentCreator.Walk(umlExtent);

            File.WriteAllText("../../uml.cs", generator.Result.ToString());
            File.WriteAllText("../../FillTheUML.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister"
            };
            generator.Walk(mofExtent);

            extentCreator = new FillClassTreeByExtentCreator("DatenMeister._MOF")
            {
                Namespace = "DatenMeister.Filler"
            };

            extentCreator.Walk(mofExtent);

            File.WriteAllText("../../mof.cs", generator.Result.ToString());
            File.WriteAllText("../../FillTheMOF.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for MOF written");

            // Generates tree for PrimitiveTypes
            generator = new ClassTreeGenerator
            {
                Namespace = "DatenMeister"
            };
            generator.Walk(primitiveTypeExtent);

            extentCreator = new FillClassTreeByExtentCreator("DatenMeister._PrimitiveTypes")
            {
                Namespace = "DatenMeister.Filler"
            };

            extentCreator.Walk(primitiveTypeExtent);

            File.WriteAllText("../../primitivetypes.cs", generator.Result.ToString());
            File.WriteAllText("../../FillThePrimitiveTypes.cs", extentCreator.Result.ToString());
            System.Console.WriteLine("C# Code for PrimitiveTypes written");
        }
    }
}

