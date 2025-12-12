using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestLoadWorkspaceAndExtents
{
    [Test]
    public async Task TestExistingOfRelevantWorkspaceAndExtents()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
            ?? throw new InvalidOperationException("TypeIndexStore not found");

        var typesWorkspace = 
            typeIndexStore.GetCurrentIndexStore().Workspaces.FirstOrDefault(x => x.WorkspaceId == "Types");
        Assert.That(typesWorkspace, Is.Not.Null);
        
        typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
        Assert.That(typesWorkspace, Is.Not.Null);
        Assert.That(typesWorkspace!.WorkspaceId, Is.EqualTo(WorkspaceNames.WorkspaceTypes));
        
        // Check, that the metaworkspace is existing
        Assert.That(typesWorkspace.MetaclassWorkspaces.Contains(WorkspaceNames.WorkspaceUml), Is.True);
        
        // Check, that the extent for the Types are in, we will have a flat list within all extents
        Assert.That(typesWorkspace.Extents.Count, Is.GreaterThan(0));
        Assert.That(
            typesWorkspace.Extents.Any(x=>x.Uri == WorkspaceNames.UriExtentInternalTypes),
            Is.True);
    }

    [Test]
    public async Task TestMetaWorkspaces()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");

        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
        Assert.That(typesWorkspace, Is.Not.Null);
        Assert.That(typesWorkspace!.MetaclassWorkspaces.Count, Is.GreaterThan(0));
        Assert.That(typesWorkspace.MetaclassWorkspaces, Does.Contain(WorkspaceNames.WorkspaceUml));
    }

    [Test]
    public async Task TestNeighborWorkspaces()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");

        var dataWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceData);
        Assert.That(dataWorkspace, Is.Not.Null);
        Assert.That(dataWorkspace!.NeighborWorkspaces.Count, Is.GreaterThan(0));
        Assert.That(dataWorkspace.NeighborWorkspaces, Does.Contain(WorkspaceNames.WorkspaceManagement));
        Assert.That(dataWorkspace.NeighborWorkspaces, Does.Not.Contain(WorkspaceNames.WorkspaceUml));
        Assert.That(dataWorkspace.NeighborWorkspaces, Does.Not.Contain(WorkspaceNames.WorkspaceData));
    }
}