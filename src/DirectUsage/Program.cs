using System;
using System.Diagnostics;
using DatenMeister.Integration;

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

            watch.Stop();

            Console.WriteLine(watch.Elapsed.ToString());

            Console.ReadKey();
        }
    }
}
