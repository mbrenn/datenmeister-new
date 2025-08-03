using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules;

[TestFixture]
public class DataviewTests
{
    [Test]
    public async Task TestCreationOfDataviews()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var helper = dm.Resolve<DataViewHelper>();
        var viewWorkspace = helper.GetViewWorkspace();

        Assert.That(viewWorkspace.extent.Count(), Is.EqualTo(0));
        helper.CreateDataview("Test", "dm:///view/test");

        Assert.That(viewWorkspace.extent.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task TestPropertyFilter()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var dataExtent = await CreateDataForTest(dm);
        Assert.That(dataExtent.elements().Count(), Is.GreaterThan(1));

        var helper = dm.Resolve<DataViewHelper>();
        var dataView = helper.CreateDataview("Test", "dm:///view/test");

        var userViewExtent = helper.GetUserFormExtent();

        var factory = new MofFactory(userViewExtent);
        var extentSource = factory.create(_DataViews.TheOne.__SelectByExtentNode);
        extentSource.set(_DataViews._SelectByExtentNode.extentUri, "dm:///testdata");
        userViewExtent.elements().add(extentSource);

        var propertyFilter = factory.create(_DataViews.TheOne.__RowFilterByPropertyValueNode);
        userViewExtent.elements().add(propertyFilter);
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.property, "name");
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.comparisonMode,
            _DataViews.___ComparisonMode.Contains);
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.value, "ai");
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.input, extentSource);

        dataView.set(_DataViews._DataView.viewNode, propertyFilter);

        var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
        var extent = workspaceLogic.FindExtent("dm:///view/test");

        Assert.That(extent, Is.Not.Null);

        var elements = extent!.elements().OfType<IElement>().ToArray();
        Assert.That(elements.All(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
        Assert.That(elements.Any(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
        Assert.That(elements.Length, Is.GreaterThan(0));

        // Go to Non-Contain
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.comparisonMode,
            _DataViews.___ComparisonMode.DoesNotContain);
        elements = extent.elements().OfType<IElement>().ToArray();
        Assert.That(elements.All(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.False);
        Assert.That(elements.Any(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.False);
        Assert.That(elements.Length, Is.GreaterThan(0));
    }

    [Test]
    public async Task TestDynamicSourceNodes()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var dataExtent = await CreateDataForTest(dm);

        var factory = InMemoryObject.TemporaryFactory;

        // Creates the dataview
        var extentSource = factory.create(_DataViews.TheOne.__DynamicSourceNode);
        extentSource.set(_DataViews._DynamicSourceNode.name, "input");

        var propertyFilter = factory.create(_DataViews.TheOne.__RowFilterByPropertyValueNode);
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.property, "name");
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.comparisonMode,
            _DataViews.___ComparisonMode.Contains);
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.value, "ai");
        propertyFilter.set(_DataViews._RowFilterByPropertyValueNode.input, extentSource);

        // Gets the elements
        var dataViewEvaluator = new DataViewEvaluation(dm.WorkspaceLogic, dm.ScopeStorage);
        dataViewEvaluator.AddDynamicSource("input", dataExtent.elements());

        var elements = dataViewEvaluator.GetElementsForViewNode(propertyFilter)
            .OfType<IElement>().ToArray();

        // Evaluates the elements
        Assert.That(elements.All(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
        Assert.That(elements.Any(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
        Assert.That(elements.Length, Is.GreaterThan(0));
    }

    [Test]
    public async Task TestAllWorkspacesEvaluation()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        await CreateDataForTest(dm);

        var factory = InMemoryObject.TemporaryFactory;

        var selectFromAllWorkspacesNode =
            factory.create(_DataViews.TheOne.__SelectFromAllWorkspacesNode);

        var dataViewEvaluator = new DataViewEvaluation(dm.WorkspaceLogic, dm.ScopeStorage);
        var result =
            dataViewEvaluator
                .GetElementsForViewNode(selectFromAllWorkspacesNode)
                .OfType<IElement>()
                .ToList();

        var grouped = result.GroupBy(x => x.GetUriExtentOf()?.GetWorkspace())
            .ToList();
            
        Assert.That(grouped.Count(), Is.GreaterThan(1));
        Assert.That(grouped.Any(x => x.Key?.id == WorkspaceNames.WorkspaceData), Is.True);
        Assert.That(grouped.Any(x => x.Key?.id == WorkspaceNames.WorkspaceTypes), Is.True);
    }

    [Test]
    public async Task TestSelectByWorkspaceEvaluation()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        await CreateDataForTest(dm);

        var factory = InMemoryObject.TemporaryFactory;

        var selectByWorkspaceNode =
            factory.create(_DataViews.TheOne.__SelectByWorkspaceNode);
        selectByWorkspaceNode.set(_DataViews._SelectByWorkspaceNode.workspaceId, WorkspaceNames.WorkspaceData);

        var dataViewEvaluator = new DataViewEvaluation(dm.WorkspaceLogic, dm.ScopeStorage);
        var result =
            dataViewEvaluator
                .GetElementsForViewNode(selectByWorkspaceNode)
                .OfType<IElement>()
                .ToList();

        var grouped = result.GroupBy(x => x.GetUriExtentOf()?.GetWorkspace())
            .ToList();
            
        Assert.That(grouped.Count(), Is.EqualTo(1));
        Assert.That(grouped.Any(x => x.Key?.id == WorkspaceNames.WorkspaceData), Is.True);
        Assert.That(grouped.Any(x => x.Key?.id == WorkspaceNames.WorkspaceTypes), Is.False);
            
        Assert.That(result.Any(x=>x.getOrDefault<string>("name") == "Bach"), Is.True);
    }

    private async Task<IUriExtent> CreateDataForTest(IDatenMeisterScope dm)
    {
        var localTypeSupport = dm.Resolve<LocalTypeSupport>();
        var userTypeExtent = localTypeSupport.GetUserTypeExtent();

        // Create two example types
        var userTypeFactory = new MofFactory(userTypeExtent);
        var createdClass = userTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
        createdClass.set(_UML._CommonStructure._NamedElement.name, "First Class");
        userTypeExtent.elements().add(createdClass);

        var secondClass = userTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
        secondClass.set(_UML._CommonStructure._NamedElement.name, "Second Class");
        userTypeExtent.elements().add(secondClass);

        // Ok, now add the data
        var extent = (await XmiExtensions
            .CreateAndAddXmiExtent(dm.Resolve<ExtentManager>(), "dm:///testdata", "testdata.xmi")).Extent!;
        Assert.That(extent, Is.Not.Null);
        var factory = new MofFactory(extent);
        var element1 = factory.create(createdClass);
        element1.set("name", "Bach");
        element1.set("zip", 32432);
        extent.elements().add(element1);

        element1 = factory.create(createdClass);
        element1.set("name", "Mainz");
        element1.set("zip", 55130);
        extent.elements().add(element1);

        element1 = factory.create(secondClass);
        element1.set("name", "Bischofsheim");
        element1.set("zip", 65474);
        extent.elements().add(element1);

        return extent;
    }
}