using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.SourcecodeGenerator;
using DatenMeister.XMI.UmlBootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.SourceGeneration
{
    class Program
    {
        public static void Main(string[] args)
        {
            var strapper = Bootstrapper.PerformFullBootstrap("data/UML.xmi");

            foreach (var element in strapper.UmlInfrastructure.elements().Cast<IObject>())
            {
                var generator = new ClassTreeGenerator();
                generator.CreateClassTree(element);
                Console.WriteLine(generator.Result.ToString());

            }

            Console.ReadKey();
        }
    }
}
