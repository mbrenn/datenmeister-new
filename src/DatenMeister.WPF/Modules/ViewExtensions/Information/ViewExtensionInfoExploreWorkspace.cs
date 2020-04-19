using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    public class ViewExtensionInfoExploreWorkspace : ViewExtensionInfoExplore
    {
        public ViewExtensionInfoExploreWorkspace(INavigationHost navigationHost, INavigationGuest? navigationGuest) :
            base(navigationHost, navigationGuest)
        {
        }
    }
}