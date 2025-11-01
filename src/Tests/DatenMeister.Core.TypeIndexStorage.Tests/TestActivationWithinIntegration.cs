using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestActivationWithinIntegration
{
    [Test]
    public async Task TestActivation()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore, Is.Not.Null);
    }

    [Test]
    public async Task TestExistingOfIndexStore()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        
        Assert.That(typeIndexStore?.GetCurrentIndexStore(), Is.Not.Null);
    }
}