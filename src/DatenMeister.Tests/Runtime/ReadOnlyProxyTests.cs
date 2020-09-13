using System;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Proxies.ReadOnly;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Tests.CSV;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ReadOnlyProxyTests
    {
        [Test]
        public void TestChainFromExtentToElement()
        {
            var csvExtent = CreateSimpleCsvExtent();

            var readOnly = new ReadOnlyUriExtent(csvExtent);
            Assert.Throws<ReadOnlyAccessException>(() => readOnly.elements().clear());

            var element = readOnly.elements().ElementAt(0) as IElement;
            Assert.That(element, Is.Not.Null);
            Assert.Throws<ReadOnlyAccessException>(() =>
                element.unset("Test"));

            Assert.That(readOnly.elements().size(), Is.GreaterThan(0));
            Assert.That(readOnly.elements().size(), Is.EqualTo(csvExtent.elements().size()));

            var property1  = ((IObjectAllProperties) element).getPropertiesBeingSet().ElementAt(0);
            Assert.That(element.get(property1), Is.Not.Null);

            Assert.Throws<ReadOnlyAccessException>(() => ((IElementSetMetaClass) element).SetMetaClass(null));
        }

        /// <summary>
        /// Creates a simple extent containing three elements with four properties
        /// </summary>
        /// <returns>The created uri extent</returns>
        private static IUriExtent CreateSimpleCsvExtent()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText(CSVExtentTests.PathForTemporaryDataFile, csvFile);

            var mapper = new ConfigurationToExtentStorageMapper();
            mapper.AddMapping(typeof (CsvExtentLoaderConfig), scope => new CsvProviderLoader(null));
            var workspaceData = WorkspaceLogic.InitDefault();

            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(new IntegrationSettings());
            var logic = new ExtentManager(WorkspaceLogic.Create(workspaceData), scopeStorage);
            var configuration = new CsvExtentLoaderConfig("dm:///local/")
            {
                filePath = CSVExtentTests.PathForTemporaryDataFile,
                Settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            var csvExtent = logic.LoadExtent(configuration);
            return csvExtent.Extent ?? throw new InvalidOperationException("Loading failed");
        }
    }
}