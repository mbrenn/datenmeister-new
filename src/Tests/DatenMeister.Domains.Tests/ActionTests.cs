using NUnit.Framework;

namespace DatenMeister.Domains.Tests;

[TestFixture]
public class ActionTests
{
    [Test]
    public async Task CheckThatDomainsAreLoaded()
    {
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        
        
        await Task.CompletedTask;
    }
}