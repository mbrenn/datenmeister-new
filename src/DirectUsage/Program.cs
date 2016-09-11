using System;
using System.Diagnostics;
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

            var extent = dm.LoadCsv("files/test.csv", "dm:///csv");
            Console.WriteLine(extent.ToString());

            var xmiExtent = dm.CreateXmiExtent("dm:///extent");
            Console.WriteLine();

            watch.Stop();

            Console.WriteLine(watch.Elapsed.ToString());

            Console.ReadKey();
        }
    }
}
