namespace DatenMeister.Core.Provider.InMemory;

internal interface ICanUpdateCacheId
{
    internal void UpdateCachedId(InMemoryObject inMemoryObject, string? formerId);
}