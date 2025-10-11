using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
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

[TestFixture]
public class TestColumnFilters
{
    [Test]
    public async Task TestInfrastructure()
    {
        var (scope, queryExtent, inputQuery) = await SetupExtentAndQuery();
        
        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        var result = viewLogic.GetElementsForViewNode(inputQuery).OfType<IElement>().ToList();
        Assert.That(result.Count, Is.GreaterThan(0));
        foreach (var row in result)
        {
            Assert.That(row.isSet("name"), Is.True);
            Assert.That(row.isSet("age"), Is.True);
            Assert.That(row.isSet("iq"), Is.True);
        }
    }

    [Test]
    public async Task TestExcludeColumns()
    {
        var (scope, queryExtent, inputQuery) = await SetupExtentAndQuery();
        var queryFactory = new MofFactory(queryExtent);
        var filterColumns = queryFactory.create(_DataViews.TheOne.Column.__ColumnFilterExcludeNode);
        queryExtent.elements().add(filterColumns);
        filterColumns.set(_DataViews._Column._ColumnFilterExcludeNode.columnNamesComma, "age,iq");
        filterColumns.set(_DataViews._Column._ColumnFilterExcludeNode.input, inputQuery);
        
        
        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        var result = viewLogic.GetElementsForViewNode(filterColumns).OfType<IElement>().ToList();
        Assert.That(result.Count, Is.GreaterThan(0));
        foreach (var row in result)
        {
            Assert.That(row.isSet("name"), Is.True);
            Assert.That(row.isSet("age"), Is.False);
            Assert.That(row.isSet("iq"), Is.False);
        }
    }

    [Test]
    public async Task TestIncludeColumnsOnly()
    {
        var (scope, queryExtent, inputQuery) = await SetupExtentAndQuery();
        var queryFactory = new MofFactory(queryExtent);
        var filterColumns = queryFactory.create(_DataViews.TheOne.Column.__ColumnFilterIncludeOnlyNode);
        queryExtent.elements().add(filterColumns);
        filterColumns.set(_DataViews._Column._ColumnFilterIncludeOnlyNode.columnNamesComma, "name, age");
        filterColumns.set(_DataViews._Column._ColumnFilterIncludeOnlyNode.input, inputQuery);
        
        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);
        var result = viewLogic.GetElementsForViewNode(filterColumns).OfType<IElement>().ToList();
        Assert.That(result.Count, Is.GreaterThan(0));
        foreach (var row in result)
        {
            Assert.That(row.isSet("name"), Is.True);
            Assert.That(row.isSet("age"), Is.True);
            Assert.That(row.isSet("iq"), Is.False);
        }
    }
    

    private static async Task<(IDatenMeisterScope scope, IUriExtent queryExtent, IElement inputQuery)> SetupExtentAndQuery()
    {
        IDatenMeisterScope? scope = null;
        try
        {
            scope = await DatenMeisterTests.GetDatenMeisterScope();

            // Create the Target Extent
            var extentManager = scope.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
            loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///query");
            loaderConfig.set(
                _ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);

            var loadedExtentInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedExtentInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            var extent = loadedExtentInfo.Extent!;
            var factory = new MofFactory(extent);

            // Create the data!
            var viewNode = factory.create(_DataViews.TheOne.__ViewNode);
            var selectWorkspace = factory.create(_DataViews.TheOne.Source.__SelectByExtentNode);
            selectWorkspace.set(_DataViews._Source._SelectByExtentNode.workspaceId, "Data");
            selectWorkspace.set(_DataViews._Source._SelectByExtentNode.extentUri, "dm:///test");
            extent.elements().add(viewNode);
            extent.elements().add(selectWorkspace);
            
            // Loads the data itself
            var dataLoaderConfig =
                InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
            dataLoaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            dataLoaderConfig.set(
                _ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);

            var dataLoadedExtentInfo = await extentManager.LoadExtent(dataLoaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(dataLoadedExtentInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            var dataExtent = dataLoadedExtentInfo.Extent!;
            var dataFactory = new MofFactory(extent);

            dataExtent.elements().add(
                dataFactory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person1", ["age"] = 18, ["iq"] = 110}));
            dataExtent.elements().add(
                dataFactory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person2", ["age"] = 15, ["iq"] = 120}));
            dataExtent.elements().add(
                dataFactory.create(null)
                    .SetProperties(new Dictionary<string, object>
                        {["name"] = "person3", ["age"] = 20, ["iq"] = 95 }));
            dataExtent.elements().add(
                dataFactory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person4", ["age"] = 20, ["iq"] = 100}));
            dataExtent.elements().add(
                dataFactory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person5", ["age"] = 20, ["iq"] = 109}));
            
            return (scope, extent, selectWorkspace);
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