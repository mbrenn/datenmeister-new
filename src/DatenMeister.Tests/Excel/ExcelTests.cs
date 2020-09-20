﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Excel.Integration;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Excel
{
    [TestFixture]
    public class ExcelTests
    {
        [Test]
        public void LoadExcel()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var excelExtent = dm.LoadExcel("d:///excel", Path.Combine(currentDirectory, "Excel/Quadratzahlen.xlsx"));

            Console.WriteLine(excelExtent.ToString());
            foreach (var sheet in excelExtent.GetRootObjects())
            {
                var allProperties = ((IEnumerable<object>) sheet.GetProperty("items")).First() as IProviderObject;
                Assert.That(sheet.GetProperty("name").ToString(), Is.EqualTo("Tabelle1"));
                Assert.That(allProperties, Is.Not.Null);

                Assert.That(allProperties.GetProperties().Count(), Is.EqualTo(2));
                Assert.That(allProperties.GetProperties().ElementAt(0), Is.EqualTo("Wert"));

                foreach (var item in (IEnumerable<object>) sheet.GetProperty("items"))
                {
                    var itemAsElement = (IProviderObject) item;

                    var value1 = Convert.ToInt32(itemAsElement.GetProperty("Wert"));
                    var value2 = Convert.ToInt32(itemAsElement.GetProperty("Quadratzahl"));
                    Assert.That(value1 * value1 - value2, Is.EqualTo(0));
                }

                Assert.That(((IEnumerable<object>) sheet.GetProperty("items")).Count(), Is.GreaterThan(10));
            }
        }

        [Test]
        public void PerformExcelReference()
        {
            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var excelReferenceSettings =
                    InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelReferenceLoaderConfig);
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig.extentUri,
                    "dm:///excel2");
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig.filePath,
                    Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig.hasHeader,
                    true);
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig.sheetName,
                    "Tabelle1");

                var extentManager = dm.Resolve<ExtentManager>();
                var loadedExtent = extentManager.LoadExtent(excelReferenceSettings, ExtentCreationFlags.LoadOrCreate);
                Assert.That(loadedExtent.Extent, Is.Not.Null);
                Assert.That(loadedExtent.Extent!.elements().Count(), Is.GreaterThan(0));

                var secondElement = loadedExtent.Extent.elements().ElementAtOrDefault(1) as IObject;
                Assert.That(secondElement, Is.Not.Null);
                var value1 = DotNetHelper.AsInteger(secondElement.get("Wert"));
                var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

                Assert.That(value1, Is.EqualTo(2));
                Assert.That(value2, Is.EqualTo(4));

                dm.UnuseDatenMeister();
            }

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(false))
            {
                var extentManager = dm.Resolve<IWorkspaceLogic>();

                var loadedExtent = extentManager.FindExtent("dm:///excel2");
                Assert.That(loadedExtent, Is.Not.Null);
                Assert.That(loadedExtent.elements().Count(), Is.GreaterThan(0));

                var secondElement = loadedExtent.elements().ElementAtOrDefault(1) as IObject;
                Assert.That(secondElement, Is.Not.Null);
                var value1 = DotNetHelper.AsInteger(secondElement.get("Wert"));
                var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

                Assert.That(value1, Is.EqualTo(2));
                Assert.That(value2, Is.EqualTo(4));
            }
        }

        [Test]
        public void PerformExcelImport()
        {
            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                
                
                var excelReferenceSettings =
                    InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelImportLoaderConfig);
                
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentUri,
                    "dm:///excel2");
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.filePath,
                    Path.Combine(currentDirectory!, "Excel/Quadratzahlen.xlsx"));
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentPath,
                    Path.Combine(currentDirectory, "test.xmi"));
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.hasHeader,
                    true);
                excelReferenceSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.sheetName,
                    "Tabelle1");
                
                var extentManager = dm.Resolve<ExtentManager>();
                var loadedExtent = extentManager.LoadExtent(excelReferenceSettings, ExtentCreationFlags.LoadOrCreate);
                Assert.That(loadedExtent.Extent, Is.Not.Null);
                Assert.That(loadedExtent.Extent!.elements().Count(), Is.GreaterThan(0));

                var secondElement = loadedExtent.Extent!.elements().ElementAtOrDefault(1) as IObject;
                Assert.That(secondElement, Is.Not.Null);
                var value1 = DotNetHelper.AsInteger(secondElement.get("Wert"));
                var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

                Assert.That(value1, Is.EqualTo(2));
                Assert.That(value2, Is.EqualTo(4));

                dm.UnuseDatenMeister();
            }
            
            using (var dm = DatenMeisterTests.GetDatenMeisterScope(false))
            {
                var extentManager = dm.Resolve<IWorkspaceLogic>();

                var loadedExtent = extentManager.FindExtent("dm:///excel2");
                Assert.That(loadedExtent, Is.Not.Null);
                Assert.That(loadedExtent.elements().Count(), Is.GreaterThan(0));

                var secondElement = loadedExtent.elements().ElementAtOrDefault(1) as IObject;
                Assert.That(secondElement, Is.Not.Null);
                var value1 = DotNetHelper.AsInteger(secondElement.get("Wert"));
                var value2 = DotNetHelper.AsInteger(secondElement.get("Quadratzahl"));

                Assert.That(value1, Is.EqualTo(2));
                Assert.That(value2, Is.EqualTo(4));
            }
        }
    }
}