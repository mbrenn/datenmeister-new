using System;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.CSV.Runtime;
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
            
            var mapper = new ManualConfigurationToExtentStorageMapper();
            mapper.AddMapping(typeof (CSVExtentLoaderConfig), scope => new CSVExtentLoader(null));
            var dataLayers = WorkspaceLogic.InitDefault();

            var data = new ExtentStorageData();
            var logic = new ExtentManager(data, mapper, null, new WorkspaceLogic(dataLayers), new IntegrationSettings());
            var configuration = new CSVExtentLoaderConfig
            {
                Path = CSVExtentTests.PathForTemporaryDataFile,
                ExtentUri = "datenmeister:///local/",
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
            logic.StoreAll();

            var read = File.ReadAllText(CSVExtentTests.PathForTemporaryDataFile);
            Assert.That(read.Contains("eens"), Is.True);
            File.Delete(CSVExtentTests.PathForTemporaryDataFile);
        }
    }
}