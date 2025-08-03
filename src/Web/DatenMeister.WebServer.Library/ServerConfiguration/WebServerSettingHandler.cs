namespace DatenMeister.WebServer.Library.ServerConfiguration;

/// <summary>
/// A supporting class to ease the access to the
/// webserver settings. Especially when it comes
/// </summary>
public class WebServerSettingHandler
{
    private WebServerSettings? _webServerSettings;

    public WebServerSettings WebServerSettings
    {
        get => _webServerSettings ??= new WebServerSettings();
        set => _webServerSettings = value;
    }

    private static WebServerSettingHandler? _theOne;

    /// <summary>
    /// Defines the one webserver setting handler which is available for global access
    /// </summary>
    public static WebServerSettingHandler TheOne => 
        _theOne ??= new WebServerSettingHandler();

    /// <summary>
    /// Gets the css style string to be used for the webpage
    /// </summary>
    public string BodyCssStyle
    {
        get
        {
            if (!string.IsNullOrEmpty(WebServerSettings.backgroundColor))
            {
                return $"background-color: {WebServerSettings.backgroundColor}";
            }

            return string.Empty;
        }    
    }
        
}