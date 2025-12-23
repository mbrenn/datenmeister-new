using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.DataView;

/// <summary>
/// Provides a cache for the data views
/// </summary>
public class DataViewCache
{
    /// <summary>
    /// Gets or sets a value indicating whether the cache is dirty and needs to be rebuilt
    /// </summary>
    public bool IsDirty { get; set; } = true;

    /// <summary>
    /// Gets the list of cached data views
    /// </summary>
    public List<IElement> CachedDataViews { get; } = [];

    /// <summary>
    /// Marks the cache as dirty
    /// </summary>
    public void MarkAsDirty()
    {
        IsDirty = true;
    }
}