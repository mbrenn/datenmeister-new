using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests;

[TestFixture]
public class WorkspaceTests
{
    [Test]
    public void TestFindByUri()
    {
        var workspaceLogic = WorkspaceLogic.Create(new WorkspaceData());
        var workspace = new Workspace("data", "No annotation");
        workspaceLogic.AddWorkspace(workspace);

        var extent = new MofUriExtent(new InMemoryProvider(), "http://test/", null);
        var factory = new MofFactory(extent);
        var element = factory.create(null);
        extent.elements().add(element);

        workspaceLogic.AddExtent(workspace, extent);

        var uri = extent.uri(element);

        // Now check, if everything is working
        var found = extent.element(uri);
        Assert.That(found, Is.EqualTo(element));

        var anotherFound = workspace.FindObject(uri);
        Assert.That(anotherFound, Is.EqualTo(element));
    }

    [Test]
    public void TestWorkspaceConfiguration()
    {
        var workspace = new Workspace("data", "No annotation");
        Assert.That(workspace.id, Is.EqualTo("data"));
        Assert.That(workspace.annotation, Is.EqualTo("No annotation"));
    }

    [Test]
    public void CheckDependabilityOfWorkspaces()
    {
        var workspaceData = WorkspaceLogic.InitDefault();
        var workspaceLogic = WorkspaceLogic.Create(workspaceData);

        var workspaces = workspaceLogic.GetWorkspacesOrderedByDependability(ResolveType.Default).ToList();

        Assert.That(workspaces.First().id, Is.EqualTo("Data").Or.EqualTo("Management"));
        Assert.That(workspaces.ElementAt(workspaces.Count - 2).id, Is.EqualTo("UML"));
        Assert.That(workspaces.Last().id, Is.EqualTo("MOF"));


        workspaces = workspaceLogic.GetWorkspacesOrderedByDependability(ResolveType.OnlyMetaWorkspaces).ToList();

        Assert.That(workspaces.ElementAt(workspaces.Count - 2).id, Is.EqualTo("UML"));
        Assert.That(workspaces.Last().id, Is.EqualTo("MOF"));
        Assert.That(workspaces.Any(x=>x.id == WorkspaceNames.WorkspaceData), Is.False);
        Assert.That(workspaces.Any(x=>x.id == WorkspaceNames.WorkspaceManagement), Is.False);
    }
}