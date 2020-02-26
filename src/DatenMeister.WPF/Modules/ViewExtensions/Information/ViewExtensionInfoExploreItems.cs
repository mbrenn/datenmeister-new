using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    public class ViewExtensionInfoExploreItems : ViewExtensionInfoExplore
    {
        public ViewExtensionInfoExploreItems(INavigationHost navigationHost, INavigationGuest? navigationGuest) : base(
            navigationHost, navigationGuest)
        {
        }
    }
}