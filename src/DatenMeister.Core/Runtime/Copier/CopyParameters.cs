using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Encapsulates the parameters passed to the PredicateToClone function in CopyOption.
/// Used to provide context information for determining whether an object should be cloned.
/// </summary>
public struct CopyParameters
{
    /// <summary>
    /// Gets or sets the source object from which the property is being copied.
    /// </summary>
    public object SourceObject { get; set; }

    /// <summary>
    /// Gets or sets the object that is being evaluated for copying.
    /// </summary>
    public object ObjectToBeCopied { get; set; }

    /// <summary>
    /// Gets or sets the name of the property being copied.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the target object that will receive the copied property.
    /// </summary>
    public IObject TargetObject { get; set; }
}