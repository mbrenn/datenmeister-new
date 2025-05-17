using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Proxies.ReadOnly;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Tests.Provider;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ReadOnlyProxyTests
    {
        [Test]
        public async Task TestChainFromExtentToElement()
        {
            var csvExtent = await CreateSimpleCsvExtent();

            var readOnly = new ReadOnlyUriExtent(csvExtent);
            Assert.Throws<ReadOnlyAccessException>(() => readOnly.elements().clear());

            var element = readOnly.elements().ElementAt(0) as IElement;
            Assert.That(element, Is.Not.Null);
            Assert.Throws<ReadOnlyAccessException>(() =>
                element!.unset("Test"));

            Assert.That(readOnly.elements().size(), Is.GreaterThan(0));
            Assert.That(readOnly.elements().size(), Is.EqualTo(csvExtent.elements().size()));

            var property1 = ((IObjectAllProperties)element!).getPropertiesBeingSet().ElementAt(0);
            Assert.That(element.get(property1), Is.Not.Null);

            Assert.Throws<ReadOnlyAccessException>(() => ((IElementSetMetaClass)element).SetMetaClass(null));
        }

        /// <summary>
        /// Creates a simple extent containing three elements with four properties
        /// </summary>
        /// <returns>The created uri extent</returns>
        private static async Task<IUriExtent> CreateSimpleCsvExtent()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText(CSVExtentTests.PathForTemporaryDataFile, csvFile);

            var mapper = new ProviderToProviderLoaderMapper();
            mapper.AddMapping(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig,
                _ => new CsvProviderLoader());

            var workspaceData = WorkspaceLogic.InitDefault();

            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(mapper);
            scopeStorage.Add(new IntegrationSettings());
            var logic = new ExtentManager(WorkspaceLogic.Create(workspaceData), scopeStorage);


            var settings =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvSettings);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader, false);
            settings.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator, ' ');

            var configuration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///local/");
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath,
                CSVExtentTests.PathForTemporaryDataFile);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.settings, settings);

            /*
            var configuration = new CsvExtentLoaderConfig("dm:///local/")
            {
                filePath = CSVExtentTests.PathForTemporaryDataFile,
                settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };*/

            var csvExtent = await logic.LoadExtent(configuration);
            return csvExtent.Extent ?? throw new InvalidOperationException("Loading failed");
        }
    }
}