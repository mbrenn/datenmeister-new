namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Provides predefined copy option configurations for common copying scenarios.
/// </summary>
public static class CopyOptions
{
    /// <summary>
    /// Gets the default copy options which create new ids for each copied element.
    /// This is the standard behavior for creating independent copies.
    /// </summary>
    public static CopyOption None { get; } = new()
    {
        PredicateToClone = CopyOption.PredicateForUmlCopying
    };

    /// <summary>
    /// Gets the copy options which also copies the ids from source to target elements.
    /// Use this when preserving original identifiers is required.
    /// </summary>
    public static CopyOption CopyId => new() {CopyId = true};
}