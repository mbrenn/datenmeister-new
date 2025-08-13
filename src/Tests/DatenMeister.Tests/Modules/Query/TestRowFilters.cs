using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Query;

public class TestRowFilters
{
    
    [Test]
    public async Task TestWhenMetaClassIsWithoutInherits()
    {
        var (scope, queryByMetaClass) = await SetupExtentAndQuery();
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
        var (scope, queryByMetaClass) = await SetupExtentAndQuery();
        await using var datenMeisterScope = scope;
        queryByMetaClass.set(_DataViews._RowFilterByMetaclassNode.includeInherits, true);

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);

        var resultingNodes =
            viewLogic.GetElementsForViewNode(queryByMetaClass).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(2));
        Assert.That(
            resultingNodes.All(x =>
                x.metaclass != null &&
                (x.metaclass.equals(_DataViews.TheOne.__ViewNode) ||
                 x.metaclass.equals(_DataViews.TheOne.__SelectByWorkspaceNode))));
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
        var queryByExtent = factory.create(_DataViews.TheOne.__SelectByExtentNode);
        queryByExtent.set(_DataViews._SelectByExtentNode.workspaceId, "Data");
        queryByExtent.set(_DataViews._SelectByExtentNode.extentUri, "dm:///test");
        var queryOnPosition = factory.create(_DataViews.TheOne.__RowFilterOnPositionNode);

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.input, queryByExtent);

        // Test first 10 items
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.position, 0);
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.amount, 10);
        var resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(10));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("Item 0"));
        Assert.That(resultingNodes[9].get(_DataViews._ViewNode.name), Is.EqualTo("Item 9"));

        // Test items 15 to 25
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.position, 15);
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.amount, 11);
        resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(11));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("Item 15"));
        Assert.That(resultingNodes[10].get(_DataViews._ViewNode.name), Is.EqualTo("Item 25"));

        // Test last 5 items
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.position, 95);
        queryOnPosition.set(_DataViews._RowFilterOnPositionNode.amount, 10);
        resultingNodes = viewLogic.GetElementsForViewNode(queryOnPosition).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.EqualTo(5));
        Assert.That(resultingNodes[0].get(_DataViews._ViewNode.name), Is.EqualTo("Item 95"));
        Assert.That(resultingNodes[4].get(_DataViews._ViewNode.name), Is.EqualTo("Item 99"));
    }

    private static async Task<(IDatenMeisterScope scope, IElement queryByMetaClass)> SetupExtentAndQuery()
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
            var selectWorkspace = factory.create(_DataViews.TheOne.__SelectByWorkspaceNode);
            extent.elements().add(viewNode);
            extent.elements().add(selectWorkspace);
        
            // Now create the query
            var dropDownByQueryData = factory.create(_Forms.TheOne.__DropDownByQueryData);
            var queryStatement = factory.create(_DataViews.TheOne.__QueryStatement);

            var queryByExtent = factory.create(_DataViews.TheOne.__SelectByWorkspaceNode);
            queryByExtent.set(_DataViews._SelectByWorkspaceNode.workspaceId, "Data");

            var queryFlatten = factory.create(_DataViews.TheOne.__FlattenNode);
            var queryByMetaClass = factory.create(_DataViews.TheOne.__RowFilterByMetaclassNode);

            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByExtent);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByMetaClass);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryFlatten);
            queryStatement.set(_DataViews._QueryStatement.resultNode, queryByMetaClass);

            dropDownByQueryData.set(_Forms._DropDownByQueryData.query, queryStatement);

            queryFlatten.set(_DataViews._FlattenNode.input, queryByExtent);

            queryByMetaClass.set(_DataViews._RowFilterByMetaclassNode.input, queryFlatten);
            queryByMetaClass.set(
                _DataViews._RowFilterByMetaclassNode.metaClass,
                _DataViews.TheOne.__ViewNode);
            return (scope, queryByMetaClass);
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