using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Users.UserProperties;

public class ViewModeSelection
{
    /// <summary>
    /// Gets or sets the view mode of the user
    /// </summary>
    public IElement? viewMode { get; set; }

    /// <summary>
    /// Gets or sets the extent to which the user is belonging
    /// </summary>
    public string? extentUri { get; set; }

    /// <summary>
    /// Gets or sets just an additional tag that can be used for filtering
    /// </summary>
    public string? tag { get; set; }
}