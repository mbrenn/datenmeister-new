namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Provides predefined copy option configurations for common copying scenarios.
/// This static class offers convenient, pre-configured CopyOption instances that can be used
/// with the ObjectCopier to control copying behavior.
/// </summary>
/// <example>
/// Basic usage:
/// <code>
/// // Use default options
/// var copiedElement = ObjectCopier.Copy(factory, sourceElement, CopyOptions.None);
///
/// // Use copy with ID preservation
/// var copiedElement = ObjectCopier.Copy(factory, sourceElement, CopyOptions.CopyId);
///
/// // Customize with a predicate
/// var customOption = CopyOptions.None;
/// customOption.PredicateToClone = parameters =>
/// {
///     // Force copy for "children" property
///     return parameters.PropertyName == "children";
/// };
/// var copiedElement = ObjectCopier.Copy(factory, sourceElement, customOption);
/// </code>
/// </example>
public static class CopyOptions
{
    /// <summary>
    /// Gets the default copy options which create new ids for each copied element.
    /// This is the standard behavior for creating independent copies.
    /// Includes a predicate for UML-specific copying behavior.
    /// </summary>
    /// <remarks>
    /// This option uses GetPredicateForUmlCopying() which provides intelligent copying
    /// decisions for UML model elements. New IDs are generated for all copied elements,
    /// ensuring that the copies are distinct entities in the system.
    /// </remarks>
    public static CopyOption None { get; } = new()
    {
        PredicateToClone = CopyOption.GetPredicateForUmlCopying()
    };

    /// <summary>
    /// Gets the copy options which also copies the ids from source to target elements.
    /// Use this when preserving original identifiers is required, such as when creating
    /// synchronized replicas or maintaining identity relationships.
    /// </summary>
    public static CopyOption CopyId => new()
    {
        CopyId = true,
        PredicateToClone = CopyOption.GetPredicateForUmlCopying()
    };
}