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
            var umlExtent = new MofUriExtent("datenmeister:///uml");
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var loader = new SimpleLoader(factory);
            loader.Load(umlExtent, "data/UML.xmi");
            loader.Load(mofExtent, "data/MOF.xmi");

            // Generates tree for UML
            var generator = new ClassTreeGenerator();
            generator.Namespace = "DatenMeister";
            generator.Walk(umlExtent);

            var extentCreator = new FillClassTreeByExtentCreator("DatenMeister._UML");
            extentCreator.Namespace = "DatenMeister.Filler";
            extentCreator.Walk(umlExtent);

            File.WriteAllText("../../uml.cs", generator.Result.ToString());
            File.WriteAllText("../../FillTheUML.cs", extentCreator.Result.ToString());
            Console.WriteLine("C# Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator();
            generator.Namespace = "DatenMeister";
            generator.Walk(mofExtent);

            extentCreator = new FillClassTreeByExtentCreator("DatenMeister._MOF");
            extentCreator.Namespace = "DatenMeister.Filler";
            extentCreator.Walk(mofExtent);

            File.WriteAllText("../../mof.cs", generator.Result.ToString());
            File.WriteAllText("../../FillTheMOF.cs", extentCreator.Result.ToString());
            Console.WriteLine("C# Code for MOF written");
        }
    }
}

