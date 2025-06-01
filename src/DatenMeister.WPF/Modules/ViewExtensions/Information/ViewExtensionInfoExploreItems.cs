using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

public class ViewExtensionInfoExploreItems(INavigationHost navigationHost, INavigationGuest? navigationGuest)
    : ViewExtensionInfoExplore(navigationHost, navigationGuest)
{
    public string WorkspaceId { get; set; } = string.Empty;

    public string ExtentUrl { get; set; } = string.Empty;
}