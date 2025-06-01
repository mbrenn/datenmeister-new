using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

/// <summary>
/// Contains the information about the window or dialog in which the viewextension will be shown
/// </summary>
public class ViewExtensionInfo(INavigationHost navigationHost, INavigationGuest? navigationGuest)
{
    /// <summary>
    /// Gets or sets the navigation host querying the view extensions
    /// </summary>
    public INavigationHost NavigationHost { get; set; } = navigationHost;

    /// <summary>
    /// Gets or sets the navigation guest which is currently
    /// </summary>
    public INavigationGuest? NavigationGuest { get; set; } = navigationGuest;
}