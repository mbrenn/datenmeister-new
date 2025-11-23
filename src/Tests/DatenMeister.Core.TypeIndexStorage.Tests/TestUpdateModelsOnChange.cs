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
    
}