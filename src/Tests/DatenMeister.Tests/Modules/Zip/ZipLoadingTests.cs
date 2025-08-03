using DatenMeister.Actions;
using DatenMeister.Types;
using DatenMeister.Zip.Logic;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Zip;

[TestFixture]
public class ZipLoadingTests
{
    [Test]
    public async Task TestZipLoading()
    {
        // Check that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var localTypeSupport = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage);
        var localTypeExtent = localTypeSupport.GetInternalTypeExtent();

        // Find item
        var element = localTypeExtent.element(DatenMeister.Zip.Model._Root.TheOne.__ZipFileExtractAction.Uri);
        Assert.That(element, Is.Not.Null);
    }

    [Test]
    public async Task TestExistingOfActionHandler()
    {
        // Check that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var actionLogicState = dm.ScopeStorage.Get<ActionLogicState>();
        Assert.That(actionLogicState.ActionHandlers.Any(x => x is ZipLogicActionHandler), Is.True);
    }
}