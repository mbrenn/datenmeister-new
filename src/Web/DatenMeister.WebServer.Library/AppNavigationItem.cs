namespace DatenMeister.WebServer.Library;

public class AppNavigationItem
{
    /// <summary>
    /// Gets or sets the title of the navigation item
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the url of the item
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the url to the image
    /// </summary>
    public string Image { get; set; } = string.Empty;
}