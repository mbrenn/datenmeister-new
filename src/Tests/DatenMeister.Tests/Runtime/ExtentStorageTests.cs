using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Tests.Provider;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime;

[TestFixture]
public class ExtentStorageTests
{
    [Test]
    public async Task TestExtentStorageLogic()
    {
        var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
        var fullPath = Path.Combine(CSVExtentTests.PathForTemporaryDataFile);
        File.WriteAllText(fullPath, csvFile);

        var mapper = new ProviderToProviderLoaderMapper();
        mapper.AddMapping(
            _ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig,
            _ => new CsvProviderLoader());
        var dataLayers = WorkspaceLogic.InitDefault();

        var scopeStorage = new ScopeStorage();
        scopeStorage.Add(new IntegrationSettings());
        scopeStorage.Add(mapper);
        var extentManager = new ExtentManager(WorkspaceLogic.Create(dataLayers), scopeStorage);
        extentManager.OpenDecoupled();

        var setting =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig);
        setting.set(_ExtentLoaderConfigs._CsvSettings.hasHeader, false);
        setting.set(_ExtentLoaderConfigs._CsvSettings.separator, ' ');

        var configuration =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig);
        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///local/");
        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath,
            CSVExtentTests.PathForTemporaryDataFile);
        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);
        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.settings, setting);

        var csvExtent = await extentManager.LoadExtent(configuration);
        Assert.That(csvExtent, Is.Not.Null);
        Assert.That(csvExtent.Extent, Is.Not.Null);

        Assert.That(csvExtent.Extent!.elements().Count(), Is.EqualTo(4));
        await extentManager.StoreExtent(csvExtent.Extent);

        // Changes content, store it and check, if stored
        var settings =
            configuration.getOrDefault<IElement>(_ExtentLoaderConfigs._CsvExtentLoaderConfig
                .settings);
        Assert.That(settings, Is.Not.Null);
        var columns =
            settings.getOrDefault<IReflectiveCollection>(_ExtentLoaderConfigs._CsvSettings.columns);
        Assert.That(columns, Is.Not.Null);

        ((IObject)csvExtent.Extent.elements().ElementAt(0)!)
            .set(columns.ElementAt(0) as string ?? throw new InvalidOperationException(
                "Exception"), "eens");
        await extentManager.UnloadManager(true);

        var read = File.ReadAllText(CSVExtentTests.PathForTemporaryDataFile);
        Assert.That(read.Contains("eens"), Is.True);
        File.Delete(CSVExtentTests.PathForTemporaryDataFile);
    }

    [Test]
    public async Task TestConfigurationRetrieval()
    {
        var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
        var fullPath = Path.Combine(CSVExtentTests.PathForTemporaryDataFile);
        File.WriteAllText(fullPath, csvFile);

        var mapper = new ProviderToProviderLoaderMapper();
        mapper.AddMapping(
            _ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig,
            _ => new CsvProviderLoader());
        var dataLayers = WorkspaceLogic.InitDefault();

        var scopeStorage = new ScopeStorage();
        scopeStorage.Add(mapper);
        scopeStorage.Add(new IntegrationSettings());
        var logic = new ExtentManager(WorkspaceLogic.Create(dataLayers), scopeStorage);
        var settings = InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__CsvSettings);
        settings.set(_ExtentLoaderConfigs._CsvSettings.hasHeader, false);
        settings.set(_ExtentLoaderConfigs._CsvSettings.separator, ' ');
        var configuration =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig);

        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, "dm:///local/");
        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath,
            CSVExtentTests.PathForTemporaryDataFile);
        configuration.set(_ExtentLoaderConfigs._CsvExtentLoaderConfig.settings, settings);

        var csvExtent = await logic.LoadExtent(configuration);
        Assert.That(csvExtent, Is.Not.Null);
        Assert.That(csvExtent.Extent, Is.Not.Null);

        var foundConfiguration = logic.GetLoadConfigurationFor(csvExtent.Extent!);
        Assert.That(foundConfiguration, Is.EqualTo(configuration));


        foundConfiguration = logic.GetLoadConfigurationFor(null!);
        Assert.That(foundConfiguration, Is.Null);

        foundConfiguration =
            logic.GetLoadConfigurationFor(new MofUriExtent(new InMemoryProvider(), "dm:///temp", null));
        Assert.That(foundConfiguration, Is.Null);
    }

    [Test]
    public async Task TestSettingsSectionPreservedOnShutdown()
    {
        var tempXmlPath = Path.GetTempFileName();
        var tempCsvPath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempCsvPath, "1\t12345\t10.0\t20.0\tCity\r\n");

            var initialXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<xmi:XMI xmlns:xmi=""http://www.omg.org/spec/XMI/20131001"" Version=""1.0"">
  <item p2:type=""dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig"" p2:id=""0d285a84-6054-480b-8a81-d182315e6852"" extentUri=""dm:///zipcodes/157555861"" filePath=""{tempCsvPath.Replace("\\", "\\\\")}"" workspaceId=""Data"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"">
    <settings p2:type=""dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings"" p2:id=""f72d4e29-435d-4651-af86-4fdbf2e53c89"" separator=""&#x9;"" encoding=""UTF-8"" metaclassUri-ref=""dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode"">
      <columns>id</columns>
      <columns>zip</columns>
      <columns>positionLong</columns>
      <columns>positionLat</columns>
      <columns>name</columns>
    </settings>
  </item>
</xmi:XMI>";
            File.WriteAllText(tempXmlPath, initialXml);

            var mapper = new ProviderToProviderLoaderMapper();
            mapper.AddMapping(
                _ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig,
                _ => new CsvProviderLoader());
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(new IntegrationSettings());
            scopeStorage.Add(mapper);
            scopeStorage.Add(new DatenMeister.Core.TypeIndexAssembly.TypeIndexStore());
            var extentStorageData = new ExtentStorageData { FilePath = tempXmlPath };
            scopeStorage.Add(extentStorageData);

            var dataLayers = WorkspaceLogic.InitDefault();
            var workspaceLogic = WorkspaceLogic.Create(dataLayers, scopeStorage);
            var typesExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentInternalTypes, null);
            workspaceLogic.AddExtent(workspaceLogic.GetTypesWorkspace(), typesExtent);
            DatenMeister.Core.Uml.Helper.PackageMethods.ImportByStream(
                new ScopeStorage(),
                DatenMeister.Core.XmiFiles.XmiResources.GetDatenMeisterTypesStream(),
                null,
                typesExtent,
                "DatenMeister");

            var typeIndexLogic = new DatenMeister.Core.TypeIndexAssembly.TypeIndexLogic(workspaceLogic);
            await typeIndexLogic.CreateIndexFirstTime();

            var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
            await extentManager.LoadAllExtents();

            // Now shutdown / unload manager
            await extentManager.UnloadManager(true);

            var savedXml = File.ReadAllText(tempXmlPath);
            Assert.That(savedXml, Does.Contain("separator"), "Saved XML should contain settings content (separator)");
            Assert.That(savedXml, Does.Contain("<settings"), "Saved XML should contain <settings tag");
        }
        finally
        {
            if (File.Exists(tempXmlPath)) File.Delete(tempXmlPath);
            if (File.Exists(tempCsvPath)) File.Delete(tempCsvPath);
        }
    }
}