using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    public class ViewExtensionInfoExploreItems : ViewExtensionInfoExplore
    {
        public string WorkspaceId { get; set; } = string.Empty;

        public string ExtentUrl { get; set; } = string.Empty;
        
        public ViewExtensionInfoExploreItems(INavigationHost navigationHost, INavigationGuest? navigationGuest) : base(
            navigationHost, navigationGuest)
        {
        }
    }
}