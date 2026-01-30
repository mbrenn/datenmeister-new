namespace DatenMeister.WebServer.Library.ServerConfiguration;

/// <summary>
/// Defines the available server settings
/// </summary>
public class WebServerSettings
{
    /// <summary>
    /// Gets or sets the background-color
    /// </summary>
    public string designBackgroundColor { get; set; } = string.Empty;
    
    /// <summary>
    /// Sets the information whether the side toolbar shall be hidden or not.
    /// </summary>
    public bool layoutHideSideToolbar { get; set; } = false;

    public bool layoutHideTopToolbar { get; set; } = false;

    public string startPage { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the flag whether the webserver shall listen to all IPs or just on localhost
    /// </summary>
    public bool webServerIsPublic { get; set; } = false;
    
    /// <summary>
    /// Defines to use the secure connection via https. It is recommended to set the certificate
    /// into the appsettings.json file.
    /// </summary>
    public bool webServerUseHttps { get; set; } = false;
}