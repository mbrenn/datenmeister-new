using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    /// <summary>
    /// This class is used when an item list is required to show all properties of the given item.
    /// Here, the plugins are asked whether a view extension shall be shown for these items.
    /// </summary>
    public class ViewExtensionItemPropertiesInformation : ViewExtensionInfo
    {
        /// <summary>
        /// Gets or sets the value which is intended to be shown
        /// </summary>
        public IObject? Value { get; set; }
        
        /// <summary>
        /// Gets or sets the property which is queried to be shown
        /// </summary>
        public string? Property { get; set; }

        public ViewExtensionItemPropertiesInformation(INavigationHost navigationHost, INavigationGuest navigationGuest) : base(navigationHost, navigationGuest)
        {
        }
    }
}