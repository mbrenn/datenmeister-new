using DatenMeister.EMOF.InMemory;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.XMI;
using System;
using System.IO;

namespace DatenMeister.SourceGeneration
{
    class Program
    {
        public static void Main(string[] args)
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
            Console.WriteLine("C# Code for UML written");

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
            Console.WriteLine("C# Code for MOF written");

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
            Console.WriteLine("C# Code for PrimitiveTypes written");

#if !DEBUG
            File.Copy("../../primitivetypes.cs", "../../../DatenMeister/Filler/primitivetypes.cs", true);
            File.Copy("../../FillThePrimitiveTypes.cs", "../../../DatenMeister/Filler/FillThePrimitiveTypes.cs", true);
            File.Copy("../../mof.cs", "../../../DatenMeister/Filler/mof.cs", true);
            File.Copy("../../FillTheMOF.cs", "../../../DatenMeister/Filler/FillTheMOF.cs", true);
            File.Copy("../../uml.cs", "../../../DatenMeister/Filler/uml.cs", true);
            File.Copy("../../FillTheUML.cs", "../../../DatenMeister/Filler/FillTheUML.cs", true);
#endif
        }
    }
}

