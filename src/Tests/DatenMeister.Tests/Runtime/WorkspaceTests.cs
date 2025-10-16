using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Runtime.Workspaces.Data;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime;

[TestFixture]
public class WorkspaceTests
{
    [Test]
    public void TestStoreAndLoadEmptyWorkspaces()
    {
        // Stores an empty workspace
        var workSpaceCollection = WorkspaceLogic.GetEmptyLogic();
        var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
        workspaceLoader.Store();

        var newWorkSpaceCollection = WorkspaceLogic.GetEmptyLogic();
        workspaceLoader = new WorkspaceLoader(
            workSpaceCollection,
            new ScopeStorage().Add(
                new WorkspaceLoaderConfig
                (
                    DatenMeisterTests.GetPathForTemporaryStorage("workspaces.xml")
                )));
        workspaceLoader.Load();

        Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(0));
    }

    [Test]
    public void TestStoreAndLoadDefaultWorkspaces()
    {
        // Stores an empty workspace
        var workSpaceCollection = WorkspaceLogic.GetDefaultLogic();
        var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
        var nr = workSpaceCollection.Workspaces.Count();
        workspaceLoader.Store();

        var newWorkSpaceCollection = WorkspaceLogic.GetDefaultLogic();
        workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
        workspaceLoader.Load();

        Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(nr));
    }

    [Test]
    public void TestStoreAndLoadHavingTwoWorkspaces()
    {
        // Stores an empty workspace
        var workSpaceCollection = WorkspaceLogic.GetEmptyLogic();
        workSpaceCollection.AddWorkspace(new Workspace("test", "Continue"));
        workSpaceCollection.AddWorkspace(new Workspace("another", "annotation"));
        var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
        workspaceLoader.Store();

        var newWorkSpaceCollection = WorkspaceLogic.GetEmptyLogic();
        workspaceLoader = CreateWorkspaceLoader(newWorkSpaceCollection);
        workspaceLoader.Load();

        Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
        var first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "test");
        Assert.That(first, Is.Not.Null);
        Assert.That(first!.id, Is.EqualTo("test"));
        Assert.That(first.annotation, Is.EqualTo("Continue"));
        first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "another");
        Assert.That(first, Is.Not.Null);
        Assert.That(first!.id, Is.EqualTo("another"));
        Assert.That(first.annotation, Is.EqualTo("annotation"));

        Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
    }

    [Test]
    public void TestStoreAndLoadHavingTwoWorkspacesWithConflict()
    {
        // Stores an empty workspace
        var workSpaceCollection = WorkspaceLogic.GetEmptyLogic();
        workSpaceCollection.AddWorkspace(new Workspace("test", "Continue"));
        workSpaceCollection.AddWorkspace(new Workspace("another", "annotation"));
        var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
        workspaceLoader.Store();

        var newWorkSpaceCollection = WorkspaceLogic.GetEmptyLogic();
        newWorkSpaceCollection.AddWorkspace(new Workspace("test", "Continue"));
        workspaceLoader = CreateWorkspaceLoader(newWorkSpaceCollection);
        workspaceLoader.Load();

        Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
        var first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "test");
        Assert.That(first, Is.Not.Null);
        Assert.That(first!.id, Is.EqualTo("test"));
        Assert.That(first.annotation, Is.EqualTo("Continue"));
        first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "another");
        Assert.That(first, Is.Not.Null);
        Assert.That(first!.id, Is.EqualTo("another"));
        Assert.That(first.annotation, Is.EqualTo("annotation"));

        Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
    }

    [Test]
    public void OnTestWorkspaceEvents()
    {
        var workSpaceCollection = WorkspaceLogic.GetEmptyLogic() as WorkspaceLogic;
        Assert.That(workSpaceCollection, Is.Not.Null);

        var data = workSpaceCollection!.WorkspaceData;
        Assert.That(data, Is.Not.Null);

        var counter = 0;

        data.WorkspaceAdded += (_, _) => { counter++; };
        data.WorkspaceRemoved += (_, _) => { counter--; };

        Assert.That(counter, Is.EqualTo(0));

        workSpaceCollection.AddWorkspace(new Workspace("id", "anno"));
        Assert.That(counter, Is.EqualTo(1));
            
        workSpaceCollection.RemoveWorkspace("id");
        Assert.That(counter,Is.EqualTo(0));
            
        workSpaceCollection.RemoveWorkspace("id");
        Assert.That(counter,Is.EqualTo(0));
    }
        
    [Test]
    public void TestFindingOfAnExtentAndItem()
    {
        var workSpaceCollection = (WorkspaceLogic.GetDefaultLogic() as WorkspaceLogic)!;
        Assert.That(workSpaceCollection, Is.Not.Null);

        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///unittest", null);
        workSpaceCollection.GetDataWorkspace().AddExtent(extent);

        var item = MofFactory.CreateElement(extent, null);
        item.set("name", "Yes");
        (item as ICanSetId)!.Id = "yes"; 
        extent.elements().add(item);

        var found = workSpaceCollection.FindObject(WorkspaceNames.WorkspaceData, "dm:///unittest");
        Assert.That(found, Is.Not.Null);
        Assert.That(found, Is.InstanceOf<IUriExtent>());
        var extentFound = found as IUriExtent;
        Assert.That(extentFound!.contextURI(), Is.EqualTo("dm:///unittest"));

            
        var found2 = workSpaceCollection.FindObject(WorkspaceNames.WorkspaceData, "dm:///unittest#yes");
        Assert.That(found2, Is.Not.Null);
        Assert.That(found2.getOrDefault<string>("name"), Is.EqualTo("Yes"));
    }
        
    /// <summary>
    /// Creates a configured workspaceloader with defined filepath
    /// </summary>
    /// <param name="workSpaceCollection">Workspace Logic to be used</param>
    /// <returns>The created loader</returns>
    private static WorkspaceLoader CreateWorkspaceLoader(IWorkspaceLogic workSpaceCollection)
    {
        var workspaceLoader = new WorkspaceLoader(
            workSpaceCollection,
            new ScopeStorage().Add(
                new WorkspaceLoaderConfig(DatenMeisterTests.GetPathForTemporaryStorage("workspaces.xml")
                )));
        return workspaceLoader;
    }
}