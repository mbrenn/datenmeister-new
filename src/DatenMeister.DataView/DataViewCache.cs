using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.DataView;

public class DataViewCache
{
    public bool IsDirty { get; set; }

    public List<IElement> CachedDataViews { get; } = [];

    public void MarkAsDirty()
    {
        IsDirty = true;
    }
}