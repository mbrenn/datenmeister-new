using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Tests.Xmi;
using NUnit.Framework;
using static DatenMeister.Core.Models._DatenMeister._FastViewFilters;

namespace DatenMeister.Tests.Provider;

[TestFixture]
public class DotNetTests
{
    [Test]
    public void TestDotNetTypeCreation()
    {
        var typeExtent = DotNetExtentTests.Initialize();
        var provider = new DotNetProvider(typeExtent.TypeLookup);
        var extent = new MofUriExtent(provider, "dm:///test", null);
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
        public string? Title { get; set; }
        public int Number { get; set; }
    }

    public class Person
    {
        public string? Name { get; set; }
        public string? Prename { get; set; }
    }

    public class TestClassWithList
    {
        public string? Title { get; set; }
        public int Number { get; set; }
        public List<string> Authors { get; set; } = new();
        public List<Person> Persons { get; set; } = new();
    }

    public class PersonWithParent
    {
        public string? Name { get; set; }
        public string? Prename { get; set; }
        public PersonWithParent? Parent { get; set; }
    }

    public enum TestEnumeration
    {
        First,
        Second,
        Third
    }

    public class PersonWithEnumeration
    {
        public string? Name { get; set; }
        public TestEnumeration Level { get; set; }
    }

    public class PersonWithAnotherPersonPerElement
    {
        public string? Name { get; set; }

        public IElement? Spouse { get; set; }

        public List<IElement>? Children { get; set; }
    }

    [Test]
    public void TestOfEnumeration()
    {
        var person = new PersonWithEnumeration { Name = "Hallo", Level = TestEnumeration.First };
        var asMof = DotNetConverter.ConvertFromDotNetObject(person);

        var copy = DotNetConverter.ConvertToDotNetObject<PersonWithEnumeration>(asMof);
        Assert.That(copy.Name, Is.EqualTo("Hallo"));
        Assert.That(copy.Level, Is.EqualTo(TestEnumeration.First));
    }

    [Test]
    public async Task TestOfEnumerationWithFactory()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = scope.Resolve<IWorkspaceLogic>();

        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, "dm:///test", scope.ScopeStorage);
        workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace()!, extent);

        var factory = new MofFactory(extent);
        var value = factory.create(_DatenMeister.TheOne.FastViewFilters.__PropertyComparisonFilter);
        value.set(_PropertyComparisonFilter.Property, "Test");
        value.set(_PropertyComparisonFilter.Value, "Content");
        value.set(_PropertyComparisonFilter.ComparisonType, ___ComparisonType.GreaterThan);

        Assert.That(value.getOrDefault<string>(_PropertyComparisonFilter.Value), Is.EqualTo("Content"));
        Assert.That(value.getOrDefault<string>(_PropertyComparisonFilter.Property), Is.EqualTo("Test"));
        Assert.That(value.getOrDefault<___ComparisonType>(_PropertyComparisonFilter.ComparisonType),
            Is.EqualTo(___ComparisonType.GreaterThan));
    }

    public class TestClassForConversion
    {
        public bool fixRowCount { get; set; }
        public bool fixColumnCount { get; set; }
        public string? filePath { get; set; }
        public string? sheetName { get; set; }
        public int offsetRow { get; set; }
        public int offsetColumn { get; set; }
        public int countRows { get; set; }
        public int countColumns { get; set; }
        public bool hasHeader { get; set; } = true;
        public string? idColumnName { get; set; }
    }

    [Test]
    public void TestDotNetConversion()
    {
        var settings = new TestClassForConversion
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

        var copy = DotNetConverter.ConvertToDotNetObject<TestClassForConversion>(asMof);

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
    public async Task TestFindingXmiStorageLoaderConfig()
    {
        await using var datenMeister = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = datenMeister.Resolve<IWorkspaceLogic>();

        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, "dm:///test", datenMeister.ScopeStorage);
        workspaceLogic.AddExtent(workspaceLogic.GetDefaultWorkspace()!, extent);

        var csvLoaderType = workspaceLogic.FindElement(
            WorkspaceNames.UriExtentInternalTypes +
            "#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");

        Assert.That(csvLoaderType, Is.Not.Null);
    }
}