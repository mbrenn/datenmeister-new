using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Modules.ZipCodeExample.Model;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Query;

public class TestRowFilters
{
    [Test]
    public async Task TestFilterByPropertyEqual()
    {
        var (scope, queryFlatten, queryByMetaClass) = await SetupExtentAndQuery();
        var testExtent = scope.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceData, "dm:///test")!;
        Assert.That(testExtent, Is.Not.Null);
        
        // Add some values
        var factory = new MofFactory(testExtent);
        testExtent.elements().add(factory.create(null).SetProperties(
            new Dictionary<string, object>
                { ["code"] = "12345", ["name"] = "Berlin" }));
        testExtent.elements().add(factory.create(null).SetProperties(
            new Dictionary<string, object>
                { ["code"] = "55130", ["name"] = "Mainz" }));
        testExtent.elements().add(factory.create(null).SetProperties(
            new Dictionary<string, object>
                { ["code"] = "652a3", ["name"] = "Invalidenheim" }));
        testExtent.elements().add(factory.create(null).SetProperties(
            new Dictionary<string, object>
                { ["code"] = "90231", ["name"] = "Rosenheim" }));
        
        // Now, we create the query and check that the filtering is working
        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);

        var queryProperty = factory.create(_DataViews.TheOne.Row.__RowFilterByPropertyValueNode);
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.input, queryFlatten);
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.property, "code");
        
        // Test 1, check, that only Berlin is returned
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, 
            _DataViews.___ComparisonMode.Equal);
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.value, "12345");
        
        var resultingNodes = viewLogic.GetElementsForViewNode(queryProperty).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(1));
        Assert.That(resultingNodes.All(x => x.getOrDefault<string>("code") == "12345"));
        
        // Test 2, check, that not Berlin is returned
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, 
            _DataViews.___ComparisonMode.NotEqual);
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.value, "12345");
        
        resultingNodes = viewLogic.GetElementsForViewNode(queryProperty).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.GreaterThan(2));
        Assert.That(resultingNodes.All(x => x.getOrDefault<string>("code") != "12345"));
        
        // Test 3, check that all 'heims' are returned
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.property, "name");
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, 
            _DataViews.___ComparisonMode.Contains);
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.value, "heim");
        resultingNodes = viewLogic.GetElementsForViewNode(queryProperty).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(2));
        Assert.That(resultingNodes.All(x => x.getOrDefault<string>("name").Contains("heim")));
        
        // Test 4, check that only the matches of a 5 digit zip code are returned
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.property, "code");
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, 
            _DataViews.___ComparisonMode.RegexMatch);
        queryProperty.set(_DataViews._Row._RowFilterByPropertyValueNode.value, "^[0-9]{5}$");
        resultingNodes = viewLogic.GetElementsForViewNode(queryProperty).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(3));
        Assert.That(resultingNodes.All(x =>
        {
            var name = x.getOrDefault<string>("name");
            return name is "Berlin" or "Mainz" or "Rosenheim";
        }));
    }
    
    [Test]
    public async Task TestWhenMetaClassIsWithoutInherits()
    {
        var (scope, _, queryByMetaClass) = await SetupExtentAndQuery();
        await using var datenMeisterScope = scope;

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        
        var resultingNodes = 
            viewLogic.GetElementsForViewNode(queryByMetaClass).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(1));  
        Assert.That(resultingNodes.All(
            x => x.metaclass != null && x.metaclass.equals(_DataViews.TheOne.__ViewNode)));
    }

    [Test]
    public async Task TestWhenMetaClassIsWitInherits()
    {
        var (scope, _, queryByMetaClass) = await SetupExtentAndQuery();
        await using var datenMeisterScope = scope;
        queryByMetaClass.set(_DataViews._Row._RowFilterByMetaclassNode.includeInherits, true);

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);

        var resultingNodes =
            viewLogic.GetElementsForViewNode(queryByMetaClass).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(2));
        Assert.That(
            resultingNodes.All(x =>
                x.metaclass != null &&
                (x.metaclass.equals(_DataViews.TheOne.__ViewNode) ||
                 x.metaclass.equals(_DataViews.TheOne.Source.__SelectByWorkspaceNode))));
    }


    [Test]
    public async Task TestOrderBy()
    {
        var scope = await DatenMeisterTests.GetDatenMeisterScope();

        var extentManager = scope.Resolve<ExtentManager>();

        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);

        var loadedExtentInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        Assert.That(loadedExtentInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        var factory = new MofFactory(loadedExtentInfo.Extent!);

        var item = factory.create(_DataViews.TheOne.__ViewNode);
        item.set(_DataViews._ViewNode.name, $"X");
        loadedExtentInfo.Extent!.elements().add(item);
        
        item = factory.create(_DataViews.TheOne.__ViewNode);
        item.set(_DataViews._ViewNode.name, $"B");
        loadedExtentInfo.Extent!.elements().add(item);

        item = factory.create(_DataViews.TheOne.__ViewNode);
        item.set(_DataViews._ViewNode.name, $"A");
        loadedExtentInfo.Extent!.elements().add(item);
        
        // Now, we create the query and check that the ordering is working
        var queryByExtent = factory.create(_DataViews.TheOne.Source.__SelectByExtentNode);
        queryByExtent.set(_DataViews._Source._SelectByExtentNode.workspaceId, "Data");
        queryByExtent.set(_DataViews._Source._SelectByExtentNode.extentUri, "dm:///test");
        
        var queryOnPosition = factory.create(_DataViews.TheOne.Row.__RowOrderByNode);
        queryOnPosition.set(_DataViews._Row._RowOrderByNode.input, queryByExtent);
        queryOnPosition.set(_DataViews._Row._RowOrderByNode.propertyName, "name");
        
        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        var resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        
        Assert.That(resultingNodes.Count, Is.EqualTo(3));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("A"));
        Assert.That(resultingNodes[1].get(_DataViews._ViewNode.name), Is.EqualTo("B"));
        Assert.That(resultingNodes[2].get(_DataViews._ViewNode.name), Is.EqualTo("X"));
        
        // Now check the reverse order
        queryOnPosition.set(_DataViews._Row._RowOrderByNode.orderDescending, true);
        resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(3));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("X"));
        Assert.That(resultingNodes[1].get(_DataViews._ViewNode.name), Is.EqualTo("B"));
        Assert.That(resultingNodes[2].get(_DataViews._ViewNode.name), Is.EqualTo("A"));
    }
    

    [Test]
    public async Task TestWhenOnPosition()
    {
        var scope = await DatenMeisterTests.GetDatenMeisterScope();
        
        var extentManager = scope.Resolve<ExtentManager>();

        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);
        
        var loadedExtentInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        Assert.That(loadedExtentInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        var factory = new MofFactory(loadedExtentInfo.Extent!);

        // Create the data for 100 of items
        foreach(var i in Enumerable.Range(0, 100))
        {
            var item = factory.create(_DataViews.TheOne.__ViewNode);
            item.set(_DataViews._ViewNode.name, $"Item {i}");
            loadedExtentInfo.Extent!.elements().add(item);
        }
        
        // Now, we create the query and check that a the OnPosition is capable to retrieve
        // the first 10 items, the items 15 to 25 and the last 5 items by querying position of 95 and amount of 10
        var queryByExtent = factory.create(_DataViews.TheOne.Source.__SelectByExtentNode);
        queryByExtent.set(_DataViews._Source._SelectByExtentNode.workspaceId, "Data");
        queryByExtent.set(_DataViews._Source._SelectByExtentNode.extentUri, "dm:///test");
        var queryOnPosition = factory.create(_DataViews.TheOne.Row.__RowFilterOnPositionNode);

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.input, queryByExtent);

        // Test first 10 items
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.position, 0);
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.amount, 10);
        var resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(10));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("Item 0"));
        Assert.That(resultingNodes[9].get(_DataViews._ViewNode.name), Is.EqualTo("Item 9"));

        // Test items 15 to 25
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.position, 15);
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.amount, 11);
        resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(11));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("Item 15"));
        Assert.That(resultingNodes[10].get(_DataViews._ViewNode.name), Is.EqualTo("Item 25"));

        // Test last 5 items
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.position, 95);
        queryOnPosition.set(_DataViews._Row._RowFilterOnPositionNode.amount, 10);
        resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(5));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("Item 95"));
        Assert.That(resultingNodes[4].get(_DataViews._ViewNode.name), Is.EqualTo("Item 99"));
    }

    private static async Task<(IDatenMeisterScope scope, IElement queryFlatten, IElement queryByMetaClass)> SetupExtentAndQuery()
    {
        IDatenMeisterScope? scope = null;
        try
        {
            scope = await DatenMeisterTests.GetDatenMeisterScope();

            // Create the Target Extent

            var extentManager = scope.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
            loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);
        
            var loadedExtentInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedExtentInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            var extent = loadedExtentInfo.Extent!;
            var factory = new MofFactory(extent);
        
            // Create the data!
            var viewNode = factory.create(_DataViews.TheOne.__ViewNode);
            var selectWorkspace = factory.create(_DataViews.TheOne.Source.__SelectByWorkspaceNode);
            extent.elements().add(viewNode);
            extent.elements().add(selectWorkspace);
        
            // Now create the query
            var dropDownByQueryData = factory.create(_Forms.TheOne.__DropDownByQueryData);
            var queryStatement = factory.create(_DataViews.TheOne.__QueryStatement);

            var queryByExtent = factory.create(_DataViews.TheOne.Source.__SelectByWorkspaceNode);
            queryByExtent.set(_DataViews._Source._SelectByWorkspaceNode.workspaceId, "Data");

            var queryFlatten = factory.create(_DataViews.TheOne.Row.__RowFlattenNode);
            var queryByMetaClass = factory.create(_DataViews.TheOne.Row.__RowFilterByMetaclassNode);

            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByExtent);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByMetaClass);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryFlatten);
            queryStatement.set(_DataViews._QueryStatement.resultNode, queryByMetaClass);

            dropDownByQueryData.set(_Forms._DropDownByQueryData.query, queryStatement);

            queryFlatten.set(_DataViews._Row._RowFlattenNode.input, queryByExtent);

            queryByMetaClass.set(_DataViews._Row._RowFilterByMetaclassNode.input, queryFlatten);
            queryByMetaClass.set(
                _DataViews._Row._RowFilterByMetaclassNode.metaClass,
                _DataViews.TheOne.__ViewNode);
            return (scope, queryFlatten, queryByMetaClass);
        }
        catch
        {
            if (scope != null)
            {
                await scope.DisposeAsync();
            }

            throw;
        }
    }
}