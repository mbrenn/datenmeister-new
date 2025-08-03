using NUnit.Framework;

namespace BurnSystems.TimeCache.Tests;

[TestFixture]
public class TestTimeCachedDictionary
{
    [Test]
    public void TestTimeCaching()
    {
        var cache = new TimeCachedDictionary<string, string>()
        {
            CachingTime = TimeSpan.FromSeconds(0.1)
        };
        
        cache.Add("1", "2");
        cache.Add("2", "3");
        
        Assert.That(cache["1"], Is.EqualTo("2"));
        Assert.That(cache["2"], Is.EqualTo("3"));
        
        Thread.Sleep(150);
        
        Assert.That( cache.TryGetValue("1", out var _), Is.False);
        Assert.That(cache.ContainsKey("2"), Is.False);
    }

    [Test]
    public void TestInvalidateCache()
    {
        var cache = new TimeCachedDictionary<string, string>()
        {
            CachingTime = TimeSpan.FromSeconds(20)
        };
        
        cache.Add("1", "2");
        cache.Add("2", "3");
        
        Assert.That(cache["1"], Is.EqualTo("2"));
        Assert.That(cache["2"], Is.EqualTo("3"));
        
        cache.InvalidateCache();
        
        Assert.That( cache.TryGetValue("1", out var _), Is.False);
        Assert.That(cache.ContainsKey("2"), Is.False);
        
    }
}