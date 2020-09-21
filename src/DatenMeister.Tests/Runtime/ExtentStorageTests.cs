using System;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
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
            mapper.AddMapping(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig, 
                scope => new CsvProviderLoader());
            var dataLayers = WorkspaceLogic.InitDefault();

            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(new IntegrationSettings());
            scopeStorage.Add(mapper);
            var extentManager = new ExtentManager(WorkspaceLogic.Create(dataLayers), scopeStorage);
            extentManager.OpenDecoupled();
            
            var setting =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            setting.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader, false);
            setting.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator, ' ');
            
            var configuration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///local/");
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath, CSVExtentTests.PathForTemporaryDataFile);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.workspaceId, WorkspaceNames.WorkspaceData);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings, setting);

            var csvExtent = extentManager.LoadExtent(configuration);
            Assert.That(csvExtent, Is.Not.Null);
            Assert.That(csvExtent.Extent, Is.Not.Null);

            Assert.That(csvExtent.Extent!.elements().Count(), Is.EqualTo(4));
            extentManager.StoreExtent(csvExtent.Extent);

            // Changes content, store it and check, if stored
            var settings = configuration.getOrDefault<IElement>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings);
            Assert.That(settings, Is.Not.Null);
            var columns =
                settings.getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns);
            Assert.That(columns, Is.Not.Null);

            ((IObject) csvExtent.Extent.elements().ElementAt(0))
                .set(columns.ElementAt(0) as string ?? throw new InvalidOperationException(
                    "Exception"), "eens");
            extentManager.UnloadManager(true);

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
            mapper.AddMapping(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig, 
                scope => new CsvProviderLoader());
            var dataLayers = WorkspaceLogic.InitDefault();

            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(mapper);
            scopeStorage.Add(new IntegrationSettings());
            var logic = new ExtentManager(WorkspaceLogic.Create(dataLayers), scopeStorage);
            var configuration = new CsvExtentLoaderConfig("dm:///local/")
            {
                filePath = CSVExtentTests.PathForTemporaryDataFile,
                settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            /*
            var csvExtent = logic.LoadExtent(configuration);
            Assert.That(csvExtent, Is.Not.Null);
            Assert.That(csvExtent.Extent, Is.Not.Null);

            var foundConfiguration = logic.GetLoadConfigurationFor(csvExtent.Extent);
            Assert.That(foundConfiguration, Is.EqualTo(configuration));


            foundConfiguration = logic.GetLoadConfigurationFor(null!);
            Assert.That(foundConfiguration, Is.Null);

            foundConfiguration = logic.GetLoadConfigurationFor(new MofUriExtent(new InMemoryProvider(), "dm:///temp"));
            Assert.That(foundConfiguration, Is.Null);
            */
        }
    }
}