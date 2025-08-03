namespace DatenMeister.WebServer.Library.PageRegistration;

/// <summary>
/// Defines the data that is handled
/// </summary>
public class PageRegistrationData
{
    /// <summary>
    /// Creates a list of page factories
    /// </summary>
    public List<PageFactory> PageFactories { get; } = new();

    /// <summary>
    /// Stores the list of javascript files to be added.
    /// </summary>
    public List<string> JavaScriptFiles { get; } = new();

    /// <summary>
    /// Stores the list of CSS Files
    /// </summary>
    public List<string> CssFiles { get; } = new();
}