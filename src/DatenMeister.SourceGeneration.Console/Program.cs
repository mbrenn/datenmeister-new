using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.XMI;
using DatenMeister.XMI.UmlBootstrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            generator.CreateClassTree(umlExtent);

            File.WriteAllText("../../uml.cs", generator.Result.ToString());
            Console.WriteLine("C# Code for UML written");

            // Generates tree for MOF
            generator = new ClassTreeGenerator();
            generator.CreateClassTree(mofExtent);

            File.WriteAllText("../../mof.cs", generator.Result.ToString());
            Console.WriteLine("C# Code for MOF written");
        }
    }
}

