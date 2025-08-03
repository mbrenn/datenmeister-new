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

[TestFixture]
public class TestWhenMetaClassIs
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
        queryByMetaClass.set(_DataViews._FilterByMetaclassNode.includeInherits, true);

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
            var queryByMetaClass = factory.create(_DataViews.TheOne.__FilterByMetaclassNode);

            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByExtent);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByMetaClass);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryFlatten);
            queryStatement.set(_DataViews._QueryStatement.resultNode, queryByMetaClass);

            dropDownByQueryData.set(_Forms._DropDownByQueryData.query, queryStatement);

            queryFlatten.set(_DataViews._FlattenNode.input, queryByExtent);

            queryByMetaClass.set(_DataViews._FilterByMetaclassNode.input, queryFlatten);
            queryByMetaClass.set(
                _DataViews._FilterByMetaclassNode.metaClass,
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