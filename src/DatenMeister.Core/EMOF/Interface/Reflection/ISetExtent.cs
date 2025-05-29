using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Interface.Reflection;

public interface ISetExtent
{
    /// <summary>
    /// Adds the element to the given extent
    /// </summary>
    /// <param name="extent">Extent to be added</param>
    void SetExtent(IExtent extent);
}