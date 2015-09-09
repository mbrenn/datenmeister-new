using DatenMeister.CSV;
using DatenMeister.MOF.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var property = new object();
            System.Console.WriteLine("DatenMeister Testing");

            var element = new MOF.InMemory.MofElement();
            element.set(property, "Test");



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

            System.Console.ReadKey();
        }

    }
}
