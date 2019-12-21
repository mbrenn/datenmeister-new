using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewManager
{
    /// <summary>
    /// Defines the navigation support for the views
    /// </summary>
    public static class NavigatorForForms
    {
        /// <summary>
        /// Opens the select view dialog and performs the required action that will be executed upon the navigation guest
        /// </summary>
        /// <param name="navigationGuest"></param>
        public static void OpenSelectFormDialog(INavigationGuest navigationGuest)
        {
            _ = NavigatorForItems.NavigateToElementDetailViewAsync(
                navigationGuest.NavigationHost,
                null);
        }
    }
}