using System.Collections.Generic;
using DatenMeisterWPF.Forms.Base.ViewExtensions;

namespace DatenMeisterWPF.Navigation
{
    /// <summary>
    /// Defines the interface for the given controls. 
    /// The control can request certain navigation elements from the host
    /// (like ribbons or menus).
    /// </summary>
    public interface INavigationGuest
    {
        /// <summary>
        /// Defines the navigation host
        /// </summary>
        INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        IEnumerable<ViewExtension> GetViewExtensions();
    }
}