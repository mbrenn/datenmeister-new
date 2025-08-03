using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.Helper;

/// <summary>
///     Defines a class being used as an internal cache for the form creation.
///     This improves the speed of form creation since some state are explicitly stored in the
///     cache instead of being required to be evaluated out of the field structure.
/// </summary>
public class FormCreatorCache
{
    /// <summary>
    ///     True, if the metaclass has been already covered
    /// </summary>
    public bool MetaClassAlreadyAdded { get; set; }

    /// <summary>
    /// The meta classes that have been covered
    /// </summary>
    public HashSet<IElement> CoveredMetaClasses { get; } = new();

    /// <summary>
    ///     The property names that already have been covered and are within the
    /// </summary>
    public HashSet<string> CoveredPropertyNames { get; } = new();

    /// <summary>
    ///     When at least one value is set, only those properties will be added which
    ///     are added to this hashset.
    /// </summary>
    public HashSet<string> FocusOnPropertyNames { get; } = new();
}