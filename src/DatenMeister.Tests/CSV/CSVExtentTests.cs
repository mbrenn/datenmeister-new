using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
        /// <summary>
        /// Gets the path for the temporary datafile
        /// </summary>
        public static string PathForTemporaryDataFile =>
            Path.Combine(
                // ReSharper disable once AssignNullToNotNullAttribute
                Path.GetDirectoryName(Assembly.GetAssembly(typeof(CSVExtentTests)).Location),
                "data.txt");

        [Test]
        public void TestStorage()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            var csvOtherFile = "eens 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText(PathForTemporaryDataFile, csvFile);

            var storageConfiguration = new CSVExtentLoaderConfig
            {
                Path = PathForTemporaryDataFile,
                ExtentUri = "datenmeister:///test",
                Settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            var storage = new CSVExtentLoader(null);
            var provider = storage.LoadExtent(storageConfiguration, false);
            var extent = new MofUriExtent(provider, "datenmeister:////test/");
            
            Assert.That(storageConfiguration.Settings.Columns.Count, Is.EqualTo(3));
            Assert.That(extent.elements().Count(), Is.EqualTo(4));

            // Stores the csv file
            storage.StoreExtent(provider, storageConfiguration);
            var readCsvFile = File.ReadAllText(PathForTemporaryDataFile);

            Assert.That(readCsvFile, Is.EqualTo(csvFile));

            var firstElement = extent.elements().ElementAt(0) as IObject;
            Assert.That(firstElement, Is.Not.Null);
            firstElement.set(storageConfiguration.Settings.Columns[0], "eens");
            storage.StoreExtent(provider, storageConfiguration);
            readCsvFile = File.ReadAllText(PathForTemporaryDataFile);
            Assert.That(readCsvFile, Is.EqualTo(csvOtherFile));

            File.Delete(PathForTemporaryDataFile);
        }
    }
}