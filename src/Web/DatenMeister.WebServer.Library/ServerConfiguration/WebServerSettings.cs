namespace DatenMeister.WebServer.Library.ServerConfiguration;

/// <summary>
/// Defines the available server settings
/// </summary>
public class WebServerSettings
{
    /// <summary>
    /// Gets or sets the background-color
    /// </summary>
    public string backgroundColor { get; set; } = string.Empty;
    
    /// <summary>
    /// Sets the information whether the side toolbar shall be hidden or not.
    /// </summary>
    public bool hideSideToolbar { get; set; } = false;

    public bool hideTopToolbar { get; set; } = false;

    public string startPage { get; set; } = string.Empty;
}