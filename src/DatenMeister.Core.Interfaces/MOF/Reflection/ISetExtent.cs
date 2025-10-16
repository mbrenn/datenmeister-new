using DatenMeister.Core.Interfaces.MOF.Identifiers;

namespace DatenMeister.Core.Interfaces.MOF.Reflection;

public interface ISetExtent
{
    /// <summary>
    /// Adds the element to the given extent
    /// </summary>
    /// <param name="extent">Extent to be added</param>
    void SetExtent(IExtent extent);
}