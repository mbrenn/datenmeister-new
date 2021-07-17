using System.IO;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Provider.CSV.Runtime;
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

            var settings = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvSettings);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader, false);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator, ' ');
            
            var storageConfiguration = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///test");
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath, PathForTemporaryDataFile);
            storageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings, settings);
               
            var storage = new CsvProviderLoader
            {
                WorkspaceLogic = WorkspaceLogic.GetEmptyLogic()
            };
            var provider = storage.LoadProvider(storageConfiguration, ExtentCreationFlags.LoadOnly);
            var extent = new MofUriExtent(provider.Provider, "dm:////test/");

            var csvSettings = storageConfiguration
                .getOrDefault<IElement>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings);
            Assert.That(settings, Is.Not.Null);
            var columns = csvSettings.getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns);

            Assert.That(columns.Count(), Is.EqualTo(3));
            Assert.That(extent.elements().Count(), Is.EqualTo(4));

            // Stores the csv file
            storage.StoreProvider(provider.Provider, storageConfiguration);
            var readCsvFile = File.ReadAllText(PathForTemporaryDataFile);

            Assert.That(readCsvFile, Is.EqualTo(csvFile));

            var firstElement = extent.elements().ElementAt(0) as IObject;
            Assert.That(firstElement, Is.Not.Null);
            
            Assert.That(storageConfiguration
                .getOrDefault<IElement>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings)
                .getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns).ElementAt(0), 
                Is.EqualTo("Column 1"));

            firstElement.set("Column 1", "eens");

            Assert.That(firstElement.getOrDefault<string>("Column 1"), Is.EqualTo("eens"));

            storage.StoreProvider(provider.Provider, storageConfiguration);
            readCsvFile = File.ReadAllText(PathForTemporaryDataFile);
            Assert.That(readCsvFile, Is.EqualTo(csvOtherFile));

            File.Delete(PathForTemporaryDataFile);
        }
    }
}