using System.Threading.Tasks;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Zip;

[TestFixture]
public class ZipLoadingTests
{
    [Test]
    public async Task TestZipLoading()
    {
        // Check, that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        
        var localTypeSupport = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage);
        var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
        
        // Find item
        var element = localTypeExtent.element(DatenMeister.Zip.Model._Root.TheOne.__ZipFileExtractAction.Uri);
        Assert.That(element, Is.Not.Null);
    }
}