using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

public class ViewExtensionInfoExploreExtents(INavigationHost navigationHost, INavigationGuest? navigationGuest)
    : ViewExtensionInfoExplore(navigationHost, navigationGuest)
{
    /// <summary>
    /// Gets or sets the workspace id currently in the scope
    /// </summary>
    public string WorkspaceId { get; set; } = string.Empty;
}