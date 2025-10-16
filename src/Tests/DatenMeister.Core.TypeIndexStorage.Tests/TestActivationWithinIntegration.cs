using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestActivationWithinIntegration
{
    [Test]
    public async Task TestActivation()
    {
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore, Is.Not.Null);
    }
}