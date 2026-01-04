using DatenMeister.Core.Interfaces;
using DatenMeister.Tests;
using NUnit.Framework;

namespace IssueMeisterLib.Tests;

[TestFixture]
public class TestMigration
{
    [Test]
    public async Task TestMigrationOfIssue()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var result1 = dm.WorkspaceLogic.Resolve("dm:///_internal/types/internal#IssueMeister.Issue",
            ResolveType.IncludeTypeWorkspace, true);
        var result2 = dm.WorkspaceLogic.Resolve(IssueMeisterPlugin.DmInternTypesDomainsDatenmeister + "#IssueMeister.Issue",
            ResolveType.IncludeTypeWorkspace, true);
        
        Assert.That(result1, Is.Not.Null);
        Assert.That(result2, Is.Not.Null);
    }
}