using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Query;

[TestFixture]
public class TestFlattening
{
    public const string TestExtentUri = "dm:///test";

    [Test]
    public async Task TestFlatteningOfTypesExtent()
    {
        var (scope, queryFlattening) = await SetupExtentAndQuery();
        await using var datenMeisterScope = scope;

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);

        var resultingNodes =
            viewLogic.GetElementsForViewNode(queryFlattening).OfType<IElement>().ToList();
        Assert.That(resultingNodes.Count, Is.GreaterThan(1));
        Assert.That(resultingNodes.Any (x =>x.equals(_DataViews.TheOne.Source.__SelectByWorkspaceNode)));
    }

    public static async Task<(IDatenMeisterScope scope, IElement queryByMetaClass)> SetupExtentAndQuery()
    {
        IDatenMeisterScope? scope = null;
        try
        {
            scope = await DatenMeisterTests.GetDatenMeisterScope();

            // Create the Target Extent
            var extentManager = scope.Resolve<ExtentManager>();

            var (extent, viewExtent) = await TestRowFilters.LoadAndValidateExtents(extentManager);
            
            var factory = new MofFactory(extent);

            // Create the data!
            var viewNode = factory.create(_DataViews.TheOne.__ViewNode);
            var selectWorkspace = factory.create(_DataViews.TheOne.Source.__SelectByWorkspaceNode);
            extent.elements().add(viewNode);
            extent.elements().add(selectWorkspace);

            // Now create the query
            var dropDownByQueryData = factory.create(_Forms.TheOne.__DropDownByQueryData);
            var queryStatement = factory.create(_DataViews.TheOne.__QueryStatement);
            viewExtent.elements().add(queryStatement);

            var queryByExtent = factory.create(_DataViews.TheOne.Source.__SelectByExtentNode);
            queryByExtent.set(_DataViews._Source._SelectByExtentNode.workspaceId, "Types");
            queryByExtent.set(_DataViews._Source._SelectByExtentNode.extentUri, WorkspaceNames.UriExtentInternalTypes);
            
            var queryFlatten = factory.create(_DataViews.TheOne.Row.__RowFlattenNode);
            var queryByMetaClass = factory.create(_DataViews.TheOne.Row.__RowFilterByMetaclassNode);

            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByExtent);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByMetaClass);
            queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryFlatten);
            queryStatement.set(_DataViews._QueryStatement.resultNode, queryByMetaClass);

            dropDownByQueryData.set(_Forms._DropDownByQueryData.query, queryStatement);

            queryFlatten.set(_DataViews._Row._RowFlattenNode.input, queryByExtent);
            return (scope, queryFlatten);
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