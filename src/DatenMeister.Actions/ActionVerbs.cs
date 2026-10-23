namespace DatenMeister.Actions;

/// <summary>
/// Defines a default set of action verbs that can be used.
/// Additional action verbs can be added to this list or directly used without registration
/// </summary>
public static class ActionVerbs
{
    /// <summary>
    /// Defines that we need an execution
    /// </summary>
    public const string Execute = "execute";
    
    /// <summary>
    /// Defines that we need a rendering
    /// </summary>
    public const string Render = "render";
}