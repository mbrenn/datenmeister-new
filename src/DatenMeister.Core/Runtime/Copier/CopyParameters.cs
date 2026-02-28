using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Encapsulates the parameters passed to the PredicateToClone function in CopyOption.
/// This struct provides comprehensive context information for determining whether an object
/// should be cloned during the copy operation. The predicate function receives this information
/// and returns true to force a deep copy, or false to follow the default copy behavior.
/// </summary>
/// <remarks>
/// The CopyParameters struct is used in conjunction with CopyOption.PredicateToClone to enable
/// fine-grained control over the copying process. This allows implementers to make copying decisions
/// based on the specific context of each property being copied, such as the source object,
/// the target object, the property name, and the value being copied.
/// </remarks>
/// <example>
/// Example usage with a predicate:
/// <code>
/// var copyOption = new CopyOption
/// {
///     PredicateToClone = parameters =>
///     {
///         // Force copy if the property name is "children"
///         return parameters.PropertyName == "children";
///     }
/// };
/// </code>
/// </example>
public struct CopyParameters
{
    /// <summary>
    /// Gets or sets the source object from which the property is being copied.
    /// This represents the parent object that contains the property being evaluated.
    /// </summary>
    /// <remarks>
    /// This property allows the predicate to make decisions based on the context of the source object.
    /// For example, you might want to force copying only for certain types of source objects.
    /// </remarks>
    public object SourceObject { get; set; }

    /// <summary>
    /// Gets or sets the object that is being evaluated for copying.
    /// This is the actual value of the property that may be copied or referenced.
    /// </summary>
    /// <remarks>
    /// This is typically an IElement or IReflectiveCollection. The predicate can examine this value
    /// to determine whether it should be deep copied. For example, you might want to force copying
    /// for objects that meet certain criteria (e.g., specific metaclass, certain property values).
    /// </remarks>
    public object ObjectToBeCopied { get; set; }

    /// <summary>
    /// Gets or sets the name of the property being copied from the source object.
    /// </summary>
    /// <remarks>
    /// This allows the predicate to make property-specific decisions. For example, you might
    /// want to always deep copy the "children" property but use references for the "parent" property.
    /// The property name corresponds to the MOF property name in the object model.
    /// </remarks>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the target object that will receive the copied property value.
    /// This is the destination object being constructed during the copy operation.
    /// </summary>
    /// <remarks>
    /// This property provides context about where the copied value will be placed.
    /// The predicate can use this to make decisions based on the target object's state,
    /// metaclass, or other properties. This is particularly useful for maintaining
    /// consistency in the target object structure.
    /// </remarks>
    public IObject TargetObject { get; set; }
}