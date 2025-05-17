using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;

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

        /// <summary>
        /// Goes through all the given view extensions and performs the necessary evaluation like
        /// creation of menus, context menus or other
        /// </summary>
        /// <param name="viewExtensions">View Extensions to be evaluated</param>
        void EvaluateViewExtensions(ICollection<ViewExtension> viewExtensions);

        /// <summary>
        ///  Updates the content
        /// </summary>
        void UpdateForm();
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