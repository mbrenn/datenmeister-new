using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
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

            var storageConfiguration = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///test");
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath, PathForTemporaryDataFile);
            /*configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///test");*/
            
            throw new NotImplementedException();

            var storageConfiguration2 = new CsvExtentLoaderConfig("dm:///test")
            {
                filePath = PathForTemporaryDataFile,
                settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };
            
            /*            
            var storage = new CsvProviderLoader
            {
                WorkspaceLogic = WorkspaceLogic.GetEmptyLogic()
            };
            var provider = storage.LoadProvider(storageConfiguration, ExtentCreationFlags.LoadOnly);
            var extent = new MofUriExtent(provider.Provider, "dm:////test/");

            Assert.That(storageConfiguration.settings.Columns.Count, Is.EqualTo(3));
            Assert.That(extent.elements().Count(), Is.EqualTo(4));

            // Stores the csv file
            storage.StoreProvider(provider.Provider, storageConfiguration);
            var readCsvFile = File.ReadAllText(PathForTemporaryDataFile);

            Assert.That(readCsvFile, Is.EqualTo(csvFile));

            var firstElement = extent.elements().ElementAt(0) as IObject;
            Assert.That(firstElement, Is.Not.Null);
            firstElement.set(storageConfiguration.settings.Columns[0], "eens");
            storage.StoreProvider(provider.Provider, storageConfiguration);
            readCsvFile = File.ReadAllText(PathForTemporaryDataFile);
            Assert.That(readCsvFile, Is.EqualTo(csvOtherFile));

            File.Delete(PathForTemporaryDataFile);*/
        }
    }
}