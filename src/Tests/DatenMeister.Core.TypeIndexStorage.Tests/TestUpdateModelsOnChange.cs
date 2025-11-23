using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestUpdateModelsOnChange
{
    /// <summary>
    /// This tests checks that in case the Type Model is changed,
    /// that there is an update request being started.
    /// </summary>
    [Test]
    public async Task TestUpdateOnChange()
    {
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore, Is.Not.Null);
        typeIndexStore!.WaitForAvailabilityOfIndexStore();

        var updates = typeIndexStore.TriggersReceived;

        var typeExtent =
            dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceTypes, WorkspaceNames.UriExtentInternalTypes);
        Assert.That(typeExtent, Is.Not.Null);

        // Now change the type
        var element = typeExtent!.element("#DatenMeister.Models.Actions.CreateFormByMetaclass");
        Assert.That(element, Is.Not.Null);
        element!.set(_UML._StructuredClassifiers._Class.isAbstract, true);
        
        // We should have an update...
        var newUpdates = typeIndexStore.TriggersReceived;
        
        Assert.That(newUpdates, Is.EqualTo(updates + 1));
    }

    [Test]
    public async Task CheckThatUpdateIsHonored()
    {
        
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore, Is.Not.Null);
        typeIndexStore!.WaitForAvailabilityOfIndexStore();

        var updates = typeIndexStore.TriggersReceived;

        var typeExtent =
            dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceTypes, WorkspaceNames.UriExtentInternalTypes);
        Assert.That(typeExtent, Is.Not.Null);

        // Now change the type
        var element = typeExtent!.element("#DatenMeister.Models.Actions.CreateFormByMetaclass");
        Assert.That(element, Is.Not.Null);
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass");
        
        // We should have an update...
        var newUpdates = typeIndexStore.TriggersReceived;
        Assert.That(newUpdates, Is.EqualTo(updates + 1));
        
        // Wait until the index is up to date. Set Limitation of 5 seconds
        if (!typeIndexStore.IndexIsUpToDateEvent.WaitOne(TimeSpan.FromSeconds(5)))
        {
            Assert.Fail("Index is not up to date. The index was not updated properly");
        }
        
        // Check now that the new type is available
        var found = typeIndexStore.GetCurrentIndexStore()
            .Workspaces
            .FirstOrDefault(x => x.WorkspaceId == WorkspaceNames.WorkspaceTypes)
            ?.ClassModels
            .FirstOrDefault(x => x.Name == "CreateUpdatedFormByMetaclass");
        
        Assert.That(found, Is.Not.Null);
    }
    
    [Test]
    public async Task CheckThatMultipleUpdatesAreHonored()
    {
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore, Is.Not.Null);
        typeIndexStore!.WaitForAvailabilityOfIndexStore();

        var typeExtent =
            dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceTypes, WorkspaceNames.UriExtentInternalTypes);
        Assert.That(typeExtent, Is.Not.Null);

        // Now change the type
        var element = typeExtent!.element("#DatenMeister.Models.Actions.CreateFormByMetaclass");
        Assert.That(element, Is.Not.Null);
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass");
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass1");
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass2");
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass3");
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass4");
        
        // Wait until the index is up to date. Set Limitation of 5 seconds
        if (!typeIndexStore.IndexIsUpToDateEvent.WaitOne(TimeSpan.FromSeconds(5)))
        {
            Assert.Fail("Index is not up to date. The index was not updated properly");
        }
        
        // Check now that the new type is available
        var found = typeIndexStore.GetCurrentIndexStore()
            .Workspaces
            .FirstOrDefault(x => x.WorkspaceId == WorkspaceNames.WorkspaceTypes)
            ?.ClassModels
            .FirstOrDefault(x => x.Name == "CreateUpdatedFormByMetaclass4");
        
        Assert.That(found, Is.Not.Null);
    }
    
    [Test]
    public async Task CheckThatMultipleUpdatesAreHonoredWithTimeDelay()
    {
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore, Is.Not.Null);
        typeIndexStore!.WaitForAvailabilityOfIndexStore();

        var typeExtent =
            dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceTypes, WorkspaceNames.UriExtentInternalTypes);
        Assert.That(typeExtent, Is.Not.Null);

        // Now change the type
        var element = typeExtent!.element("#DatenMeister.Models.Actions.CreateFormByMetaclass");
        Assert.That(element, Is.Not.Null);
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass");
        await Task.Delay(TimeSpan.FromMilliseconds(400));
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass1");
        await Task.Delay(TimeSpan.FromMilliseconds(400));
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass2");
        await Task.Delay(TimeSpan.FromMilliseconds(400));
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass3");
        await Task.Delay(TimeSpan.FromMilliseconds(400));
        element!.set(_UML._StructuredClassifiers._Class.name, "CreateUpdatedFormByMetaclass4");
        await Task.Delay(TimeSpan.FromMilliseconds(400));
        
        // Wait until the index is up to date. Set Limitation of 5 seconds
        if (!typeIndexStore.IndexIsUpToDateEvent.WaitOne(TimeSpan.FromSeconds(5)))
        {
            Assert.Fail("Index is not up to date. The index was not updated properly");
        }
        
        // Check now that the new type is available
        var found = typeIndexStore.GetCurrentIndexStore()
            .Workspaces
            .FirstOrDefault(x => x.WorkspaceId == WorkspaceNames.WorkspaceTypes)
            ?.ClassModels
            .FirstOrDefault(x => x.Name == "CreateUpdatedFormByMetaclass4");
        
        Assert.That(found, Is.Not.Null);
    }
}