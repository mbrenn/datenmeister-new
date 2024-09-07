using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.CSV.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
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
                Path.GetDirectoryName(Assembly.GetAssembly(typeof(CSVExtentTests))!.Location)!,
                "data.txt");

        [Test]
        public async Task TestStorage()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            var csvOtherFile = "eens 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText(PathForTemporaryDataFile, csvFile);

            var settings = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvSettings);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader, false);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator, ' ');

            var storageConfiguration = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///test");
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath,
                PathForTemporaryDataFile);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings, settings);

            var storage = new CsvProviderLoader
            {
                WorkspaceLogic = WorkspaceLogic.GetEmptyLogic()
            };
            
            var provider = await storage.LoadProvider(storageConfiguration, ExtentCreationFlags.LoadOnly);
            var extent = new MofUriExtent(provider.Provider, "dm:////test/", null);

            var csvSettings = storageConfiguration
                .getOrDefault<IElement>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings);
            Assert.That(settings, Is.Not.Null);
            var columns =
                csvSettings.getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings
                    .columns);

            Assert.That(columns.Count(), Is.EqualTo(3));
            Assert.That(extent.elements().Count(), Is.EqualTo(4));

            // Stores the csv file
            await storage.StoreProvider(provider.Provider, storageConfiguration);
            var readCsvFile = File.ReadAllText(PathForTemporaryDataFile);

            // We need to change a bit the line endings since the tests are also required to within Linux 
            Assert.That(
                readCsvFile.Replace("\r\n", "\n").Replace("\n\n", "\n"),
                Is.EqualTo(csvFile.Replace("\r\n", "\n").Replace("\n\n", "\n")));

            var firstElement = extent.elements().ElementAt(0) as IObject;
            Assert.That(firstElement, Is.Not.Null);

            Assert.That(storageConfiguration
                    .getOrDefault<IElement>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings)
                    .getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns)
                    .ElementAt(0),
                Is.EqualTo("Column 1"));

            firstElement!.set("Column 1", "eens");

            Assert.That(firstElement.getOrDefault<string>("Column 1"), Is.EqualTo("eens"));

            await storage.StoreProvider(provider.Provider, storageConfiguration);
            readCsvFile = File.ReadAllText(PathForTemporaryDataFile);
            // We need to change a bit the line endings since the tests are also required to within Linux 
            Assert.That(
                readCsvFile.Replace("\r\n", "\n").Replace("\n\n", "\n"),
                Is.EqualTo(csvOtherFile.Replace("\r\n", "\n").Replace("\n\n", "\n")));

            File.Delete(PathForTemporaryDataFile);
        }

        [Test]
        public void TestTrimming()
        {
            var csvFile = "Name , Vorname\r\n Martin,Brenn \r\nAndi , Köpke ";

            /// Step 1: Load the data
            // Does nothing... Test stub
            // Use the CSV Importer
            var data = new InMemoryProvider();
            var csvLoader = new CsvLoader(null);

            // Convert the string to a stream
            byte[] byteArray = Encoding.UTF8.GetBytes(csvFile);
            var stream = new MemoryStream(byteArray);

            // Configures the CSV Import
            var settings = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvSettings);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader, true);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.encoding, "UTF-8");
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator, ",");

            // Now, do the import
            csvLoader.Load(data, stream, settings);

            // Test the data
            var firstElement = data.GetRootObjects().ElementAt(0);
            Assert.That(firstElement, Is.Not.Null);
            Assert.That(firstElement.GetProperty("Name "), Is.EqualTo(" Martin"));
            Assert.That(firstElement.GetProperty(" Vorname"), Is.EqualTo("Brenn "));

            var secondElement = data.GetRootObjects().ElementAt(1);
            Assert.That(secondElement, Is.Not.Null);
            Assert.That(secondElement.GetProperty("Name "), Is.EqualTo("Andi "));
            Assert.That(secondElement.GetProperty(" Vorname"), Is.EqualTo(" Köpke "));

            /////
            // Now perform the test with activated trimming
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.trimCells, true);
            data = new InMemoryProvider();
            stream = new MemoryStream(byteArray);
            csvLoader.Load(data, stream, settings);

            // Test the data
            firstElement = data.GetRootObjects().ElementAt(0);
            Assert.That(firstElement, Is.Not.Null);
            Assert.That(firstElement.GetProperty("Name"), Is.EqualTo("Martin"));
            Assert.That(firstElement.GetProperty("Vorname"), Is.EqualTo("Brenn"));

            secondElement = data.GetRootObjects().ElementAt(1);
            Assert.That(secondElement, Is.Not.Null);
            Assert.That(secondElement.GetProperty("Name"), Is.EqualTo("Andi"));
            Assert.That(secondElement.GetProperty("Vorname"), Is.EqualTo("Köpke"));

        }
    }
}