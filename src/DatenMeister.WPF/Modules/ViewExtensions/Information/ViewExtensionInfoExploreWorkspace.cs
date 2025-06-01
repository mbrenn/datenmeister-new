using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

public class ViewExtensionInfoExploreWorkspace(INavigationHost navigationHost, INavigationGuest? navigationGuest)
    : ViewExtensionInfoExplore(navigationHost, navigationGuest);