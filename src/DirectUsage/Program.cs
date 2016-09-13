using System;
using System.Diagnostics;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Integration;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;

namespace DirectUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            var dm = GiveMe.DatenMeister();
            Console.WriteLine(dm.ToString());

            // Testing CSV
            var extent = dm.LoadCsv("files/test.csv", "dm:///csv");
            Console.WriteLine(extent.ToString());

            // Testing Excel
            var xmiExtent = dm.CreateXmiExtent("dm:///extent");
            Console.WriteLine(xmiExtent.ToString());


            var excelExtent = dm.LoadExcel("files/Quadrat.xlsx", "d:///excel");

            Console.WriteLine(excelExtent.ToString());
            foreach (var sheet in excelExtent.elements())
            {
                var sheetAsElement = (IElement) sheet;
                Console.WriteLine(sheetAsElement.get("name").ToString());
            }

            watch.Stop();

            Console.WriteLine(watch.Elapsed.ToString());

            Console.ReadKey();
        }
    }
}
