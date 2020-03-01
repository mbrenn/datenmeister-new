#nullable enable
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    /// <summary>
    /// Contains the information about the window or dialog in which the viewextension will be shown
    /// </summary>
    public class ViewExtensionInfo
    {
        /// <summary>
        /// Gets or sets the navigation host querying the view extensions
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets or sets the navigation guest which is currently
        /// </summary>
        public INavigationGuest? NavigationGuest { get; set; }

        public ViewExtensionInfo(INavigationHost navigationHost, INavigationGuest? navigationGuest)
        {
            NavigationHost = navigationHost;
            NavigationGuest = navigationGuest;
        }
    }
}