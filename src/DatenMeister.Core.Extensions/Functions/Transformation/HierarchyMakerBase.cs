using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Extensions.Functions.Transformation;

public abstract class HierarchyMakerBase
{
    /// <summary>
    /// Gets or sets the extent that is used to convert the elements
    /// </summary>
    public IReflectiveSequence? Sequence { get; set; }

    /// <summary>
    /// Gets or sets the target extent that shall retrieve the converted elements.
    /// If this element is null, then the original extent will be modified
    /// </summary>
    public IReflectiveSequence? TargetSequence { get; set; }

    /// <summary>
    /// Gets or sets the factory being used to create the objects in the target extent
    /// </summary>
    public IFactory? TargetFactory { get; set; }

    /// <summary>
    /// Gets or sets the name of the column containing the id
    /// </summary>
    public string IdColumn { get; set; } = "id";
}