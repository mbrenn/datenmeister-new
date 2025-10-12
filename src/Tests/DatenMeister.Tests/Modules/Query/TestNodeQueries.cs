using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Query;

[TestFixture]
public class TestNodeQueries
{
    [Test]
    public async Task TestReferenceQuery()
    {
        var extentAndQuery = await TestFlattening.SetupExtentAndQuery();
        var scope = extentAndQuery.scope;

        var extent = extentAndQuery.scope.WorkspaceLogic.FindExtent(TestFlattening.TestExtentUri)
                     ?? throw new InvalidOperationException("Extent not found");

        var factory = new MofFactory(extent);
        var selectWorkspace = factory.create(_DataViews.TheOne.Source.__SelectByWorkspaceNode);
        selectWorkspace.set(_DataViews._Source._SelectByWorkspaceNode.workspaceId, WorkspaceNames.WorkspaceData);
        extent.elements().add(selectWorkspace);

        var uriOfSelectWorkspace = selectWorkspace.GetUri();
        var referenceQuery = factory.create(_DataViews.TheOne.Node.__ReferenceViewNode);
        referenceQuery.set(_DataViews._Node._ReferenceViewNode.workspaceId, WorkspaceNames.WorkspaceData);
        referenceQuery.set(_DataViews._Node._ReferenceViewNode.itemUri, uriOfSelectWorkspace);
        referenceQuery.set("name", "This is a query");

        extent.elements().add(referenceQuery);

        var viewLogic = new DataView.DataViewEvaluation(scope.WorkspaceLogic, scope.ScopeStorage);

        var resultingNodes =
            viewLogic.GetElementsForViewNode(referenceQuery).OfType<IElement>().ToList();
        Assert.That(
            resultingNodes.Any(
                x => x.getOrDefault<string>("name") == "This is a query"));
    }
}