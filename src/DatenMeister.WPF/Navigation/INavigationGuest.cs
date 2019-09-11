using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base.ViewExtensions;

namespace DatenMeister.WPF.Navigation
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

    /// <summary>
    /// This interface allows the retrieval of the collection which is the focus of the current navigation guest
    /// </summary>
    public interface IExtentNavigationGuest
    {
        IExtent Extent { get; }
    }

    /// <summary>
    /// This interface allows the retrieval of the collection which is the focus of the current navigation guest
    /// </summary>
    public interface ICollectionNavigationGuest
    {
        IReflectiveCollection Collection { get; }
    }
    
    /// <summary>
    /// This interface allows the retrieval of an item which is the focus of the current navigation guest
    /// </summary>
    public interface IItemNavigationGuest
    {
        IObject Item { get; }
    }
}