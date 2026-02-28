using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Defines options that control the behavior of the object copying process.
/// These options allow fine-grained control over ID handling, reference cloning,
/// recursion depth, and custom copy predicates.
/// </summary>
public class CopyOption
{
    /// <summary>
    /// Gets or sets a value indicating whether the IDs of the objects shall be copied
    /// or whether a new ID shall be generated for each copied element.
    /// Default is false (new IDs are generated).
    /// </summary>
    public bool CopyId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all references shall be cloned,
    /// preventing the creation of URI references to external extents.
    /// When true, all referenced objects are copied rather than referenced.
    /// Default is false.
    /// </summary>
    public bool CloneAllReferences { get; set; }

    /// <summary>
    /// Gets or sets the predicate function that determines whether a given object should be cloned
    /// during the copy process. The predicate is evaluated based on specific parameters such as the
    /// source object, property name, and target object. When this predicate returns true,
    /// the object will be forcefully copied regardless of other options.
    /// </summary>
    /// <remarks>
    /// The predicate function receives a CopyParameters struct containing:
    /// - SourceObject: The parent object containing the property being copied
    /// - ObjectToBeCopied: The value being evaluated for copying
    /// - PropertyName: The name of the property being copied
    /// - TargetObject: The destination object that will receive the copied value
    ///
    /// Return true to force a deep copy, or false to follow the default copy behavior.
    /// Use GetPredicateForUmlCopying() to obtain a predicate optimized for UML model copying.
    /// </remarks>
    public Predicate<CopyParameters>? PredicateToClone { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only primitive elements shall be copied
    /// without any recursive copying of nested objects. When true, complex nested structures
    /// are not traversed. Default is false.
    /// </summary>
    public bool NoRecursion { get; set; }

    /// <summary>
    /// Creates a predicate function optimized for UML model copying that makes intelligent decisions
    /// about when to perform deep copies versus maintaining references.
    /// </summary>
    /// <param name="copyPredicateParameter">
    /// Configuration parameters that control the predicate's behavior regarding extent boundaries,
    /// workspace boundaries, and temporary extent handling. Uses default values if not specified.
    /// </param>
    /// <returns>
    /// A predicate function that can be assigned to PredicateToClone. This function receives
    /// CopyParameters and returns true to force a deep copy, or false to maintain a reference.
    /// </returns>
    /// <remarks>
    /// This predicate implements UML-aware copying logic that considers:
    ///
    /// 1. **Null extent handling**: Always copies if source or target extent is null,
    ///    since elements without extents cannot be reliably referenced.
    ///
    /// 2. **Cross-extent copying**: If CopyAcrossExtents is enabled, copies when source
    ///    and target extents differ.
    ///
    /// 3. **Temporary extent handling**: If CopyFromTemporaryExtent is enabled (default: true),
    ///    copies objects from temporary extents to permanent extents.
    ///
    /// 4. **Cross-workspace copying**: If CopyAcrossWorkspaces is enabled (default: true),
    ///    copies objects when source and target workspaces differ, avoiding problematic
    ///    cross-workspace references.
    ///
    /// 5. **Composite relationships**: Always copies composite properties (UML composition),
    ///    where the contained object is considered part of the container's lifecycle.
    ///
    /// The predicate evaluates these conditions in order and returns true for the first
    /// matching condition. If no condition matches, it returns false (use reference).
    /// </remarks>
    /// <example>
    /// <code>
    /// // Use with default parameters (copy across workspaces and from temporary extents)
    /// var option = new CopyOption
    /// {
    ///     PredicateToClone = CopyOption.GetPredicateForUmlCopying()
    /// };
    ///
    /// // Use with custom parameters to also copy across extents
    /// var customParam = new CopyPredicateParameter
    /// {
    ///     CopyAcrossExtents = true,
    ///     CopyAcrossWorkspaces = true,
    ///     CopyFromTemporaryExtent = true
    /// };
    /// var option2 = new CopyOption
    /// {
    ///     PredicateToClone = CopyOption.GetPredicateForUmlCopying(customParam)
    /// };
    /// </code>
    /// </example>
    public static Predicate<CopyParameters> GetPredicateForUmlCopying(CopyPredicateParameter copyPredicateParameter = default)
    {
        return parameters =>
        {
            var sourceExtent = (parameters.SourceObject as IHasExtent)?.Extent as IUriExtent;
            var targetExtent = (parameters.TargetObject as MofObject)?.ReferencedExtent as IUriExtent;

            // First, check, if the source or target extent is null. If that is the case, then
            // we will copy because we will never find the values again!
            if (sourceExtent == null || targetExtent == null)
                return true;

            if (copyPredicateParameter.CopyAcrossExtents)
            {
                if (sourceExtent != targetExtent)
                    return true;
            }

            if (copyPredicateParameter.CopyFromTemporaryExtent)
            {
                if (sourceExtent.contextURI() == WorkspaceNames.UriTemporaryExtent
                    && targetExtent.contextURI() != WorkspaceNames.UriTemporaryExtent)
                    return true;
            }

            if (copyPredicateParameter.CopyAcrossWorkspaces)
            {
                // Ok, now we try to get the workspaces. If the workspaces are different, then we will for
                // sure copy, because we don't like references across workspaces. These won't work
                var sourceWorkspace = (sourceExtent as IHasWorkspace)?.Workspace;
                var targetWorkspace = (targetExtent as IHasWorkspace)?.Workspace;

                if (sourceWorkspace != null && targetWorkspace != null && sourceWorkspace != targetWorkspace)
                    return true;
            }

            // Ok, now we figure out, if we are a composite. If yes, then perform a copy
            if (parameters.SourceObject is not MofObject sourceObject)
            {
                // In case we are not a MofObject, we fall back to not copy
                return false;
            }

            var attribute = sourceObject.GetClassModel()?.FindAttribute(parameters.PropertyName);
            return attribute is { IsComposite: true };
        };
    }
}

/// <summary>
/// Configuration parameters that control the behavior of the UML copying predicate
/// returned by GetPredicateForUmlCopying(). These parameters determine when objects
/// should be deep copied versus referenced based on extent and workspace boundaries.
/// </summary>
/// <remarks>
/// This struct is used in conjunction with GetPredicateForUmlCopying() to customize
/// how the predicate decides whether to copy or reference objects. The parameters
/// affect decisions based on:
/// - Extent boundaries (same vs. different extents)
/// - Workspace boundaries (same vs. different workspaces)
/// - Temporary extent handling (copying from temporary to permanent storage)
///
/// Default values are optimized for typical UML model copying scenarios where
/// cross-workspace references are problematic and temporary data should be
/// materialized into permanent storage.
/// </remarks>
/// <example>
/// <code>
/// // Use default behavior
/// var predicate1 = CopyOption.GetPredicateForUmlCopying();
///
/// // Copy everything across extent boundaries
/// var param = new CopyPredicateParameter
/// {
///     CopyAcrossExtents = true,
///     CopyAcrossWorkspaces = true,
///     CopyFromTemporaryExtent = true
/// };
/// var predicate2 = CopyOption.GetPredicateForUmlCopying(param);
/// </code>
/// </example>
public struct CopyPredicateParameter
{
    /// <summary>
    /// Initializes a new instance of CopyPredicateParameter with default values.
    /// </summary>
    public CopyPredicateParameter()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether objects should be copied when
    /// the source and target are in different extents.
    /// Default is false.
    /// </summary>
    /// <remarks>
    /// When true, any property value referencing an object in a different extent
    /// will be deep copied instead of maintaining the cross-extent reference.
    /// This is useful when you want to create completely self-contained copies
    /// that don't depend on other extents.
    ///
    /// When false (default), cross-extent references are preserved unless other
    /// conditions (like CopyAcrossWorkspaces or composite relationships) force copying.
    /// </remarks>
    public bool CopyAcrossExtents = false;

    /// <summary>
    /// Gets or sets a value indicating whether objects should be copied when
    /// the source and target are in different workspaces.
    /// Default is true.
    /// </summary>
    /// <remarks>
    /// When true (default), any property value referencing an object in a different
    /// workspace will be deep copied. This prevents problematic cross-workspace references
    /// which often don't work correctly due to workspace isolation.
    ///
    /// When false, cross-workspace references are maintained. Use with caution, as
    /// cross-workspace references may not resolve correctly in many scenarios.
    ///
    /// This setting only applies when both source and target have identifiable workspaces.
    /// </remarks>
    public bool CopyAcrossWorkspaces = true;

    /// <summary>
    /// Gets or sets a value indicating whether objects should be copied when
    /// moving from a temporary extent to a non-temporary extent.
    /// Default is true.
    /// </summary>
    /// <remarks>
    /// When true (default), objects from temporary extents (WorkspaceNames.UriTemporaryExtent)
    /// are always deep copied when the target is not a temporary extent. This ensures that
    /// temporary data is materialized into permanent storage rather than creating references
    /// to transient objects that may be cleaned up.
    ///
    /// When false, references to temporary extent objects are maintained. This is rarely
    /// desired as temporary extents are typically short-lived.
    ///
    /// Copying from non-temporary to temporary extents is not affected by this setting.
    /// </remarks>
    public bool CopyFromTemporaryExtent = true;
}