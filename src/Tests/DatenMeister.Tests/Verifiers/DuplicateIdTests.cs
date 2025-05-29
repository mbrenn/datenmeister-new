using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Verifier;
using DatenMeister.Extent.Verifier.Verifiers;
using DatenMeister.TemporaryExtent;
using NUnit.Framework;

namespace DatenMeister.Tests.Verifiers;

[TestFixture]
public class DuplicateIdTests
{
    [Test]
    public async Task TestDuplicateId()
    {
        var scopeStorage = new ScopeStorage();
        var workspaceLogic = new WorkspaceLogic(scopeStorage);
        var temporaryExtentLogic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);

        var dataWorkspace = workspaceLogic.AddWorkspace(new Workspace("Data"));
        temporaryExtentLogic.CreateTemporaryExtent();
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
        dataWorkspace.AddExtent(extent);

        var factory = new MofFactory(extent);

        var verifier = new Verifier(workspaceLogic, scopeStorage);
        verifier.AddVerifier(() => new DuplicateIdVerifier(workspaceLogic));

        await verifier.VerifyExtents();
        Assert.That(verifier.VerifyEntries.Count, Is.EqualTo(0));

        var firstItem = factory.create(null);
        var secondItem = factory.create(null);
        extent.elements().add(firstItem);
        extent.elements().add(secondItem);
            
        await verifier.VerifyExtents();
        Assert.That(verifier.VerifyEntries.Count, Is.EqualTo(0));

        var thirdItem = factory.create(null);
        var fourthItem = factory.create(null);
        (thirdItem as ICanSetId)!.Id = "123";
        (fourthItem as ICanSetId)!.Id = "123";
        extent.elements().add(thirdItem);
        extent.elements().add(fourthItem);
            
        await verifier.VerifyExtents();
        Assert.That(verifier.VerifyEntries.Count, Is.GreaterThan(0));
    }
}