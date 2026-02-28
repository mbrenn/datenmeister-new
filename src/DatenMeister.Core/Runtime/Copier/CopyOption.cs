using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Reflection;

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
    public Predicate<CopyParameters>? PredicateToClone { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only primitive elements shall be copied
    /// without any recursive copying of nested objects. When true, complex nested structures
    /// are not traversed. Default is false.
    /// </summary>
    public bool NoRecursion { get; set; }

    public static Predicate<CopyParameters> PredicateForUmlCopying
    {
        get
        {
            return parameters =>
            {
                var sourceExtent = (parameters.SourceObject as IHasExtent)?.Extent;
                var targetExtent = (parameters.TargetObject as IHasExtent)?.Extent;

                // First, check, if the source or target extent is null. If that is the case, then
                // we will copy because we will never find the values again! 
                if (sourceExtent == null || targetExtent == null)
                {
                    return true;
                }

                // Ok, now we try to get the workspaces. If the workspaces are different, then we will for 
                // sure copy, because we don't like references across workspaces. These won't work
                var sourceWorkspace = (sourceExtent as IHasWorkspace)?.Workspace;
                var targetWorkspace = (targetExtent as IHasWorkspace)?.Workspace;

                if (sourceWorkspace != null && targetWorkspace != null && sourceWorkspace != targetWorkspace)
                {
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
}