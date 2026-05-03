using System.Reflection;
using Autofac;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Provider.Excel.Integration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using NUnit.Framework;

namespace DatenMeister.Tests.Excel;

[TestFixture]
public class ExcelTests
{
    [Test]
    public async Task LoadExcel()
    {
        var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var excelExtent = dm.LoadExcel("d:///excel", Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));

        foreach (var sheet in excelExtent.GetRootObjects().Take(1))
        {
            var allProperties = ((IEnumerable<object>)sheet.GetProperty("items")!).First() as IProviderObject;
            Assert.That(sheet.GetProperty("name")!.ToString(), Is.EqualTo("Tabelle1"));
            Assert.That(allProperties, Is.Not.Null);

            Assert.That(allProperties!.GetProperties().Count(), Is.EqualTo(2));
            Assert.That(allProperties.GetProperties().ElementAt(0), Is.EqualTo("Wert"));

            foreach (var item in (IEnumerable<object>)sheet.GetProperty("items")!)
            {
                var itemAsElement = (IProviderObject)item;

                var value1 = Convert.ToInt32(itemAsElement.GetProperty("Wert"));
                var value2 = Convert.ToInt32(itemAsElement.GetProperty("Quadratzahl"));
                Assert.That(value1 * value1 - value2, Is.EqualTo(0));
            }

            Assert.That(((IEnumerable<object>)sheet.GetProperty("items")!).Count(), Is.GreaterThan(10));
        }
    }

    [Test]
    public async Task PerformExcelReference()
    {
        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope())
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var excelReferenceSettings =
                InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__ExcelReadOnlyLoaderConfig);
            excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.extentUri,
                "dm:///excel2");
            excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.filePath,
                Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));
            excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.hasHeader,
                true);
            excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.sheetName,
                "Tabelle1");

            var extentManager = dm.Resolve<ExtentManager>();
            var loadedExtent = await extentManager.LoadExtent(excelReferenceSettings, ExtentCreationFlags.LoadOrCreate);
            Assert.That(loadedExtent.Extent, Is.Not.Null);
            Assert.That(loadedExtent.Extent!.elements().Count(), Is.GreaterThan(0));

            var secondElement = loadedExtent.Extent.elements().ElementAtOrDefault(1) as IObject;
            Assert.That(secondElement, Is.Not.Null);
            var value1 = DotNetHelper.AsInteger(secondElement!.get("Wert"));
            var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

            Assert.That(value1, Is.EqualTo(2));
            Assert.That(value2, Is.EqualTo(4));

            await dm.UnuseDatenMeister();
        }

        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope(false))
        {
            var extentManager = dm.Resolve<IWorkspaceLogic>();

            var loadedExtent = extentManager.FindExtent("dm:///excel2");
            Assert.That(loadedExtent, Is.Not.Null);
            Assert.That(loadedExtent!.elements().Count(), Is.GreaterThan(0));

            var secondElement = loadedExtent.elements().ElementAtOrDefault(1) as IObject;
            Assert.That(secondElement, Is.Not.Null);
            var value1 = DotNetHelper.AsInteger(secondElement!.get("Wert"));
            var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

            Assert.That(value1, Is.EqualTo(2));
            Assert.That(value2, Is.EqualTo(4));
        }
    }

    [Test]
    public async Task PerformExcelTestSkipRows()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var excelReferenceSettings =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__ExcelReadOnlyLoaderConfig);
        excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.extentUri,
            "dm:///excel2");
        excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.filePath,
            Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));
        excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.hasHeader,
            true);
        excelReferenceSettings.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.sheetName,
            "Tabelle2");
        excelReferenceSettings.set(
            _ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.skipEmptyRowsCount,
            0);

        var extentManager = dm.Resolve<ExtentManager>();
        var loadedExtent = await extentManager.LoadExtent(excelReferenceSettings, ExtentCreationFlags.LoadOrCreate);
        Assert.That(loadedExtent.Extent, Is.Not.Null);
        Assert.That(loadedExtent.Extent!.elements().Count(), Is.LessThan(40));


        var excelReferenceSettings2 =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__ExcelReadOnlyLoaderConfig);
        excelReferenceSettings2.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.extentUri,
            "dm:///excel2");
        excelReferenceSettings2.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.filePath,
            Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));
        excelReferenceSettings2.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.hasHeader,
            true);
        excelReferenceSettings2.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.sheetName,
            "Tabelle2");
        excelReferenceSettings2.set(
            _ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.skipEmptyRowsCount,
            0);
        excelReferenceSettings2.set(_ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.extentUri,
            "dm:///excel3");
        excelReferenceSettings2.set(
            _ExtentLoaderConfigs._ExcelReadOnlyLoaderConfig.skipEmptyRowsCount,
            5);

        var loadedExtent2 = await extentManager.LoadExtent(excelReferenceSettings2, ExtentCreationFlags.LoadOrCreate);
        Assert.That(loadedExtent2.Extent, Is.Not.Null);
        Assert.That(loadedExtent2.Extent!.elements().Count(), Is.GreaterThan(40));
        await dm.UnuseDatenMeister();
    }

    [Test]
    public async Task PerformExcelImport()
    {
        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope())
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var excelImportLoaderConfig =
                InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__ExcelConvertToXmiOnceConfig);

            excelImportLoaderConfig.set(_ExtentLoaderConfigs._ExcelConvertToXmiOnceConfig.extentUri,
                "dm:///excel2");
            excelImportLoaderConfig.set(_ExtentLoaderConfigs._ExcelConvertToXmiOnceConfig.filePath,
                Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));
            excelImportLoaderConfig.set(_ExtentLoaderConfigs._ExcelConvertToXmiOnceConfig.xmiFilePath,
                Path.Combine(currentDirectory!, "test.xmi"));
            excelImportLoaderConfig.set(_ExtentLoaderConfigs._ExcelConvertToXmiOnceConfig.hasHeader,
                true);
            excelImportLoaderConfig.set(_ExtentLoaderConfigs._ExcelConvertToXmiOnceConfig.sheetName,
                "Tabelle1");

            var extentManager = dm.Resolve<ExtentManager>();
            var loadedExtent = await extentManager.LoadExtent(excelImportLoaderConfig, ExtentCreationFlags.LoadOrCreate);
            Assert.That(loadedExtent.Extent, Is.Not.Null);
            Assert.That(loadedExtent.Extent!.elements().Count(), Is.GreaterThan(0));

            var secondElement = loadedExtent.Extent!.elements().ElementAtOrDefault(1) as IObject;
            Assert.That(secondElement, Is.Not.Null);
            var value1 = DotNetHelper.AsInteger(secondElement!.get("Wert"));
            var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

            Assert.That(value1, Is.EqualTo(2));
            Assert.That(value2, Is.EqualTo(4));

            await dm.UnuseDatenMeister();
        }

        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope(false))
        {
            var extentManager = dm.Resolve<IWorkspaceLogic>();

            var loadedExtent = extentManager.FindExtent("dm:///excel2");
            Assert.That(loadedExtent, Is.Not.Null);
            Assert.That(loadedExtent!.elements().Count(), Is.GreaterThan(0));

            var secondElement = loadedExtent.elements().ElementAtOrDefault(1) as IObject;
            Assert.That(secondElement, Is.Not.Null);
            var value1 = DotNetHelper.AsInteger(secondElement!.get("Wert"));
            var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

            Assert.That(value1, Is.EqualTo(2));
            Assert.That(value2, Is.EqualTo(4));
        }
    }
}