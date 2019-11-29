using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    /// <summary>
    /// Defines the context of the query for the view extension.
    /// </summary>
    public enum ViewExtensionContext
    {
        /// <summary>
        /// For the root window, independent which element has been selected
        /// </summary>
        Application,

        /// <summary>
        /// The view extension is queried for a detailed element
        /// </summary>
        Detail,

        /// <summary>
        /// The view extension is queried for the root element of the Extension view
        /// </summary>
        Extent,

        /// <summary>
        /// The view extension is queried for a specific view in which he user has
        /// selected an element and all its properties are shown as lists
        /// </summary>
        View
    }

    /// <summary>
    /// Contains the information about the window or dialog in which the viewextension will be shown
    /// </summary>
    public class ViewExtensionTargetInformation
    {
        /// <summary>
        /// Initialies a new instance of the class
        /// </summary>
        /// <param name="context">Context used for the query</param>
        public ViewExtensionTargetInformation(ViewExtensionContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets or sets the navigation host querying the view extensions
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets or sets the navigation guest which is currently
        /// </summary>
        public INavigationGuest NavigationGuest { get; set; }

        /// <summary>
        /// Defines the context for the view
        /// </summary>
        public ViewExtensionContext Context { get; set; }
    }

    /// <summary>
    /// This class is used when an item list is required to show all properties of the given item.
    /// Here, the plugins are asked whether a view extension shall be shown for these items.
    /// </summary>
    public class ViewExtensionForItemPropertiesInformation : ViewExtensionTargetInformation
    {
        public ViewExtensionForItemPropertiesInformation(ViewExtensionContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets or sets the value which is intended to be shown
        /// </summary>
        public IObject Value { get; set; }
        
        /// <summary>
        /// Gets or sets the property which is queried to be shown
        /// </summary>
        public string Property { get; set; }
    }
}