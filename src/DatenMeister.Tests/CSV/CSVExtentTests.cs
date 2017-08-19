using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.XMI.EMOF;
using NUnit.Framework;

namespace DatenMeister.Tests.CSV
{
    [TestFixture]
    public class CSVExtentTests
    {
        [Test]
        public void TestStorage()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            var csvOtherFile = "eens 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText("data.txt", csvFile);

            var storageConfiguration = new CSVExtentLoaderConfig
            {
                Path = "data.txt",
                ExtentUri = "dm:///test",
                Settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            var storage = new CSVExtentLoader(null);
            var provider = storage.LoadExtent(storageConfiguration, false);
            var extent = new MofUriExtent(provider, "dm:////test/");

            
            Assert.That(storageConfiguration.Settings.Columns.Count, Is.EqualTo(3));
            Assert.That(extent.elements().Count(), Is.EqualTo(4));

            // Stores the csv file
            storage.StoreExtent(provider, storageConfiguration);
            var readCsvFile = File.ReadAllText("data.txt");

            Assert.That(readCsvFile, Is.EqualTo(csvFile));

            var firstElement = extent.elements().ElementAt(0) as IObject;
            Assert.That(firstElement, Is.Not.Null);
            firstElement.set(storageConfiguration.Settings.Columns[0], "eens");
            storage.StoreExtent(provider, storageConfiguration);
            readCsvFile = File.ReadAllText("data.txt");
            Assert.That(readCsvFile, Is.EqualTo(csvOtherFile));

            File.Delete("data.txt");
        }
    }
}