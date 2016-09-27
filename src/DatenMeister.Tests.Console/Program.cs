using System.Diagnostics;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Helper;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml;

namespace DatenMeister.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var property = "Prop";
            System.Console.WriteLine("DatenMeister Testing");

            var element = new MofElement();
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

            System.Console.WriteLine($"Loaded: {extent.elements().Count()} Zipcodes");

            System.Console.WriteLine();
        }

        private static void TestUmlBootstrap()
        {
            System.Console.WriteLine("Testing Uml-Bootstrap.");

            var watch = new Stopwatch();
            watch.Start();

            var dataLayerLogic = new WorkspaceLogic(new WorkspaceData());
            var fullStrap = Bootstrapper.PerformFullBootstrap(dataLayerLogic,
                null, 
                BootstrapMode.Mof,
                new Bootstrapper.FilePaths
                {
                    PathPrimitive = "data/PrimitiveTypes.xmi",
                    PathUml = "data/UML.xmi",
                    PathMof = "data/MOF.xmi"
                });
            watch.Stop();

            var descendents = AllDescendentsQuery.GetDescendents(fullStrap.UmlInfrastructure);
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

            System.Console.WriteLine($"Elapsed Time for Bootstrap {watch.ElapsedMilliseconds:n0} ms");
        }

        private static void TestFillTree()
        {
            var watch = new Stopwatch();
            watch.Start();
            var factory = new MofFactory();
            var mofExtent = new MofUriExtent(WorkspaceNames.UriMof);
            var umlExtent = new MofUriExtent(WorkspaceNames.UriUml);
            var loader = new SimpleLoader(factory);
            loader.LoadFromFile(mofExtent, "data/MOF.xmi");
            loader.LoadFromFile(mofExtent, "data/UML.xmi");

            var mof = new _MOF();
            var uml = new _UML();
            FillTheMOF.DoFill(mofExtent.elements(), mof);
            FillTheUML.DoFill(umlExtent.elements(), uml);

            watch.Stop();
            System.Console.WriteLine($"Elapsed Time for MOF and UML Fill {watch.ElapsedMilliseconds:n0} ms");
        }
    }
}
