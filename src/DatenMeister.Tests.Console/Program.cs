using DatenMeister.CSV;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Queries;
using DatenMeister.XMI.UmlBootstrap;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;
using DatenMeister.EMOF.Helper;
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
            TestUmlBootstrap();
            TestFillTree();

            System.Console.ReadKey();
        }

        private static void TestZipCodes()
        {
            // Checks the loading of the PLZ
            System.Console.WriteLine("Loading the Zip codes");

            var extent = new MofUriExtent("mof:///plz");
            var factory = new MofFactory();

            var csvSettings = new CSVSettings();
            csvSettings.Encoding = Encoding.GetEncoding("ISO-8859-1");
            csvSettings.Separator = '\t';
            csvSettings.HasHeader = false;

            var provider = new CSVDataProvider();
            provider.Load(extent, factory, "data/plz.csv", csvSettings);

            System.Console.WriteLine($"Loaded: {extent.elements().Count().ToString()} Zipcodes");

            System.Console.WriteLine();
            System.Console.WriteLine("----");
        }

        private static void TestUmlBootstrap()
        {
            System.Console.WriteLine("Testing Uml-Bootstrap.");

            var watch = new Stopwatch();
            watch.Start();
            var fullStrap = Bootstrapper.PerformFullBootstrap("data/UML.xmi");
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
            var factory = new MofFactory();
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var loader = new SimpleLoader(factory);
            loader.Load(mofExtent, "data/MOF.xmi");

            var mof = new _MOF();
            FillTheMOF.DoFill(mofExtent.elements(), mof);
            

        }
    }
}
