using DatenMeister.CSV;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Queries;
using DatenMeister.XMI.UmlBootstrap;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DatenMeister.XMI;
using DatenMeister.Filler;

namespace DatenMeister.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var property = new object();
            System.Console.WriteLine("DatenMeister Testing");

            var element = new EMOF.InMemory.MofElement();
            element.set(property, "Test");

            TestZipCodes();
            System.Console.WriteLine("----");
            TestUmlBootstrap();
            System.Console.WriteLine("----");
            TestFillTree();
            System.Console.WriteLine("----");
            System.Console.WriteLine("Please press key.");
            System.Console.ReadKey();
        }

        private static void TestZipCodes()
        {
            // Checks the loading of the PLZ
            System.Console.WriteLine("Loading the Zip codes");

            var extent = new MofUriExtent("mof:///plz");
            var factory = new MofFactory();

            var csvSettings = new CSVSettings
            {
                Encoding = "ISO-8859-1",
                Separator = '\t',
                HasHeader = false
            };

            var provider = new CSVDataProvider(null);
            provider.Load(extent, factory, "data/plz.csv", csvSettings);

            System.Console.WriteLine($"Loaded: {extent.elements().Count().ToString()} Zipcodes");

            System.Console.WriteLine();
        }

        private static void TestUmlBootstrap()
        {
            System.Console.WriteLine("Testing Uml-Bootstrap.");

            var watch = new Stopwatch();
            watch.Start();
            var fullStrap = Bootstrapper.PerformFullBootstrap("data/PrimitiveTypes", "data/UML.xmi", "data/MOF.xmi");
            watch.Stop();

            var descendents = AllDescendentsQuery.getDescendents(fullStrap.UmlInfrastructure);
            System.Console.WriteLine($"Having {descendents.Count()} elements");
            var n = 0;
            foreach (var child in descendents)
            {
                if (child.isSet("name"))
                {
                    n++;
                }
            }

            System.Console.WriteLine($"Having {n} elements with name");

            System.Console.WriteLine($"Elapsed Time for Bootstrap {watch.ElapsedMilliseconds.ToString("n0")} ms");
        }

        private static void TestFillTree()
        {
            var watch = new Stopwatch();
            watch.Start();
            var factory = new MofFactory();
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var umlExtent = new MofUriExtent("datenmeister:///uml");
            var loader = new SimpleLoader(factory);
            loader.Load(mofExtent, "data/MOF.xmi");
            loader.Load(mofExtent, "data/UML.xmi");

            var mof = new _MOF();
            var uml = new _UML();
            FillTheMOF.DoFill(mofExtent.elements(), mof);
            FillTheUML.DoFill(umlExtent.elements(), uml);

            watch.Stop();
            System.Console.WriteLine($"Elapsed Time for MOF and UML Fill {watch.ElapsedMilliseconds.ToString("n0")} ms");
        }
    }
}
