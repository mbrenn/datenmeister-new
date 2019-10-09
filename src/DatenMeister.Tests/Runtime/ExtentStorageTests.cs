using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Tests.CSV;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ExtentStorageTests
    {
        [Test]
        public void TestExtentStorageLogic()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            var fullPath = Path.Combine(CSVExtentTests.PathForTemporaryDataFile);
            File.WriteAllText(fullPath, csvFile);
            
            var mapper = new ConfigurationToExtentStorageMapper();
            mapper.AddMapping(typeof (CsvExtentLoaderConfig), scope => new CsvProviderLoader(null));
            var dataLayers = WorkspaceLogic.InitDefault();

            var data = new ExtentStorageData();
            var logic = new ExtentManager(data, mapper, null, new WorkspaceLogic(dataLayers), new IntegrationSettings());
            var configuration = new CsvExtentLoaderConfig
            {
                filePath = CSVExtentTests.PathForTemporaryDataFile,
                extentUri = "datenmeister:///local/",
                Settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            var csvExtent = logic.LoadExtent(configuration);
            Assert.That(csvExtent, Is.Not.Null);
            
            Assert.That(csvExtent.elements().Count(), Is.EqualTo(4));
            logic.StoreExtent(csvExtent);

            // Changes content, store it and check, if stored
            ((IObject) csvExtent.elements().ElementAt(0)).set(configuration.Settings.Columns[0], "eens");
            logic.StoreAllExtents();

            var read = File.ReadAllText(CSVExtentTests.PathForTemporaryDataFile);
            Assert.That(read.Contains("eens"), Is.True);
            File.Delete(CSVExtentTests.PathForTemporaryDataFile);
        }

        [Test]
        public void TestConfigurationRetrieval()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            var fullPath = Path.Combine(CSVExtentTests.PathForTemporaryDataFile);
            File.WriteAllText(fullPath, csvFile);

            var mapper = new ConfigurationToExtentStorageMapper();
            mapper.AddMapping(typeof(CsvExtentLoaderConfig), scope => new CsvProviderLoader(null));
            var dataLayers = WorkspaceLogic.InitDefault();

            var data = new ExtentStorageData();
            var logic = new ExtentManager(data, mapper, null, new WorkspaceLogic(dataLayers), new IntegrationSettings());
            var configuration = new CsvExtentLoaderConfig
            {
                filePath = CSVExtentTests.PathForTemporaryDataFile,
                extentUri = "datenmeister:///local/",
                Settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            var csvExtent = logic.LoadExtent(configuration);
            Assert.That(csvExtent, Is.Not.Null);

            var foundConfiguration = logic.GetLoadConfigurationFor(csvExtent);
            Assert.That(foundConfiguration, Is.EqualTo(configuration));


            foundConfiguration = logic.GetLoadConfigurationFor(null);
            Assert.That(foundConfiguration, Is.Null);

            foundConfiguration = logic.GetLoadConfigurationFor(new MofUriExtent(new InMemoryProvider(), "datenmeister:///temp"));
            Assert.That(foundConfiguration, Is.Null);
        }
    }
}