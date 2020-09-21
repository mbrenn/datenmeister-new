using System.Collections.Generic;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Models.EMOF;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Tests.Xmi;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class DotNetTests
    {
        [Test]
        public void TestDotNetTypeCreation()
        {
            var typeExtent = DotNetExtentTests.Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            extent.AddMetaExtent(typeExtent);
            var strapper = XmiTests.CreateUmlAndMofInstance();
            extent.AddMetaExtent(strapper.UmlInfrastructure!);
            extent.AddMetaExtent(strapper.MofInfrastructure!);
            extent.AddMetaExtent(strapper.PrimitiveTypesInfrastructure!);

            var mofFactory = new MofFactory(typeExtent);
            var dotNetTypeCreator = new DotNetTypeGenerator(mofFactory);
            var dotNetClass = dotNetTypeCreator.CreateTypeFor(typeof(TestClass));

            Assert.That(dotNetClass.get(_UML._CommonStructure._NamedElement.name), Is.EqualTo("TestClass"));
        }

        public class TestClass
        {
            public string Title { get; set; }
            public int Number { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
            public string Prename { get; set; }
        }

        public class TestClassWithList
        {
            public string Title { get; set; }
            public int Number { get; set; }
            public List<string> Authors {get;set; } = new List<string>();
            public List<Person> Persons { get; set; } = new List<Person>();
        }

        public class PersonWithParent
        {
            public string Name { get; set; }
            public string Prename { get; set; }
            public PersonWithParent Parent { get; set; }
        }

        public enum TestEnumeration
        {
            First,
            Second,
            Third
        }

        public class PersonWithEnumeration
        {
            public string Name { get; set; }
            public TestEnumeration Level { get; set; }
        }

        public class PersonWithAnotherPersonPerElement 
        {
            public string Name { get; set; }
            
            public IElement Spouse { get; set; }
            
            public List<IElement> Children { get; set; }
        }

        [Test]
        public void TestOfEnumeration()
        {
            var person = new PersonWithEnumeration {Name = "Hallo", Level = TestEnumeration.First};
            var asMof = DotNetConverter.ConvertFromDotNetObject(person);

            var copy = DotNetConverter.ConvertToDotNetObject<PersonWithEnumeration>(asMof);
            Assert.That(copy.Name, Is.EqualTo("Hallo"));
            Assert.That(copy.Level, Is.EqualTo(TestEnumeration.First));
        }

        [Test]
        public void TestOfEnumerationWithFactory()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();

            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, "dm:///test");
            workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace(), extent);

            var factory = new MofFactory(extent);
            var value = factory.create(_FastViewFilters.TheOne.__PropertyComparisonFilter);
            value.set(_FastViewFilters._PropertyComparisonFilter.Property, "Test");
            value.set(_FastViewFilters._PropertyComparisonFilter.Value, "Content");
            value.set(_FastViewFilters._PropertyComparisonFilter.ComparisonType, ComparisonType.GreaterThan);

            var convertedObject = DotNetConverter.ConvertToDotNetObject(value);
            Assert.That(convertedObject, Is.TypeOf<PropertyComparisonFilter>());
            var typed = (PropertyComparisonFilter) convertedObject;

            Assert.That(typed.Value, Is.EqualTo("Content"));
            Assert.That(typed.Property, Is.EqualTo("Test"));
            Assert.That(typed.ComparisonType, Is.EqualTo(ComparisonType.GreaterThan));
        }

        [Test]
        public void TestOfEnumerationWithValueType()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();

            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, "dm:///test");
            workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace(), extent);

            var factory = new MofFactory(extent);
            var value = factory.create(_FastViewFilters.TheOne.__PropertyComparisonFilter);
            value.set(_FastViewFilters._PropertyComparisonFilter.Property, "Test");
            value.set(_FastViewFilters._PropertyComparisonFilter.Value, "Content");
            value.set(_FastViewFilters._PropertyComparisonFilter.ComparisonType, _FastViewFilters.TheOne.ComparisonType.__GreaterThan);

            var convertedObject = DotNetConverter.ConvertToDotNetObject(value);
            Assert.That(convertedObject, Is.TypeOf<PropertyComparisonFilter>());
            var typed = (PropertyComparisonFilter)convertedObject;

            Assert.That(typed.Value, Is.EqualTo("Content"));
            Assert.That(typed.Property, Is.EqualTo("Test"));
            Assert.That(typed.ComparisonType, Is.EqualTo(ComparisonType.GreaterThan));
        }


        [Test]
        public void TestOfEnumerationWithString()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();

            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, "dm:///test");
            workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace(), extent);

            var factory = new MofFactory(extent);
            var value = factory.create(_FastViewFilters.TheOne.__PropertyComparisonFilter);
            value.set(_FastViewFilters._PropertyComparisonFilter.Property, "Test");
            value.set(_FastViewFilters._PropertyComparisonFilter.Value, "Content");
            value.set(_FastViewFilters._PropertyComparisonFilter.ComparisonType, "GreaterThan");

            var convertedObject = DotNetConverter.ConvertToDotNetObject(value);
            Assert.That(convertedObject, Is.TypeOf<PropertyComparisonFilter>());
            var typed = (PropertyComparisonFilter)convertedObject;

            Assert.That(typed.Value, Is.EqualTo("Content"));
            Assert.That(typed.Property, Is.EqualTo("Test"));
            Assert.That(typed.ComparisonType, Is.EqualTo(ComparisonType.GreaterThan));
        }

        [Test]
        public void TestDotNetConversion()
        {
            var settings = new ExcelImportLoaderConfig("dm:///test")
            {
                countColumns = 1,
                countRows = 5,
                offsetColumn = 3,
                offsetRow = 5,
                filePath = "c:\\",
                fixColumnCount = true,
                fixRowCount = false,
                hasHeader = true,
                sheetName = "Yes"
            };

            var asMof = DotNetConverter.ConvertFromDotNetObject(settings);

            var copy = DotNetConverter.ConvertToDotNetObject<ExcelImportLoaderConfig>(asMof);

            Assert.That(copy.countColumns, Is.EqualTo(1));
            Assert.That(copy.countRows, Is.EqualTo(5));
            Assert.That(copy.offsetColumn, Is.EqualTo(3));
            Assert.That(copy.offsetRow, Is.EqualTo(5));
            Assert.That(copy.filePath, Is.EqualTo("c:\\"));
            Assert.That(copy.fixColumnCount, Is.EqualTo(true));
            Assert.That(copy.fixRowCount, Is.EqualTo(false));
            Assert.That(copy.hasHeader, Is.EqualTo(true));
            Assert.That(copy.sheetName, Is.EqualTo("Yes"));
        }

        [Test]
        public void TestFindingXmiStorageLoaderConfig()
        {
            using var datenMeister  = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = datenMeister.Resolve<IWorkspaceLogic>();

            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, "dm:///test");
            workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace(), extent);

            var csvLoaderType = workspaceLogic.FindItem(
                WorkspaceNames.UriExtentInternalTypes + "#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");

            Assert.That(csvLoaderType, Is.Not.Null);
        }
    }
}