namespace DatenMeisterWPF
{
    /// <summary>
    /// Defines the interface for the given controls. 
    /// The control can request certain navigation elements from the host
    /// (like ribbons or menus).
    /// </summary>
    public interface INavigationGuest
    {
        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        /// <param name="navigationHost">Host being used for navigation </param>
        void PrepareNavigation(INavigationHost navigationHost);
    }
}