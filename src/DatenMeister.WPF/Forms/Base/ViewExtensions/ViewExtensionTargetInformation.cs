using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    /// <summary>
    /// Contains the information about the window or dialog in which the viewextension will be shown
    /// </summary>
    public class ViewExtensionTargetInformation
    {
        /// <summary>
        /// Gets or sets the navigation host querying the view extensions
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets or sets the navigation guest which is currently 
        /// </summary>
        public INavigationGuest NavigationGuest { get; set; }
    }
}