using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Integration;
using DatenMeister.Integration;
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

            var excelExtent = dm.LoadExcel("d:///excel", "files/Quadratzahlen.xlsx");

            Console.WriteLine(excelExtent.ToString());
            foreach (var sheet in excelExtent.elements())
            {
                var sheetAsElement = (IElement) sheet;
                var allProperties = ((IEnumerable<object>) sheetAsElement.get("items")).First() as IObjectAllProperties;
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


            var excelFunctions = dm.LoadExcel("d:///excel", "files/Functions.xlsx");
            var mofTarget = new MofUriExtent("dm:///");
            HierarchyMaker.Convert(new HierarchyMakerSettings()
            {
                Sequence = ((IElement) excelFunctions.elements().First()).get("items") as IReflectiveSequence,
                TargetSequence  =  mofTarget.elements(),
                TargetFactory =  new MofFactory(),
                NewChildColumn = "Parts",
                OldIdColumn = "Id", 
                OldParentColumn = "Parent"
            });

            foreach (var element in mofTarget.elements())
            {
                Console.WriteLine(element.ToString());
            }

            Console.ReadKey();

            watch.Stop();

        }
    }
}
