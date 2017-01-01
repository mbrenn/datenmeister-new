using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Integration;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Aggregation;
using DatenMeister.Runtime.Functions.Interfaces;
using DatenMeister.Runtime.Functions.Transformation;

namespace DirectUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            
            watch.Start();
            var dm = GiveMe.DatenMeister(new IntegrationSettings { PerformSlimIntegration = false });
            Console.WriteLine($"Slim: {watch.Elapsed}");
            
            Console.WriteLine(dm.ToString());

            // Testing CSV
            var extent = dm.LoadCsv("dm:///csv", "files/test.csv");
            Console.WriteLine(extent.ToString());

            // Testing Excel
            var xmiExtent = dm.CreateXmiExtent("dm:///extent");
            Console.WriteLine(xmiExtent.ToString());
            /*
            var excelExtent = dm.LoadExcel("d:///excel", "files/Quadratzahlen.xlsx");

            Console.WriteLine(excelExtent.ToString());
            foreach (var sheet in excelExtent.elements())
            {
                var sheetAsElement = (IElement) sheet;
                var allProperties = (IObjectAllProperties) sheetAsElement.GetAsEnumerable("items").First();
                Console.WriteLine("Table:" + sheetAsElement.get("name"));
                foreach (var property in allProperties.getPropertiesBeingSet())
                {
                    Console.WriteLine("Property: " + property);
                }

                foreach (var item in (IEnumerable) sheetAsElement.get("items"))
                {
                    var itemAsElement = (IElement) item;
                    // Console.WriteLine(itemAsElement.get("Wert") + ": " + itemAsElement.get("Quadratzahl"));
                }
            }

            //var excelFunctions = dm.LoadExcel("d:///excel", "files/Functions.xlsx");
            var mofTarget = new UriExtent(new InMemoryProvider());//("dm:///"));
            HierarchyMaker.Convert(new HierarchyByParentSettings()
            {
                Sequence = ((IElement) excelFunctions.elements().GetByPropertyFromCollection("name", "Per Parent").First()).get("items") as IReflectiveSequence,
                TargetSequence  =  mofTarget.elements(),
                TargetFactory =  new InMemoryFactory(),
                NewChildColumn = "Parts",
                IdColumn = "Id", 
                OldParentColumn = "Parent"
            });

            foreach (var element in mofTarget.elements())
            {
                Console.WriteLine(element.ToString());
            }

            mofTarget = new UriExtent(new InMemoryProvider());//("dm:///"));
            HierarchyMaker.Convert(new HierarchyByChildrenSettings()
            {
                Sequence = ((IElement)excelFunctions.elements().GetByPropertyFromCollection("name", "Per Child").First()).get("items") as IReflectiveSequence,
                TargetSequence = mofTarget.elements(),
                TargetFactory = new InMemoryFactory(),
                NewChildColumn = "Parts",
                IdColumn = "Id",
                OldChildrenColumn = "Child",
                ChildIdSeparator = ","
            });

            foreach (var element in mofTarget.elements())
            {
                Console.WriteLine(element.ToString());
            }

            mofTarget = new InMemoryUriExtent("dm:///");
            HierarchyMaker.Convert(new HierarchyByParentSettings()
            {
                Sequence = ((IElement)excelFunctions.elements().GetByPropertyFromCollection("name", "Länder").First()).get("items") as IReflectiveSequence,
                TargetSequence = mofTarget.elements(),
                TargetFactory = new InMemoryFactory(),
                NewChildColumn = "Länder",
                IdColumn = "Id",
                OldParentColumn = "Parent"
            });

            var foundElement = mofTarget.element("dm:///#Länder.1");

            var enumeration = foundElement.GetAsReflectiveCollection("Länder");
            var aggregate = new GroupByReflectiveCollection(
                enumeration,
                "Regierung",
                new[] { "Name", "Id" },
                new Func<IAggregator>[] {() => new ConcatAggregator(), () => new CountAggregator() },
                new[] { "Regiert", "Anzahl" });


            Console.WriteLine(TableFormatter.ToText(aggregate));

            Console.WriteLine("Waiting for Key...");
            Console.ReadKey();

            watch.Stop();
            */
        }
    }
}
