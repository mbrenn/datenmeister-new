using System.Collections.Generic;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Filler;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Tests.Xmi;
using NUnit.Framework;

namespace DatenMeister.Tests.DotNet
{
    [TestFixture]
    public class DotNetTests
    {
        [Test]
        public void TestDotNetTypeCreation()
        {
            _MOF mof;
            _UML uml;

            var typeExtent = DotNetExtentTests.Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "datenmeister:///test");
            extent.AddMetaExtent(typeExtent);
            var strapper = XmiTests.CreateUmlAndMofInstance(out mof, out uml);
            extent.AddMetaExtent(strapper.UmlInfrastructure);
            extent.AddMetaExtent(strapper.MofInfrastructure);
            extent.AddMetaExtent(strapper.PrimitiveTypesInfrastructure);

            var mofFactory = new MofFactory(typeExtent);
            var dotNetTypeCreator = new DotNetTypeGenerator(mofFactory, uml);
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

        [Test]
        public void TestDotNetConversion()
        {
            var settings = new ExcelImportSettings
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

            var copy = DotNetConverter.ConvertToDotNetObject<ExcelImportSettings>(asMof);

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
        public void TestDotNetConversionWithoutExplicitType()
        {
            var datenMeister = GiveMe.DatenMeister();
            var workspaceLogic = datenMeister.Resolve<IWorkspaceLogic>();

            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, "datenmeister:///test");
            workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace(), extent);

            var csvLoaderType = workspaceLogic.FindItem(
                "datenmeister:///_internal/types/internal#DatenMeister.Provider.XMI.ExtentStorage.XmiStorageConfiguration");

            Assert.That(csvLoaderType, Is.Not.Null);
            var memoryObject = new MofFactory(extent).create(csvLoaderType);
            memoryObject.set(nameof(XmiStorageConfiguration.workspaceId), "TEST");
            memoryObject.set(nameof(XmiStorageConfiguration.filePath), "path");
            memoryObject.set(nameof(XmiStorageConfiguration.extentUri), "dm:///");

            var asDotNetType = DotNetConverter.ConvertToDotNetObject(memoryObject);
            Assert.That(asDotNetType, Is.TypeOf<XmiStorageConfiguration>());
            var typed = (XmiStorageConfiguration) asDotNetType;
            Assert.That(typed.workspaceId, Is.EqualTo("TEST"));
            Assert.That(typed.filePath, Is.EqualTo("path"));
            Assert.That(typed.extentUri, Is.EqualTo("dm:///"));
        }
    }
}