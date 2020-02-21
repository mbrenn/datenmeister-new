using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    public class ViewExtensionInfoExploreExtents : ViewExtensionInfoExplore
    {
        public ViewExtensionInfoExploreExtents(INavigationHost navigationHost, INavigationGuest? navigationGuest) :
            base(navigationHost, navigationGuest)
        {
        }
    }
}