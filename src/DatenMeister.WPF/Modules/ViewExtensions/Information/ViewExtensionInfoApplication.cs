using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

/// <summary>
/// Implements the view extension when queried by the application itself
/// </summary>
public class ViewExtensionInfoApplication(INavigationHost navigationHost, INavigationGuest? navigationGuest)
    : ViewExtensionInfo(navigationHost, navigationGuest);